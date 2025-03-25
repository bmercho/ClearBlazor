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
        public object? Instance
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
            object? callback;

            if (_propertyType == null)
                return;

            InputParameters = new Dictionary<string, object?>
            {
                { "FieldName", PropertyInfo?.Name }
            };

            if (PropertyInfo?.GetCustomAttribute<UseSliderAttribute>() != null && IsNumberType(_propertyType))
            {
                var sliderAttrib = PropertyInfo?.GetCustomAttribute<UseSliderAttribute>();
                InputParameters.Add("Label", _autoForm.HasLabelColumn ? null: DisplayName);
                InputParameters.Add("Size", _autoForm.Size);
                InputParameters.Add("IsReadOnly", IsReadOnly);
                InputParameters.Add("Value", Value);
                callback = typeof(AutoFormField)
                              .GetMethod("CreateCallback")?
                              .MakeGenericMethod(_propertyType)
                              .Invoke(this, null);

                if (callback != null)
                    InputParameters.Add("ValueChanged", callback);

                if (sliderAttrib != null)
                {
                    InputParameters.Add("Max", sliderAttrib.Max);
                    InputParameters.Add("Min", sliderAttrib.Min);
                    InputParameters.Add("Step", sliderAttrib.Step);
                    InputParameters.Add("ShowTickMarkLabels", sliderAttrib.ShowTickMarkLabels);
                    InputParameters.Add("ShowTickMarks", sliderAttrib.ShowTickMarks);
                }
                else
                {
                    InputParameters.Add("Max", 0);
                    InputParameters.Add("Min", 100);
                    InputParameters.Add("Step", 1);
                    InputParameters.Add("ShowTickMarkLabels", true);
                    InputParameters.Add("ShowTickMarks", true);
                }

                InputType = typeof(Slider<>).MakeGenericType(_propertyType);

                return;
            }

            if (_propertyType == typeof(bool))
            {
                if (PropertyInfo?.GetCustomAttribute<UseSwitchAttribute>() != null)
                {
                    InputParameters.Add("Label", _autoForm.HasLabelColumn ? null : DisplayName);
                    InputParameters.Add("Size", _autoForm.Size);
                    InputParameters.Add("IsReadOnly", IsReadOnly);
                    InputParameters.Add("CheckedChanged", EventCallback.Factory.Create<bool>(this, ValueChanged));
                    InputParameters.Add("Checked", Value);
                    InputType = typeof(Switch);
                }
                else
                {

                    InputParameters.Add("Label", _autoForm.HasLabelColumn ? null : DisplayName);
                    var tristate = PropertyInfo?.GetCustomAttribute<CheckBoxTriStateAttribute>();
                    InputParameters.Add("Size", _autoForm.Size);
                    InputParameters.Add("IsReadOnly", IsReadOnly);
                    InputParameters.Add("CheckedChanged", EventCallback.Factory.Create<bool>(this, ValueChanged));
                    InputParameters.Add("Checked", Value);

                    AddToolTipParameter();
                    InputType = typeof(CheckBox<bool>);
                }
                return;
            }

            if (_propertyType == typeof(bool?))
            {
                InputParameters.Add("Label", _autoForm.HasLabelColumn ? null : DisplayName);
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

            Type dataType = null!;
            IList? data;
           
            if (_propertyType.IsEnum || IsNullableEnum(_propertyType))
            {
                bool nullable = IsNullableEnum(_propertyType);

                if (_propertyType.IsDefined(typeof(FlagsAttribute), inherit: true))
                {
                    dataType = typeof(ListDataItem<>).MakeGenericType(_propertyType);
                    data = GetEnumData(dataType, _propertyType);

                    InputParameters.Add("SelectData", data);
                    InputParameters.Add("MultiSelect", true);
                    InputParameters.Add("InputContainerStyle", _autoForm.InputContainerStyle);
                    InputParameters.Add("Value", Value);
                    InputType = typeof(Select<>).MakeGenericType(_propertyType);

                    callback = typeof(AutoFormField)
                        .GetMethod("CreateCallback")?
                        .MakeGenericMethod(_propertyType)
                        .Invoke(this, null);

                    if (callback != null)
                        InputParameters.Add("ValueChanged", callback);
                    return;

                }
                if (PropertyInfo?.GetCustomAttribute<UseDropDownAttribute>() != null)
                {
                    dataType = typeof(ListDataItem<>).MakeGenericType(_propertyType);
                    data = GetEnumData(dataType, _propertyType);

                    InputParameters.Add("SelectData", data);
                    InputParameters.Add("Value", Value);
                    InputParameters.Add("InputContainerStyle", _autoForm.InputContainerStyle);
                    InputType = typeof(Select<>).MakeGenericType(_propertyType);

                    callback = typeof(AutoFormField)
                        .GetMethod("CreateCallback")?
                        .MakeGenericMethod(_propertyType)
                        .Invoke(this, null);

                    if (callback != null)
                        InputParameters.Add("ValueChanged", callback);
                    return;

                }

                dataType = typeof(RadioGroupDataItem<>).MakeGenericType(_propertyType);
                data = GetEnumData(dataType, _propertyType);

                InputParameters.Add("RadioGroupData", data);

                InputParameters.Add("Value", Value);
                InputParameters.Add("Label", _autoForm.HasLabelColumn ? null : DisplayName);

                InputType = typeof(RadioGroup<>).MakeGenericType(_propertyType);

                callback = typeof(AutoFormField)
                    .GetMethod("CreateCallback")?
                    .MakeGenericMethod(_propertyType)
                    .Invoke(this, null);

                if (callback != null)
                    InputParameters.Add("ValueChanged", callback);
                return;
            }

            if (_propertyType == typeof(Color))
            {
                InputParameters.Add("Value", Value);
                InputParameters.Add("ValueChanged", EventCallback.Factory.Create<Color>(this, ValueChanged));
                InputParameters.Add("Immediate", _autoForm.Immediate);
                InputParameters.Add("InputContainerStyle", _autoForm.InputContainerStyle);
                if (PropertyInfo?.GetCustomAttribute<ShowHexColorAttribute>() != null)
                    InputParameters.Add("ShowHex", true);

                InputType = typeof(ColorPickerInput);
                return;
            }

            if (_propertyType == typeof(DateOnly) || _propertyType == typeof(DateOnly?))
            {
                InputParameters.Add("Value", Value);
                if (_propertyType == typeof(DateOnly))
                    InputParameters.Add("ValueChanged", EventCallback.Factory.Create<DateOnly>(this, ValueChanged));
                else
                    InputParameters.Add("ValueChanged", EventCallback.Factory.Create<DateOnly?>(this, ValueChanged));
                InputParameters.Add("Immediate", _autoForm.Immediate);
                InputParameters.Add("InputContainerStyle", _autoForm.InputContainerStyle);
                var dateFormatAttrib = PropertyInfo?.GetCustomAttribute<DateFormatAttribute>();
                if (dateFormatAttrib != null)
                    InputParameters.Add("DateFormat", dateFormatAttrib.DateFormat);
                var orientationAttrib = PropertyInfo?.GetCustomAttribute<OrientationAttribute>();
                if (orientationAttrib != null)
                    InputParameters.Add("Orientation", orientationAttrib.Orientation);

                InputType = typeof(DatePickerInput);
                return;
            }

            if (_propertyType == typeof(TimeOnly) || _propertyType == typeof(TimeOnly?))
            {
                InputParameters.Add("Value", Value);
                if (_propertyType == typeof(TimeOnly))
                    InputParameters.Add("ValueChanged", EventCallback.Factory.Create<TimeOnly>(this, ValueChanged));
                else
                    InputParameters.Add("ValueChanged", EventCallback.Factory.Create<TimeOnly?>(this, ValueChanged));
                InputParameters.Add("Immediate", _autoForm.Immediate);
                InputParameters.Add("InputContainerStyle", _autoForm.InputContainerStyle);
                var timeFormatAttrib = PropertyInfo?.GetCustomAttribute<TimeFormatAttribute>();
                if (timeFormatAttrib != null)
                    InputParameters.Add("TimeFormat", timeFormatAttrib.TimeFormat);
                var orientationAttrib = PropertyInfo?.GetCustomAttribute<OrientationAttribute>();
                if (orientationAttrib != null)
                    InputParameters.Add("Orientation", orientationAttrib.Orientation);
                if (PropertyInfo?.GetCustomAttribute<Hours24Attribute>() != null)
                    InputParameters.Add("Hours24", true);
                var minuteStepAttrib = PropertyInfo?.GetCustomAttribute<MinuteStepAttribute>();
                if (minuteStepAttrib != null)
                    InputParameters.Add("MinuteStep", minuteStepAttrib.MinuteStep);

                InputType = typeof(TimePickerInput);
                return;
            }

            if (_propertyType == typeof(string))
            {
                //if (IsColorAttribute(PropertyInfo))
                //{
                //    CheckPlaceholderAttribute();
                //    InputParameters.Add("LabelShown", _autoFormViewModel.ShowLabels);
                //    AddToolTipParameter();
                //    InputType = typeof(InputColorEditor);
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
            InputParameters.Add("Label", _autoForm.HasLabelColumn ? null : DisplayName);
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
            InputParameters.Add("InputContainerStyle", _autoForm.InputContainerStyle);
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
                        Type t = propertyType;
                        if (propertyType.IsGenericType)
                            t = propertyType.GenericTypeArguments[0];
                        foreach (var item in Enum.GetValues(t))
                            if (item != null)
                            {
                                dynamic? o = Activator.CreateInstance(dataType);
                                if (o != null)
                                {
                                    o.Name = item.ToString();
                                    o.Value = (dynamic)Convert.ChangeType(item, t);
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
            InputParameters?.Add("Label", _autoForm.HasLabelColumn ? null : DisplayName);
            InputParameters?.Add("IsReadOnly", IsReadOnly);
            InputParameters?.Add("Size", _autoForm.Size);
            InputParameters?.Add("InputContainerStyle", _autoForm.InputContainerStyle);
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

        public static bool IsNumberType(Type? type)
        {
            if (type == null) 
                return false;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        return IsNumberType(Nullable.GetUnderlyingType(type)!);
                    return false;
            }
            return false;
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
