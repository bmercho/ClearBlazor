using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A pagination control component.
    /// </summary>
    public partial class Pagination:ClearComponentBase
    {
        /// <summary>
        /// The size of the component.
        /// </summary>
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        /// <summary>
        /// The color of the component.
        /// </summary>
        [Parameter]
        public Color? Color { get; set; } = null;

        /// <summary>
        /// The number of pages.
        /// </summary>
        [Parameter]
        public int NumPages { get; set; } = 0;

        /// <summary>
        /// The number of pages shown.
        /// </summary>
        [Parameter]
        public int NumPagesShown { get; set; } = 0;

        /// <summary>
        /// Whether to show the first and last buttons.
        /// </summary>
        [Parameter]
        public bool ShowFirstAndLastButtons { get; set; } = false;

        /// <summary>
        /// The selected page.
        /// </summary>
        [Parameter]
        public int SelectedPage { get; set; } = 1;

        /// <summary>
        /// The event that is raised when the selected page changes.
        /// </summary>
        [Parameter]
        public EventCallback<int> SelectedPageChanged { get; set; }

        private List<int> _shownPages = new List<int>();
        private bool _atStart = true;
        private bool _atEnd = false;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            GetShownPages();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        private void GetShownPages()
        {
            _shownPages.Clear();
            var startPage = (int)Math.Floor((SelectedPage-1) / (double)NumPagesShown) * NumPagesShown + 1;
            for (int i = startPage; i < startPage + NumPagesShown; i++)
            {
                if (i <= NumPages)
                    _shownPages.Add(i);
            }
            if (SelectedPage == 1)
                _atStart = true;
            else
                _atStart = false;
            if (SelectedPage == NumPages)
                _atEnd = true;
            else
                _atEnd = false;
        }

        private async Task GotoStart()
        {
            SelectedPage = 1;
            GetShownPages();
            StateHasChanged();
            await SelectedPageChanged.InvokeAsync(SelectedPage);
        }

        private async Task GotoEnd()
        {
            SelectedPage = NumPages;
            GetShownPages();
            StateHasChanged();
            await SelectedPageChanged.InvokeAsync(SelectedPage);
        }

        private async Task GotoNext()
        {
            SelectedPage++;
            GetShownPages();
            StateHasChanged();
            await SelectedPageChanged.InvokeAsync(SelectedPage);
        }

        private async Task GotoPrev()
        {
            SelectedPage--;
            GetShownPages();
            StateHasChanged();
            await SelectedPageChanged.InvokeAsync(SelectedPage);
        }

        private async Task SelectPage(int page)
        {
            SelectedPage = page;
            GetShownPages();
            StateHasChanged();
            await SelectedPageChanged.InvokeAsync(SelectedPage);
        }
    }
}