using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// A panel docked to a side of the page that slides in and out to shown or hidden.
    /// </summary>
    public partial class Drawer : ClearComponentBase,IBackground,IDisposable, IObserver<BrowserSizeInfo>
    {
        /// <summary>
        /// The side that the drawer will reside.
        /// </summary>
        [Parameter]
        public DrawerLocation DrawerLocation { get; set; } = DrawerLocation.Left;

        /// <summary>
        /// The drawer mode.
        /// </summary>
        [Parameter]
        public DrawerMode DrawerMode { get; set; } = DrawerMode.Responsive;
        
        /// <summary>
        /// The content of the drawer
        /// </summary>
        [Parameter]
        public RenderFragment? DrawerContent { get; set; } = null;

        /// <summary>
        /// The content of the drawer body
        /// </summary>
        [Parameter]
        public RenderFragment? DrawerBody { get; set; } = null;

        /// <summary>
        /// Indicates if the drawer is open
        /// </summary>
        [Parameter]
        public bool Open { get; set; } = false;

        /// <summary>
        /// Indicates if an overlay will be shown over the container of the drawer
        /// </summary>
        [Parameter]
        public bool OverlayEnabled { get; set; } = true;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// Event that is raised when the drawer is opened or closed.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OpenChanged { get; set; }

        private string _columns = "*";
        private string _rows = "*";
        private int _drawerContentColumn = 0;
        private int _drawerBodyColumn = 1;
        private int _drawerBodyColumnSpan = 1;
        private int _drawerContentRow = 0;
        private int _drawerBodyRow = 0;
        private Grid? _content = null;
        private ElementSizeInfo? _elementSize = null;
        private string _drawerMargin = "0";
        private bool _gotSize = false;
        private DrawerMode _drawerMode = DrawerMode.Responsive;
        private bool _showOverlay = false;
        private IDisposable _unsubscriber = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Subscribe(BrowserSizeService.Instance);
        }

        public DrawerMode CurrentDrawerMode => _drawerMode;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (DrawerMode != DrawerMode.Responsive)
                _drawerMode = DrawerMode;
            ProcessModeChange();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_gotSize)
            {
                var size = await JSRuntime.InvokeAsync<ElementSizeInfo>("window.GetElementSizeInfoById", _content?.Id);
                if (size.ElementWidth != 0 && size.ElementHeight != 0)
                {
                    if (_elementSize == null || size.ElementWidth != _elementSize.ElementWidth || size.ElementHeight != _elementSize.ElementHeight)
                    {
                        _elementSize = size;
                        _gotSize = true;
                        ProcessModeChange();
                        StateHasChanged();
                    }
                }
            }
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display: grid; ";

            return css;
        }

        private async Task OverlayClicked()
        {
            if (_drawerMode == DrawerMode.Temporary && Open)
            {
                Open = false;
                await OpenChanged.InvokeAsync(false);
                ProcessModeChange();
                StateHasChanged();
            }
        }

        private void ProcessModeChange()
        {
            switch (DrawerLocation)
            {
                case DrawerLocation.Left:
                    if (_drawerMode == DrawerMode.Permanent)
                    {
                        _columns = "auto,*";
                        _drawerBodyColumn = 1;
                        _drawerBodyColumnSpan = 1;
                    }
                    else
                    {
                        _columns = "auto,*";
                        _drawerBodyColumn = 0;
                        _drawerBodyColumnSpan = 2;
                    }
                    _rows = "*";
                    _drawerContentColumn = 0;
                    _drawerContentRow = 0;
                    _drawerBodyRow = 0;
                    break;
                case DrawerLocation.Right:
                    if (_drawerMode == DrawerMode.Permanent)
                    {
                        _columns = "*,auto";
                        _drawerContentColumn = 1;
                    }
                    else
                    {
                        _columns = "*";
                        _drawerContentColumn = 0;
                    }
                    _columns = "*,auto";
                    _rows = "*";
                    _drawerBodyColumn = 0;
                    _drawerContentRow = 0;
                    _drawerBodyRow = 0;
                    break;
                case DrawerLocation.Top:
                    if (_drawerMode == DrawerMode.Permanent)
                    {
                        _rows = "auto,*";
                        _drawerBodyRow = 1;
                    }
                    else
                    {
                        _rows = "*";
                        _drawerBodyRow = 0;
                    }
                    _rows = "auto,*";
                    _columns = "*";
                    _drawerContentColumn = 0;
                    _drawerBodyColumn = 0;
                    _drawerContentRow = 0;
                    break;
                case DrawerLocation.Bottom:
                    if (_drawerMode == DrawerMode.Permanent)
                    {
                        _rows = "*,auto";
                        _drawerContentRow = 1;
                    }
                    else
                    {
                        _rows = "*";
                        _drawerContentRow = 0;
                    }
                    _rows = "*,auto";
                    _columns = "*";
                    _drawerContentColumn = 0;
                    _drawerBodyColumn = 0;
                    _drawerBodyRow = 0;
                    break;
            }
            if (_gotSize)
            {
                _showOverlay = false;
                if (_elementSize != null && _content != null)
                    if (Open)
                    {
                        if (OverlayEnabled && _drawerMode == DrawerMode.Temporary)
                            _showOverlay = true;
                        _drawerMargin = "0";
                    }
                    else
                        SetMargin();
            }
            else
                _drawerMargin = "0";
        }

        private void SetMargin()
        {
            if (_elementSize == null)
                return;

            switch (DrawerLocation)
            {
                case DrawerLocation.Left:
                    // The -5 used below is to fix an issue where the calculated element width (in OnAfterRenderAsync)
                    // is a couple of pixels less than what the actual width comes out as. Not sure why.
                    _drawerMargin = $"0,0,0,{-_elementSize.ElementWidth - 5}";
                    break;
                case DrawerLocation.Right:
                    _drawerMargin = $"0,{-_elementSize.ElementWidth-5},0,0";
                    break;
                case DrawerLocation.Top:
                    _drawerMargin = $"{-_elementSize.ElementHeight-5},0,0,0";
                    break;
                case DrawerLocation.Bottom:
                    _drawerMargin = $"0,0,{-_elementSize.ElementHeight-5},0";
                    break;
            }
        }

        protected virtual void Subscribe(IObservable<BrowserSizeInfo> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }

        protected virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }

        public virtual void OnCompleted()
        {
        }

        public virtual void OnError(Exception error)
        {
        }

        public virtual void OnNext(BrowserSizeInfo browserSizeInfo)
        {
            if (DrawerMode == DrawerMode.Responsive)
            {
                var currentDrawerMode = _drawerMode;
                if (browserSizeInfo.DeviceSize < DeviceSize.Expanded)
                {
                    _drawerMode = DrawerMode.Temporary;
                    Open = false;
                    OpenChanged.InvokeAsync(Open);
                }
                else
                    _drawerMode = DrawerMode.Permanent;
                if (currentDrawerMode != _drawerMode)
                {
                    if (_drawerMode == DrawerMode.Permanent)
                    {
                        Open = true;
                        OpenChanged.InvokeAsync(Open);
                    }
                }
                ProcessModeChange();
                StateHasChanged();
            }

        }

        //private void CheckResponsiveMode()
        //{
        //    if (DrawerMode == DrawerMode.Responsive)
        //    {
        //        var currentDrawerMode = _drawerMode;
        //        if (browserSizeInfo.DeviceSize < DeviceSize.Medium)
        //        {
        //            _drawerMode = DrawerMode.Temporary;
        //            Open = false;
        //            OpenChanged.InvokeAsync(Open);
        //        }
        //        else
        //            _drawerMode = DrawerMode.Permanent;
        //        if (currentDrawerMode != _drawerMode)
        //        {
        //            if (_drawerMode == DrawerMode.Permanent)
        //            {
        //                Open = true;
        //                OpenChanged.InvokeAsync(Open);
        //            }
        //        }
        //        ProcessModeChange();
        //        StateHasChanged();
        //    }
        //}

        public override void Dispose()
        {
            Unsubscribe();
        }

    }
}