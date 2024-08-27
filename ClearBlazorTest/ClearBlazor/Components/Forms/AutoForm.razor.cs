using Microsoft.AspNetCore.Components;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace ClearBlazor
{
    public partial class AutoForm : Form
    {
        [Parameter]
        public bool HasLabelColumn { get; set; } = false;

        [Parameter]
        public string LabelValueCols { get; set; } = "*,*";

        [Parameter]
        public TextEditFillMode TextEditFillMode { get; set; } = TextEditFillMode.Underline;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public bool Immediate { get; set; } = false;

        private string _cols = "*,*";

        List<AutoFormField> Fields = new List<AutoFormField>();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (HasLabelColumn)
                _cols = LabelValueCols;
            else
                _cols = "0,*";

            if (Model != null)
            {
                await ParseModel(Model, ReadOnly);
                StateHasChanged();
            }

        }

        public TypographyBase GetLabelTypography()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return ThemeManager.CurrentTheme.Typography.InputLabelVerySmall;
                case Size.Small:
                    return ThemeManager.CurrentTheme.Typography.InputLabelSmall;
                case Size.Normal:
                    return ThemeManager.CurrentTheme.Typography.InputLabelNormal;
                case Size.Large:
                    return ThemeManager.CurrentTheme.Typography.InputLabelLarge;
                case Size.VeryLarge:
                    return ThemeManager.CurrentTheme.Typography.InputLabelVeryLarge;
            }
            return ThemeManager.CurrentTheme.Typography.InputLabelNormal;
        }

        public string GetMargin(AutoFormField field)
        {
            var margin = 24 * (field.Depth - 1);// + 25;
            return $"10,0,0,{margin}";
        }

        public int GetFieldIndex(AutoFormField field)
        {
            int index = 0;
            foreach (var f in Fields)
            {
                if (f == field)
                    return index;
                else if (f.IsVisible)
                    index++;
            }
            return -1;
        }

        private async Task ParseModel(object model, bool readOnly)
        {
            Fields.Clear();
            Type modelType = model.GetType();

            PropertyInfo[] props = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            await ParseChildren(null, model, 1, readOnly);

        }

        // Recursively parse all the children of the given object
        private async Task ParseChildren(AutoFormField? parent, object parentObj, int depth, bool readOnly)
        {
            if (parentObj == null)
                return;

            AutoFormField field;

            Type type = parentObj.GetType();

            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (GetBrowsable(prop))
                {
                    if (prop.PropertyType.IsClass || prop.PropertyType.IsArray)
                    {
                        if (prop.PropertyType.IsArray || GetEnumerableType(prop.PropertyType) != null)
                        {
                            await ParseListOrArray(parent, parentObj, depth, readOnly, prop);
                        }
                        else
                        {
                            object? v = prop.GetValue(parentObj, null);

                            if (prop.PropertyType == typeof(string))// || prop.PropertyType == typeof(FontFamily) || prop.PropertyType == typeof(ColorContext))
                            {
                                field = new AutoFormField(this, parent, ItemType.ClassItem, prop.Name,
                                                           GetDisplayName(prop, props, parentObj),
                                                           GetDescriptionAttribute(prop),
                                                           depth, prop, parentObj,
                                                           v, -1, readOnly || IsReadOnly(prop), false);
                                AddField(parent, field);
                                await ParseChildren(field, v, depth + 1, readOnly);
                            }
                            else
                            {
                                //v = await ParseClass(parent, parentObj, depth, readOnly, v, prop);
                            }
                        }
                    }
                    else if (prop.PropertyType.IsValueType && !(parentObj is string))
                    {
                        await ParseSimpleType(parent, parentObj, depth, readOnly, prop);
                    }
                }
            }
        }

        // Recursively parses a simple type object
        private async Task ParseSimpleType(AutoFormField parent, object obj, int depth, bool readOnly, PropertyInfo prop)
        {
            object? v = prop.GetValue(obj, null);

            var field = (new AutoFormField(this, parent, ItemType.ClassItem, prop.Name,
                GetDisplayNameAttribute(prop),
                GetDescriptionAttribute(prop),
                depth, prop, obj, v, -1,
                readOnly || IsReadOnly(prop), false));
            int index = AddField(parent, field);
            if (v is DateTime || v is Color)
                return;
            await ParseChildren(Fields[index], v, depth + 1, readOnly);
        }

        // Recursively parses a list of an array object
        private async Task ParseListOrArray(AutoFormField? parent, object parentObj, int depth, bool readOnly, PropertyInfo prop)
        {
            AutoFormField field;
            object? v = prop.GetValue(parentObj, null);
            if (v == null)
            {
                if (prop.PropertyType.IsArray)
                {
                    v = Activator.CreateInstance(prop.PropertyType, new object[] { 0 }); // Length 0
                    prop.SetValue(parentObj, v, null);
                }
                else
                {
                    v = Activator.CreateInstance(prop.PropertyType);
                    prop.SetValue(parentObj, v, null);
                }
            }

            IList? arr = v as IList;

            bool tableView = GetTableView(prop);
            if (tableView)
            {
                Type? type = GetEnumerableType(prop.PropertyType);
                PropertyInfo[]? props = type == null ? null : type.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(x => x.MetadataToken).ToArray();

                if (props != null)
                    foreach (var prop1 in props)
                    {
                        if (prop1.PropertyType.IsClass || prop.PropertyType.IsArray)
                        {
                            if (prop1.PropertyType.IsArray || GetEnumerableType(prop1.PropertyType) != null)
                            {
                                tableView = false;
                                break;
                            }
                            else
                                if (prop1.PropertyType != typeof(string))
                            {
                                tableView = false;
                                break;
                            }
                        }
                    }
            }

            if (tableView)
                field = new AutoFormField(this, parent, ItemType.CollectionTableHeader, prop.Name,
                                          GetDisplayNameAttribute(prop),
                                          GetDescriptionAttribute(prop),
                                          depth, prop, parentObj, v, -1,
                                          readOnly || IsReadOnly(prop),
                                          false);
            else
                field = new AutoFormField(this, parent, ItemType.CollectionHeader, prop.Name,
                                          GetDisplayNameAttribute(prop),
                                          GetDescriptionAttribute(prop),
                                          depth, prop, v, "(Collection)", -1,
                                          readOnly || IsReadOnly(prop),
                                          false);
            int index = AddField(parent, field);
            var parentPropInfo = Fields[index];

            if (!tableView)
            {
                await ParseListOrArrayItems(parentPropInfo, arr, depth + 1, readOnly, prop);
            }
        }

        private async Task ParseListOrArrayItems(AutoFormField parent, object obj, int depth, bool readOnly, PropertyInfo prop)
        {
            AutoFormField field;
            IList? list = obj as IList;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    int num = i + 1;
                    object? element = list[i];
                    if (element != null && element.GetType().IsClass && element.GetType() != typeof(string))
                    {
                        object? newElement = element;
                        if (element != null)  // ??? was element == null
                        {
                            newElement = Activator.CreateInstance(element.GetType());
                            list[i] = newElement;
                        }
                        string? groupHeader = GetGroupHeader(element);
                        field =
                            new AutoFormField(this, parent,
                                ItemType.CollectionClassHeader,
                                groupHeader ?? "[" + num + "]",
                                GetDisplayNameAttribute(prop),
                                GetDescriptionAttribute(prop),
                                depth,
                                null,
                                newElement,
                                "",
                                i,
                                readOnly || IsReadOnly(prop),
                                groupHeader != null);
                        AddField(parent, field);
                        await ParseChildren(Fields[Fields.IndexOf(field)], element, depth + 2,
                                      readOnly);

                    }
                    else
                    {
                        field = new AutoFormField(this, parent, ItemType.ListItem, "[" + num + "]",
                            GetDisplayNameAttribute(prop),
                            GetDescriptionAttribute(prop),
                            depth, null, list, element, i,
                            readOnly || IsReadOnly(prop), false);
                        AddField(parent, field);
                    }
                }
            }
        }

        private bool GetTableView(PropertyInfo prop)
        {
            bool result = false;
            if (prop != null)
            {
                var attribs = prop.GetCustomAttributes(true);
                foreach (var attrib in attribs)
                {
                    var a = attrib as TableViewAttribute;
                    if (a != null)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        // Recursively parses a class type object
        //private async Task<object> ParseClass(AutoFormField parent, object obj, int depth, bool readOnly, object v, PropertyInfo prop)
        //{
        //    try
        //    {
        //        (string _, string customObjectProperty1, string customObjectProperty2) = GetCustomObjectAttribute(prop);

        //        if (customObjectProperty1 != null && customObjectProperty2 != null)
        //        {
        //            Type type = obj.GetType();
        //            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //            string param1 = null;
        //            string param2 = null;
        //            long id = 0;
        //            foreach (var p in props)
        //            {
        //                if (p.Name == "Id")
        //                    id = (long)p.GetValue(obj);
        //                if (p.Name == customObjectProperty1)
        //                    param1 = (string)p.GetValue(obj);
        //                if (p.Name == customObjectProperty2)
        //                    param2 = (string)p.GetValue(obj);
        //            }
        //            v = await (_parentModel as IViewModelBase)?.GetViewSettings(id, param1, param2);
        //            if (v == null)
        //                return v;
        //            prop.SetValue(obj, v, null);
        //        }

        //        if (v == null)
        //        {
        //            v = Activator.CreateInstance(prop.PropertyType);
        //            prop.SetValue(obj, v, null);
        //        }
        //        var field = (new AutoFormField(this, parent, ItemType.ClassHeader, prop.Name,
        //                                                GetDisplayNameAttribute(prop),
        //                                                GetDescriptionAttribute(prop),
        //                                                depth, prop, obj, v, _parentModel, -1,
        //                                                readOnly || IsReadOnly(prop), false));
        //        AddField(parent, field);

        //        await ParseChildren(field, v, depth + 1, readOnly);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return v;
        //}

        private int AddField(AutoFormField parent, AutoFormField newField)
        {
            var index = GetNextIndexForDepth(parent);

            Fields.Insert(index, newField);
            return index;
        }

        private void AddField(AutoFormField newField, int index)
        {
            Fields.Insert(index, newField);
        }

        private int GetNextIndexForDepth(AutoFormField field)
        {
            var index = field == null ? 0 : Fields.IndexOf(field) + 1;
            var depth = field == null ? 0 : field.Depth;
            var numFields = Fields.Count;
            while (index < numFields && Fields[index].Depth > depth)
                index++;
            return index;
        }

        //Gets the group header property name for the given property
        private string GetGroupHeader(object? element)
        {
            string? result = null;
            if (element != null)
            {
                Type type = element.GetType();

                var props = type.GetProperties();

                foreach (var prop in props)
                {
                    var attribs = prop.GetCustomAttributes(true);
                    foreach (var attrib in attribs)
                    {
                        var a = attrib as GroupHeaderAttribute;
                        if (a != null)
                        {
                            result = prop.Name;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private string GetDisplayName(PropertyInfo prop, PropertyInfo[] properties, object parent)
        {
            return GetDisplayNameAttribute(prop);
        }

        // Gets the display name for the property
        private string? GetDisplayNameAttribute(PropertyInfo prop)
        {
            string? result = null;
            var attribs = prop.GetCustomAttributes(true);
            foreach (var attrib in attribs)
            {
                var a = attrib as DisplayNameAttribute;
                if (a != null)
                {
                    result = a.DisplayName;
                    break;
                }
            }
            return result;

        }

        // Gets the description for the property
        private string? GetDescriptionAttribute(PropertyInfo prop)
        {
            string? result = null;
            object[]? attribs = prop.GetCustomAttributes(true);
            foreach (var attrib in attribs)
            {
                var a = attrib as DescriptionAttribute;
                if (a != null)
                {
                    result = a.Description;
                    break;
                }
            }
            return result;
        }

        // Returns true if the property is Browsable
        private bool GetBrowsable(PropertyInfo prop)
        {
            bool result = true;
            if (prop != null)
            {
                var attribs = prop.GetCustomAttributes(true);
                foreach (var attrib in attribs)
                {
                    var a = attrib as BrowsableAttribute;
                    if (a != null)
                    {
                        result = a.Browsable;
                        break;
                    }
                }
            }
            return result;
        }

        // Gets the type of the list. Returns null if not enumerable
        private static Type? GetEnumerableType(Type type)
        {
            if (type != typeof(string))
            {
                foreach (Type intType in type.GetInterfaces())
                {
                    if (intType.IsGenericType &&
                        intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        return intType.GetGenericArguments()[0];
                    }
                }
            }
            return null;
        }

        // Returns true if given property is read only
        private bool IsReadOnly(PropertyInfo prop)
        {
            bool result = false;
            if (prop != null)
            {
                var attribs = prop.GetCustomAttributes(true);
                foreach (var attrib in attribs)
                {
                    var a = attrib as ReadOnlyAttribute;
                    if (a != null)
                    {
                        result = a.IsReadOnly;
                        break;
                    }
                }
            }
            return result;
        }

    }
}