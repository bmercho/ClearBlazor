using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace ClearBlazor
{
    public partial class ListBoxItem<TListBox> : ClearComponentBase, IContent, IBackground, IColor
    {
        [Parameter]
        public TListBox? Value { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public Alignment ContentAlignment { get; set; } = Alignment.Stretch;

        [Parameter]
        public string? Text { get; set; } = null;

        [Parameter]
        public string? Icon { get; set; } = null;

        [Parameter]
        public string? Avatar { get; set; } = null;

        [Parameter]
        public Color? Color { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;


        [Parameter]
        public string? HRef { get; set; } = null;

        [Parameter]
        public bool InitiallyExpanded { get; set; } = true;

        [Inject]
        private NavigationManager NavManager { get; set; } = null!;

        internal Size RowSize { get; set; } = Size.Normal;
        internal int Level { get; set; } = 1;
        internal bool HasChildren { get; set; } = false;
        internal Color? ColorOverride { get; set; } = null;

        private string FontSize = "";
        private string FontFamily = "";
        private int FontWeight = 0;
        private FontStyle FontStyle = FontStyle.Normal;
        private string RowStyle = string.Empty;
        private double RowHeight = 0;
        private string LinePadding = string.Empty;
        private string RowClasses = string.Empty;
        private bool IsExpanded = true;
        private bool MouseOver = false;
        private bool FirstTime = true;
        private bool IsVisible = true;
        private bool IsSelected = false;
        private bool Multiselect = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (string.IsNullOrEmpty(Text))
                if (Value != null)
                    Text = Value.ToString();

            var parent = FindParent<ListBox<TListBox>>(Parent);
            if (parent == null)
                return;

            Multiselect = parent.MultiSelect;
            await parent.HandleChild(this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (HorizontalAlignment == null)
                HorizontalAlignment = Alignment.Stretch;
        }

        protected override void ComputeOwnClasses(StringBuilder sb)
        {
            base.ComputeOwnClasses(sb);
            if (Parent == null)
                return;

            var list = FindParent<ListBox<TListBox>>(Parent);
            if (list != null && list.Clickable)
            {
                RowClasses = "clear-ripple";
            }
            if (list == null)
                RowClasses = "";
        }

        protected override string UpdateStyle(string css)
        {
            FontSize = GetFontSize();
            FontFamily = GetFontFamily();
            FontWeight = GetFontWeight();
            FontStyle = GetFontStyle();

            css += "display: grid; ";

            RowStyle = IsSelected && HasChildren ? $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColor.Value}; " :
                      IsSelected ? $"background-color: {ThemeManager.CurrentPalette.ListSelectedColor.Value}; " :
                      MouseOver ? $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColor.Value}; " :
                               string.Empty;
            RowStyle += $"height:{GetRowHeight()}; ";
            return css;
        }

        private string GetRowHeight()
        {
            ListBoxItem<TListBox>? parent = null;
            if (Parent != null)
                parent = Parent.Parent as ListBoxItem<TListBox>;

            if (HasChildren)
            {
                if (parent != null && !parent.IsVisible)
                    return "0";
            }
            else
            {
                if (!IsVisible)
                    return "0";
            }

            switch (RowSize)
            {
                case Size.VerySmall:
                    return "27px";
                case Size.Small:
                    return "33px";
                case Size.Normal:
                    return "41px";
                case Size.Large:
                    return "51px";
                case Size.VeryLarge:
                    return "61px";
            }
            return "41px";
        }
        protected override void UpdateChildParams(ClearComponentBase child, int level)
        {
            child.HorizontalAlignmentDefaultOverride = Alignment.Start;

            if (level == 2 && child is ListBoxItem<TListBox>)
            {
                var listBoxItem = (ListBoxItem<TListBox>)child;
                if (listBoxItem != null)
                {
                    listBoxItem.RowSize = RowSize;
                    listBoxItem.Color = Color;
                    listBoxItem.Level = Level + 1;
                    HasChildren = true;
                    if (FirstTime)
                    {
                        IsExpanded = InitiallyExpanded;
                        FirstTime = false;
                    }
                    SetVisibility(this, listBoxItem);
                }
            }

            var topBottomPadding = 0;
            if (!HasChildren && IsExpanded)
                topBottomPadding = 0;

            string padding = $"{topBottomPadding},5,{topBottomPadding},{20 * (Level - 1) + 10}";
            if (LinePadding != padding)
            {
                LinePadding = padding;
                StateHasChanged();
            }
        }


        protected async Task OnMouseEnter()
        {
            MouseOver = true;
            await Task.CompletedTask;
            StateHasChanged();
        }

        protected async Task OnMouseLeave()
        {
            MouseOver = false;
            await Task.CompletedTask;
            StateHasChanged();
        }
        public async Task OnListItemClicked(MouseEventArgs e)
        {
            if (HRef != null)
                NavManager.NavigateTo(HRef);

            if (HasChildren)
            {
                IsExpanded = !IsExpanded;
                IsVisible = IsExpanded;
            }

            ListBox<TListBox>? parent = FindParent<ListBox<TListBox>>(Parent);
            if (parent != null)
                IsSelected = await parent.SetSelected(this);
            await OnClicked.InvokeAsync();
            StateHasChanged();
        }

        public void Unselect()
        {
            IsSelected = false;
            StateHasChanged();
        }
        public void Select()
        {
            IsSelected = true;
            StateHasChanged();
        }

        private void SetVisibilityOfChildren(ClearComponentBase parent, bool visibility)
        {
            foreach (var c in parent.Children)
            {
                if (c is ListBoxItem<TListBox>)
                {
                    var child = (ListBoxItem<TListBox>)c;
                    child.IsVisible = visibility;
                }
                if (c.Children.Count > 0)
                    SetVisibilityOfChildren(c, visibility);
            }
        }

        private void SetVisibility(ClearComponentBase? parent, ListBoxItem<TListBox> child)
        {
            while (parent != null)
            {
                var parentItem = parent as ListBoxItem<TListBox>;
                var parentList = parent as ListBox<TListBox>;
                if (parentList != null)
                {
                    IsVisible = IsExpanded;
                    return;
                }
                else if (parentItem != null)
                {
                    if (parentItem.IsVisible && parentItem.IsExpanded)
                        child.IsVisible = true;
                    else
                        child.IsVisible = false;
                    return;
                }
                parent = parent.Parent;
            }
        }

        private FontStyle GetFontStyle()
        {
            switch (RowSize)
            {
                case Size.VerySmall:
                    return ThemeManager.CurrentTheme.Typography.ListItemVerySmall.FontStyle;
                case Size.Small:
                    return ThemeManager.CurrentTheme.Typography.ListItemSmall.FontStyle;
                case Size.Normal:
                    return ThemeManager.CurrentTheme.Typography.ListItemNormal.FontStyle;
                case Size.Large:
                    return ThemeManager.CurrentTheme.Typography.ListItemLarge.FontStyle;
                case Size.VeryLarge:
                    return ThemeManager.CurrentTheme.Typography.ListItemVeryLarge.FontStyle;
                default:
                    return ThemeManager.CurrentTheme.Typography.ListItemNormal.FontStyle;
            }
        }

        private int GetFontWeight()
        {
            switch (RowSize)
            {
                case Size.VerySmall:
                    return ThemeManager.CurrentTheme.Typography.ListItemVerySmall.FontWeight;
                case Size.Small:
                    return ThemeManager.CurrentTheme.Typography.ListItemSmall.FontWeight;
                case Size.Normal:
                    return ThemeManager.CurrentTheme.Typography.ListItemNormal.FontWeight;
                case Size.Large:
                    return ThemeManager.CurrentTheme.Typography.ListItemLarge.FontWeight;
                case Size.VeryLarge:
                    return ThemeManager.CurrentTheme.Typography.ListItemVeryLarge.FontWeight;
                default:
                    return ThemeManager.CurrentTheme.Typography.ListItemNormal.FontWeight;
            }
        }

        private string GetFontFamily()
        {
            switch (RowSize)
            {
                case Size.VerySmall:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.ListItemVerySmall.FontFamily);
                case Size.Small:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.ListItemSmall.FontFamily);
                case Size.Normal:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.ListItemNormal.FontFamily);
                case Size.Large:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.ListItemLarge.FontFamily);
                case Size.VeryLarge:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.ListItemVeryLarge.FontFamily);
                default:
                    return string.Join(",", ThemeManager.CurrentTheme.Typography.ListItemNormal.FontFamily);
            }
        }

        private string GetFontSize()
        {
            switch (RowSize)
            {
                case Size.VerySmall:
                    return ThemeManager.CurrentTheme.Typography.ListItemVerySmall.FontSize;
                case Size.Small:
                    return ThemeManager.CurrentTheme.Typography.ListItemSmall.FontSize;
                case Size.Normal:
                    return ThemeManager.CurrentTheme.Typography.ListItemNormal.FontSize;
                case Size.Large:
                    return ThemeManager.CurrentTheme.Typography.ListItemLarge.FontSize;
                case Size.VeryLarge:
                    return ThemeManager.CurrentTheme.Typography.ListItemVeryLarge.FontSize;
                default:
                    return ThemeManager.CurrentTheme.Typography.ListItemNormal.FontSize;
            }
        }
        private Color? GetColor()
        {
            //if (Color != null)
            //    return Color;

            //if (ColorOverride != null)
            //    return ColorOverride;

            return Color.Dark;
        }
    }
}