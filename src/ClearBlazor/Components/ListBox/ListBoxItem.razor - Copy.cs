using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace ClearBlazor
{
    /// <summary>
    /// An item that can be a child of ListBox.
    /// Can be text, icon or avatar
    /// </summary>
    /// <typeparam name="TListBox"></typeparam>
    public partial class ListBoxItem<TListBox> : ClearComponentBase, IBackground, IColor
    {
        /// <summary>
        /// The value of the list box item.
        /// </summary>
        [Parameter]
        public TListBox? Value { get; set; }

        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// The horizontal alignment of the item
        /// </summary>
        [Parameter]
        public Alignment ContentAlignment { get; set; } = Alignment.Stretch;

        /// <summary>
        /// The text to be shown
        /// </summary>
        [Parameter]
        public string? Text { get; set; } = null;

        /// <summary>
        /// The icon to be shown
        /// </summary>
        [Parameter]
        public string? Icon { get; set; } = null;

        /// <summary>
        /// The avatar to be shown
        /// </summary>
        [Parameter]
        public string? Avatar { get; set; } = null;

        /// <summary>
        /// The color of the item
        /// </summary>
        [Parameter]
        public Color? Color { get; set; } = null;

        /// <summary>
        /// The background color of the item
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// The href to navigate to when clicked
        /// </summary>
        [Parameter]
        public string? HRef { get; set; } = null;

        /// <summary>
        /// Indicates if the item is initially expanded
        /// </summary>
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
        private string LinePadding = string.Empty;
        private string RowClasses = string.Empty;
        private bool IsExpanded = false;
        private bool MouseOver = false;
        private bool FirstTime = true;
        private bool IsVisible = false;
        private bool IsSelected = false;
        private bool Multiselect = false;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (string.IsNullOrEmpty(Text))
                if (Value != null)
                    Text = Value.ToString();

            var p = Parent as ListBox<TListBox>;

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

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
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

            RowStyle = string.Empty;
            if (IsSelected)
                if (MouseOver)
                    RowStyle = $"background-color: {ThemeManager.CurrentColorScheme.SecondaryContainer.SetAlpha(.8).Value}; ";
                else
                    RowStyle = $"background-color: {ThemeManager.CurrentColorScheme.SecondaryContainer.Value}; ";
            else
            {
                if (MouseOver)
                    RowStyle = $"background-color: {ThemeManager.CurrentColorScheme.SurfaceContainerHighest.SetAlpha(.8).Value}; ";
                else
                    RowStyle = string.Empty;
            }
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

            if (child is ListBoxItem<TListBox>)
            {
                var listBoxItem = (ListBoxItem<TListBox>)child;

                var parent = FindParent<ListBoxItem<TListBox>>(child.Parent);

                if (listBoxItem != null)
                {
                    listBoxItem.RowSize = RowSize;
                    listBoxItem.Color = Color;
                    listBoxItem.Level = parent == null ? 1 : parent.Level + 1;
                    HasChildren = true;
                    if (FirstTime)
                    {
                        IsExpanded = InitiallyExpanded;
                        FirstTime = false;
                    }
                    if (listBoxItem.Text == "ScrollViewer")
                    {

                    }

                    SetVisibility(this, listBoxItem);
                    Console.WriteLine($"{listBoxItem.Text}  Level={listBoxItem.Level} " +
                                      $"HasChildren={listBoxItem.HasChildren}" +
                                      $" IsExpanded={listBoxItem.IsExpanded}" +
                                      $" IsVisible={listBoxItem.IsVisible}");
                }
            }

            Console.WriteLine($"{Text}  Level={Level} " +
                  $"HasChildren={HasChildren}" +
                  $" IsExpanded={IsExpanded}" +
                  $" IsVisible={IsVisible}");

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
        private async Task OnListItemClicked(MouseEventArgs e)
        {
            if (HasChildren)
            {
                IsExpanded = !IsExpanded;
                IsVisible = IsExpanded;
                SetVisibilityOfChildren(this, IsVisible);
            }
            else
            {

                ListBox<TListBox>? parent = FindParent<ListBox<TListBox>>(Parent);
                if (parent != null)
                    IsSelected = await parent.SetSelected(this);
            }
            StateHasChanged();
            await OnClicked.InvokeAsync();
            if (HRef != null)
                NavManager.NavigateTo(HRef);
        }

        internal void Unselect()
        {
            IsSelected = false;
            StateHasChanged();
        }
        internal void Select()
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
            if (Color != null)
                return Color;

            if (ColorOverride != null)
                return ColorOverride;

            return ThemeManager.CurrentColorScheme.OnSurface;
        }
    }
}