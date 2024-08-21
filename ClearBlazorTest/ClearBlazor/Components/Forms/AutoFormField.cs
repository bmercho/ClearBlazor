using System.Reflection;
using System.Collections;
using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// Item Type enumeration
    /// </summary>
    //  
    //class TestClass
    //{
    //    string Value1             -- ClassItem
    //    int Value2                -- ClassItem
    //    List<Test1Class> Test1    -- CollectionHeader
    //        [0]                   -- CollectionClassHeader
    //            int IntValue1     -- ClassItem   
    //            string StrValue1  -- ClassItem   
    //        [1]                   -- CollectionClassHeader
    //            int IntValue1     -- ClassItem   
    //            string StrValue1  -- ClassItem   
    //    [TableView]
    //    List<Test1Class> Test1    -- CollectionTableHeader    
    //    Test2Class Test2          -- ClassHeader
    //            int IntValue1     -- ClassItem   
    //            string StrValue1  -- ClassItem   
    //    Test2Class[] Test3        -- CollectionHeader
    //        [0]                   -- CollectionClassHeader
    //            int IntValue1     -- ClassItem   
    //            string StrValue1  -- ClassItem   
    //        [1]                   -- CollectionClassHeader
    //            int IntValue1     -- ClassItem   
    //            string StrValue1  -- ClassItem   
    //    List<string> Test4        -- CollectionHeader 
    //            [0] string        -- ListItem
    //            [1] string        -- ListItem
    //}
    //public class Test1Class
    //{
    //    public int IntValue1 { get; set; }
    //    public string StrValue1 { get; set; }
    //}
    public class AutoFormField
    {
        public InputBase Input { get; set; } = null!;

        private AutoForm _autoForm;

        private string _validationMessage = string.Empty;
        private bool _hasError = false;
        private bool _isVisible = true;
        private string _name = string.Empty;
        private string _displayName = string.Empty;
        private string _tooltip = string.Empty;
        private object? _instance;
        private bool _isExpanded = false;
        private bool _isReadOnly = false;
        private readonly Type? _propertyType = null;
        private readonly bool _hasGroupHeader = false;

        public AutoFormField(AutoForm autoForm,
                             AutoFormField? parent, ItemType itemType, string name,
                             string displayName, string tooltip, int depth,
                             PropertyInfo? propertyInfo,
                             object? instance, object? value,
                             int index, bool isReadOnly, bool hasGroupHeader)
        {
            _autoForm = autoForm;
            //IsSelected = false;
            //IsAdded = false;
            //IsDeleted = false;
            Name = name;
            DisplayName = displayName;
            _tooltip = tooltip;
            ItemType = itemType;
            PropertyInfo = propertyInfo;
            //Parent = parent;
            _instance = instance;
            //ParentObject = parentObject;
            InitialValue = value;
            if (propertyInfo == null)
                _propertyType = value == null ? null : value.GetType();
            else
                _propertyType = propertyInfo.PropertyType;
            Index = index;
            _isExpanded = false;
            Depth = depth;
            IsVisible = depth < 2 ? true : false;
            IsReadOnly = isReadOnly;
            //IsPassword = false;
            //_children = new List<AutoFormField>();
            //HasChildren = false;
            //HasParent = parent != null;
            //if (parent != null)
            //{
            //    Parent.Children.Add(this);
            //    Parent.HasChildren = true;
            //    parent.IsExpanded = parent.Depth < 1;
            //    IsExpanded = false;
            //}

            //_isRequired = GetRequiredAttr();

            //(CustomObjectDependantPropertyName, string _, string _) = AutoFormViewModel.GetCustomObjectAttribute(PropertyInfo);

            //(CustomValidationProperty, CustomValidationDependantProperty) = GetCustomValidateAttribute();

            //(SelectionsProperty, SelectionsDependantProperty) = GetSelectionsAttribute();

            //VisibilityDependantProperty = GetVisibiltyDependsOnAttribute();

            //IsGroupHeader = HasGroupPropertyAttribute();

            Value = value;

            //if (IsGroupHeader)
            //    if (Parent != null)
            //        Parent.DisplayName = Value as string;

            GetEditorTypeAndParameters();

            //_hasGroupHeader = hasGroupHeader;
        }

        // The depth of the property. This indicates the heirarchy.
        public int Depth { get; set; }

        public int Index { get; set; }

        public ItemType ItemType { get; set; }

        public Type? InputType { get; set; }

        public PropertyInfo? PropertyInfo { get; set; }

        public Dictionary<string, object?>? InputParameters { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    //OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string DisplayName
        {
            get
            {
                string? displayName = null;

                // If this property has a group header then show the name of the group header name
                if (_hasGroupHeader)
                {
                    if (_instance == null)
                        return (string.IsNullOrWhiteSpace(_displayName) ? _name : _displayName);

                    PropertyInfo[]? props = _instance.GetType()?.GetProperties();

                    if (props != null)
                    {
                        foreach (var prop in props)
                        {
                            if (prop.Name == _name)
                            {
                                var val = prop.GetValue(_instance, null) as string;
                                // If there is no group header name show the index number
                                if (string.IsNullOrEmpty(val))
                                {
                                    displayName = "[" + Index + "]";
                                }
                                else
                                    displayName = val;
                                break;
                            }
                        }
                    }
                }
                return displayName ?? (string.IsNullOrWhiteSpace(_displayName) ? _name : _displayName);
            }

            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    //OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                if (_isReadOnly != value)
                {
                    _isReadOnly = value;
                    //OnPropertyChanged(nameof(IsReadOnly));
                }
            }
        }

        // The instance of the property
        public object Instance
        {
            get
            {
                return _instance;
            }
        }
        public object? InitialValue { get; set; }

        object? _value;
        public object? Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public string TooltipForName
        {
            get
            {
                if (_tooltip != null)
                    return _tooltip;
                else
                    return string.Empty;
            }
            set
            {
                if (_tooltip != value)
                {
                    _tooltip = value;
                    //**OnPropertyChanged(nameof(TooltipForName));
                }
            }
        }

        public string TooltipForValue
        {
            get
            {
                if (HasError)
                    return ValidationMessage;
                else if (_tooltip != null)
                    return _tooltip;
                else
                    return string.Empty;
            }
            set
            {
                if (_tooltip != value)
                {
                    _tooltip = value;
                    //**OnPropertyChanged(nameof(TooltipForValue));
                }
            }
        }

        public string ValidationMessage
        {
            get { return _validationMessage; }
            set
            {
                if (_validationMessage != value)
                {
                    _validationMessage = value;
                    //**CheckTooltipParameter();
                    //**OnPropertyChanged(nameof(ValidationMessage));
                    //**_autoFormViewModel.RefreshUI();
                }
            }
        }

        public bool HasError
        {
            get { return _hasError; }
            set
            {
                if (_hasError != value)
                {
                    _hasError = value;
                    //OnPropertyChanged(nameof(HasError));
                    //**CheckErrors();
                    //**_autoFormViewModel.RefreshUI();
                }
            }
        }

        // Indicates if this property is visible. Used to hide and show children
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    //OnPropertyChanged("IsVisible");
                }
            }
        }
        // Indicates if the property is a header item and the children are expanded
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                //**SetVisibility(Children, _isExpanded ? true : false);
            }
        }

        public void GetEditorTypeAndParameters()
        {
            InputParameters = new Dictionary<string, object?>();

            //if (ItemType == ItemType.CollectionTableHeader)
            //{
            //    InputType = typeof(InputTableEditor);
            //    return;
            //}

            InputParameters.Add("FieldName", PropertyInfo?.Name);

            if (_propertyType == typeof(bool))
            {
                InputParameters.Add("Label", _autoForm.HasLabelColumn ? string.Empty : DisplayName);
                var tristate = PropertyInfo?.GetCustomAttribute<CheckBoxTriStateAttribute>();
                InputParameters.Add("Size", _autoForm.Size);
                InputParameters.Add("IsReadOnly", IsReadOnly);
                InputParameters.Add("CheckedChanged", EventCallback.Factory.Create<bool>(this, ValueChanged));
                InputParameters.Add("Checked", Value);

                AddToolTipParameter();
                InputType = typeof(CheckBox<bool>);
                return;
            }

            if (_propertyType == typeof(bool?))
            {
                InputParameters.Add("Label", _autoForm.HasLabelColumn ? string.Empty : DisplayName);
                var tristate = PropertyInfo?.GetCustomAttribute<CheckBoxTriStateAttribute>();
                if (tristate != null)
                    InputParameters.Add("TriState", true);
                InputParameters.Add("Size", _autoForm.Size);
                InputParameters.Add("IsReadOnly", IsReadOnly);
                InputParameters.Add("CheckedChanged", EventCallback.Factory.Create<bool?>(this, ValueChanged));
                InputParameters.Add("Checked", Value);

                AddToolTipParameter();

                InputType = typeof(CheckBox<bool?>);
                return;
            }

            if (_propertyType == typeof(sbyte))
            {
                GetNumericEdit(typeof(NumericInput<sbyte>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<sbyte>(this, ValueChanged));
                return;
            }
            if (_propertyType == typeof(sbyte?))
            {
                GetNumericEdit(typeof(NumericInput<sbyte?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<sbyte?>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(byte))
            {
                GetNumericEdit(typeof(NumericInput<byte>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<byte>(this, ValueChanged));
                return;
            }
            if (_propertyType == typeof(byte?))
            {
                GetNumericEdit(typeof(NumericInput<byte?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<byte?>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(short))
            {
                GetNumericEdit(typeof(NumericInput<short>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<short>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(short?))
            {
                GetNumericEdit(typeof(NumericInput<short?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<short?>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(int))
            {
                GetNumericEdit(typeof(NumericInput<int>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<int>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(int?))
            {
                GetNumericEdit(typeof(NumericInput<int?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<int?>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(uint))
            {
                GetNumericEdit(typeof(NumericInput<uint>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<uint>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(uint?))
            {
                GetNumericEdit(typeof(NumericInput<uint?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<uint?>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(long))
            {
                GetNumericEdit(typeof(NumericInput<long>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<long>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(long?))
            {
                GetNumericEdit(typeof(NumericInput<long?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<long?>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(ulong))
            {
                GetNumericEdit(typeof(NumericInput<ulong>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<ulong>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(ulong?))
            {
                GetNumericEdit(typeof(NumericInput<ulong?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<ulong?>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(float))
            {
                AddNumDecimalPlaces(PropertyInfo);
                GetNumericEdit(typeof(NumericInput<float>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<float>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(float?))
            {
                AddNumDecimalPlaces(PropertyInfo);
                GetNumericEdit(typeof(NumericInput<float?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<float?>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(double))
            {
                AddNumDecimalPlaces(PropertyInfo);
                GetNumericEdit(typeof(NumericInput<double>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<double>(this, ValueChanged));
                return;
            }

            if (_propertyType == typeof(double?))
            {
                AddNumDecimalPlaces(PropertyInfo);
                GetNumericEdit(typeof(NumericInput<double?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<double?>(this, ValueChanged));
                return;
            }

            if (PropertyInfo?.PropertyType == typeof(decimal))
            {
                AddNumDecimalPlaces(PropertyInfo);
                GetNumericEdit(typeof(NumericInput<decimal>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<decimal>(this, ValueChanged));
                return;
            }

            if (PropertyInfo?.PropertyType == typeof(decimal?))
            {
                AddNumDecimalPlaces(PropertyInfo);
                GetNumericEdit(typeof(NumericInput<decimal?>));
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<decimal?>(this, ValueChanged));
                return;
            }

            if (_propertyType == null)
                return;

            if (_propertyType.IsEnum || IsNullableEnum(_propertyType))
            {
                bool nullable = IsNullableEnum(_propertyType);

                if (_propertyType.IsDefined(typeof(FlagsAttribute), inherit: true))
                {
                    Type dataType = typeof(ListDataItem<>).MakeGenericType(_propertyType);
                    var data = GetEnumData(dataType, _propertyType);

                    InputParameters.Add("SelectData", data);
                    InputParameters.Add("MultiSelect", true);
                    InputParameters.Add("Value", Value);
                    InputType = typeof(Select<>).MakeGenericType(_propertyType);

                    var callback = typeof(AutoFormField)
                        .GetMethod("CreateCallback")?
                        .MakeGenericMethod(_propertyType)
                        .Invoke(this, null);

                    if (callback != null)
                        InputParameters.Add("ValueChanged", callback);
                    return;

                }
                else
                {
                    Type dataType = typeof(RadioGroupDataItem<>).MakeGenericType(_propertyType);
                    var data = GetEnumData(dataType, _propertyType);

                    InputParameters.Add("RadioGroupData", data);

                    InputParameters.Add("Value", Value);
                    InputParameters.Add("Label", _autoForm.HasLabelColumn ? string.Empty : DisplayName);

                    InputType = typeof(RadioGroup<>).MakeGenericType(_propertyType);

                    var callback = typeof(AutoFormField)
                        .GetMethod("CreateCallback")?
                        .MakeGenericMethod(_propertyType)
                        .Invoke(this, null);

                    if (callback != null)
                        InputParameters.Add("ValueChanged", callback);
                    return;

                }
            }

            //if (_propertyType == typeof(DateTime) || _propertyType == typeof(DateTime?))
            //{
            //    CheckFormatAttribute();
            //    if (_propertyType == typeof(DateTime))
            //        InputType = typeof(InputDateEditor<DateTime>);
            //    else
            //        InputType = typeof(InputDateEditor<DateTime?>);
            //    CheckPlaceholderAttribute();
            //    AddToolTipParameter();
            //    return;
            //}

            //if (_propertyType == typeof(TimeSpan) || _propertyType == typeof(TimeSpan?))
            //{
            //    CheckFormatAttribute();
            //    if (_propertyType == typeof(TimeSpan))
            //        InputType = typeof(InputTimeEditor<TimeSpan>);
            //    else
            //        InputType = typeof(InputTimeEditor<TimeSpan?>);
            //    CheckPlaceholderAttribute();
            //    AddToolTipParameter();
            //    return;
            //}

            //if (_propertyType == typeof(DateTime) || _propertyType == typeof(DateTime?))
            //{
            //    CheckFormatAttribute();
            //    var dataType = PropertyInfo?.GetCustomAttribute<DataTypeAttribute>();
            //    if (dataType != null)
            //    {
            //        if (dataType.DataType == DataType.DateTime)
            //        {
            //            InputParameters.Add("ShowDate", true);
            //            InputParameters.Add("ShowTime", true);
            //            if (_propertyType == typeof(DateTime))
            //                InputType = typeof(InputDateTimeEditor<DateTime>);
            //            else
            //                InputType = typeof(InputDateTimeEditor<DateTime?>);
            //            return;
            //        }
            //        else if (dataType.DataType == DataType.Time)
            //        {
            //            InputParameters.Add("ShowTime", true);
            //            if (_propertyType == typeof(DateTime))
            //                InputType = typeof(InputDateTimeEditor<DateTime>);
            //            else
            //                InputType = typeof(InputDateTimeEditor<DateTime?>);
            //            return;
            //        }
            //    }
            //    InputParameters.Add("ShowDate", true);
            //    if (_propertyType == typeof(DateTime))
            //        InputType = typeof(InputDateTimeEditor<DateTime>);
            //    else
            //        InputType = typeof(InputDateTimeEditor<DateTime?>);
            //    return;
            //}


            //if (_propertyType == typeof(DateTimeOffset) || (_propertyType == typeof(DateTimeOffset?)))
            //{
            //    CheckFormatAttribute();
            //    var dataType = PropertyInfo?.GetCustomAttribute<DataTypeAttribute>();
            //    if (dataType != null)
            //    {
            //        if (dataType.DataType == DataType.DateTime)
            //        {
            //            InputParameters.Add("ShowTime", true);
            //            if (_propertyType == typeof(DateTimeOffset))
            //                InputType = typeof(InputDateTimeEditor<DateTimeOffset>);
            //            else
            //                InputType = typeof(InputDateTimeEditor<DateTimeOffset?>);
            //            return;
            //        }
            //        else if (dataType.DataType == DataType.Time)
            //        {
            //            InputParameters.Add("ShowTime", true);
            //            InputParameters.Add("TimeOnly", true);
            //            if (_propertyType == typeof(DateTimeOffset))
            //                InputType = typeof(InputDateTimeEditor<DateTimeOffset>);
            //            else
            //                InputType = typeof(InputDateTimeEditor<DateTimeOffset?>);
            //            return;
            //        }
            //    }
            //    if (_propertyType == typeof(DateTimeOffset))
            //        InputType = typeof(InputDateTimeEditor<DateTimeOffset>);
            //    else
            //        InputType = typeof(InputDateTimeEditor<DateTimeOffset?>);
            //    return;
            //}

            if (_propertyType == typeof(string))
            {
                //if (IsColourAttribute(PropertyInfo))
                //{
                //    CheckPlaceholderAttribute();
                //    InputParameters.Add("LabelShown", _autoFormViewModel.ShowLabels);
                //    AddToolTipParameter();
                //    InputType = typeof(InputColourEditor);
                //    return;
                //}

                //var image = GetImageAttribute(PropertyInfo);
                //if (image.Item1)
                //{
                //    InputType = typeof(ImageEditor);
                //    CheckPlaceholderAttribute();
                //    InputParameters.Add("Width", image.Item2);
                //    EditorParInputParametersameters.Add("Height", image.Item3);
                //    InputParameters.Add("DefaultSource", image.Item4);
                //    InputParameters.Add("AllowSelection", IsReadOnly ? false : image.Item5);
                //    return;
                //}

                CheckPlaceholderAttribute();

                //var selectionValues = GetSelections(_instance);
                //if (selectionValues != null)
                //{
                //    InputType = typeof(InputSelectionEditor);
                //    InputParameters.Add("SelectionValues", selectionValues);
                //    return;
                //}

                //(bool attribExists, bool hashPassword) = GetPasswordAttribute(PropertyInfo);
                //if (attribExists)
                //{
                //    IsPassword = true;
                //    IsPasswordHashed = hashPassword;
                //    InputType = typeof(PasswordEditor);
                //    AddToolTipParameter();
                //    InputParameters.Add("IsReadOnly", IsReadOnly);
                //    InputParameters.Add("HashPassword", hashPassword);
                //    return;
                //}
            }

            InputType = typeof(TextInput);
            InputParameters.Add("Label", _autoForm.HasLabelColumn ? string.Empty : DisplayName);
            InputParameters.Add("IsReadOnly", IsReadOnly);
            var numRowsAttrib = PropertyInfo?.GetCustomAttribute<NumRowsAttribute>();
            if (numRowsAttrib != null)
            {
                InputParameters.Add("NumLines", numRowsAttrib.NumRows);
            }
            //var requiredAttribute = PropertyInfo?.GetCustomAttribute<RequiredAttribute>();
            //if (requiredAttribute != null)
            //{
            //    InputParameters.Add("IsRequired", !requiredAttribute.AllowEmptyStrings);
            //}
            InputParameters.Add("Size", _autoForm.Size);
            InputParameters.Add("TextEditFillMode", _autoForm.TextEditFillMode);
            InputParameters.Add("Immediate", _autoForm.Immediate);
            InputParameters.Add("Value", Value);
            InputParameters.Add("ValueChanged", EventCallback.Factory.Create<string>(this, ValueChanged));

            AddToolTipParameter();
        }

        public EventCallback<T> CreateCallback<T>()
        {
            return EventCallback.Factory.Create<T>(this, ValueChanged);
        }
        public T CastObject<T>(object input)
        {
            return (T)input;
        }
        public bool IsNullableEnum(Type t)
        {
            Type? u = Nullable.GetUnderlyingType(t);
            return u != null && u.IsEnum;
        }
        private void ValueChanged<T>(T value)
        {
            Value = value;
            PropertyInfo?.SetValue(Instance, value);
        }

        private IList? GetEnumData(Type dataType, Type propertyType)
        {
            IList? data = null;

            if (dataType != null)
            {
                Type customType = typeof(List<>).MakeGenericType(dataType);

                if (customType != null)
                {
                    data = (IList?)Activator.CreateInstance(customType);

                    if (data != null)
                    {
                        foreach (var item in Enum.GetValues(propertyType))
                            if (item != null)
                            {
                                dynamic? o = Activator.CreateInstance(dataType);
                                if (o != null)
                                {
                                    o.Name = item.ToString();
                                    o.Value = (dynamic)Convert.ChangeType(item, propertyType);
                                    data.Add(o);
                                }
                            }
                    }
                }
            }
            return data;
        }
        private void GetNumericEdit(Type t)
        {
            CheckPlaceholderAttribute();
            InputParameters?.Add("Label", _autoForm.HasLabelColumn ? string.Empty : DisplayName);
            InputParameters?.Add("IsReadOnly", IsReadOnly);
            InputParameters?.Add("Size", _autoForm.Size);
            InputParameters?.Add("TextEditFillMode", _autoForm.TextEditFillMode);
            InputParameters?.Add("Immediate", _autoForm.Immediate);
            InputParameters?.Add("Value", Value);
            AddToolTipParameter();
            InputType = t;
        }

        private void AddNumDecimalPlaces(PropertyInfo? prop)
        {
            var numDecimalPointsPlacesAttrib = prop?.GetCustomAttribute<NumDecimalPlacesAttribute>();
            if (numDecimalPointsPlacesAttrib != null)
                InputParameters?.Add("DecimalPlaces", numDecimalPointsPlacesAttrib.NumDecimalPlaces);
        }

        private void CheckPlaceholderAttribute()
        {
            var placeholder = PropertyInfo?.GetCustomAttribute<PlaceholderAttribute>();
            if (placeholder != null)
                InputParameters?.Add("Placeholder", placeholder.Placeholder);
        }


        private void AddToolTipParameter()
        {
            InputParameters?.Add("ToolTip", TooltipForValue);
        }

        public bool IsContractVisible => IsExpanded ? true : false;

        public bool IsExpandVisible => ((ItemType == ItemType.CollectionClassHeader || ItemType == ItemType.ClassHeader ||
                                                    (ItemType == ItemType.CollectionHeader && ((IList)Instance!).Count > 0)) &&
                                                   !IsExpanded) ? true : false;


    }
}
