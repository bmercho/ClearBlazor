using ClearBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Text;

namespace ClearBlazor
{
    public partial class Drawer : ClearComponentBase,IBackground,IDisposable, IObserver<BrowserSizeInfo>
    {
        [Parameter]
        public DrawerLocation DrawerLocation { get; set; } = DrawerLocation.Left;

        [Parameter]
        public DrawerMode DrawerMode { get; set; } = DrawerMode.Responsive;
        
        [Parameter]
        public RenderFragment? DrawerContent { get; set; } = null;

        [Parameter]
        public RenderFragment? DrawerBody { get; set; } = null;

        [Parameter]
        public bool Open { get; set; } = false;

        [Parameter]
        public EventCallback<bool> OpenChanged { get; set; }

        [Parameter]
        public bool OverlayEnabled { get; set; } = true;

        [Parameter]
        public Color? Color { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        protected string Columns { get; set; } = "*";

        protected string Rows { get; set; } = "*";

        protected int DrawerContentColumn { get; set; } = 0;

        protected int DrawerBodyColumn { get; set; } = 1;
        protected int DrawerBodyColumnSpan { get; set; } = 1;

        protected int DrawerContentRow { get; set; } = 0;

        protected int DrawerBodyRow { get; set; } = 0;


        private Grid? Content = null;

        private ElementSize? ElementSize = null;

        private string DrawerMargin = "0";

        private bool gotSize = false;

        private DrawerMode _drawerMode = DrawerMode.Responsive;

        private bool ShowOverlay { get; set; } = false;

        private IDisposable unsubscriber;

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
            if (!gotSize)
            {
                var size = await JSRuntime.InvokeAsync<ElementSize>("window.clearBlazor.drawer.getElementSize", Content?.Id);
                if (size.ElementWidth != 0 && size.ElementHeight != 0)
                {
                    if (ElementSize == null || size.ElementWidth != ElementSize.ElementWidth || size.ElementHeight != ElementSize.ElementHeight)
                    {
                        ElementSize = size;
                        gotSize = true;
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
                        Columns = "auto,*";
                        DrawerBodyColumn = 1;
                        DrawerBodyColumnSpan = 1;
                    }
                    else
                    {
                        Columns = "auto,*";
                        DrawerBodyColumn = 0;
                        DrawerBodyColumnSpan = 2;
                    }
                    Rows = "*";
                    DrawerContentColumn = 0;
                    DrawerContentRow = 0;
                    DrawerBodyRow = 0;
                    break;
                case DrawerLocation.Right:
                    if (_drawerMode == DrawerMode.Permanent)
                    {
                        Columns = "*,auto";
                        DrawerContentColumn = 1;
                    }
                    else
                    {
                        Columns = "*";
                        DrawerContentColumn = 0;
                    }
                    Columns = "*,auto";
                    Rows = "*";
                    DrawerBodyColumn = 0;
                    DrawerContentRow = 0;
                    DrawerBodyRow = 0;
                    break;
                case DrawerLocation.Top:
                    if (_drawerMode == DrawerMode.Permanent)
                    {
                        Rows = "auto,*";
                        DrawerBodyRow = 1;
                    }
                    else
                    {
                        Rows = "*";
                        DrawerBodyRow = 0;
                    }
                    Rows = "auto,*";
                    Columns = "*";
                    DrawerContentColumn = 0;
                    DrawerBodyColumn = 0;
                    DrawerContentRow = 0;
                    break;
                case DrawerLocation.Bottom:
                    if (_drawerMode == DrawerMode.Permanent)
                    {
                        Rows = "*,auto";
                        DrawerContentRow = 1;
                    }
                    else
                    {
                        Rows = "*";
                        DrawerContentRow = 0;
                    }
                    Rows = "*,auto";
                    Columns = "*";
                    DrawerContentColumn = 0;
                    DrawerBodyColumn = 0;
                    DrawerBodyRow = 0;
                    break;
            }
            if (gotSize)
            {
                ShowOverlay = false;
                if (ElementSize != null && Content != null)
                    if (Open)
                    {
                        if (OverlayEnabled && _drawerMode == DrawerMode.Temporary)
                            ShowOverlay = true;
                        DrawerMargin = "0";
                    }
                    else
                        SetMargin();
            }
            else
                DrawerMargin = "0";
        }

        private void SetMargin()
        {
            switch (DrawerLocation)
            {
                case DrawerLocation.Left:
                    // The -5 used below is to fix an issue where the calculated element width (in OnAfterRenderAsync)
                    // is a couple of pixels less than what the actual width comes out as. Not sure why.
                    DrawerMargin = $"0,0,0,{-ElementSize.ElementWidth - 5}";
                    break;
                case DrawerLocation.Right:
                    DrawerMargin = $"0,{-ElementSize.ElementWidth-5},0,0";
                    break;
                case DrawerLocation.Top:
                    DrawerMargin = $"{-ElementSize.ElementHeight-5},0,0,0";
                    break;
                case DrawerLocation.Bottom:
                    DrawerMargin = $"0,0,{-ElementSize.ElementHeight-5},0";
                    break;
            }
        }

        public virtual void Subscribe(IObservable<BrowserSizeInfo> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
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
                if (browserSizeInfo.DeviceSize < DeviceSize.Medium)
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

    public class ElementSize
    {
        public double ElementX { get; set; }
        public double ElementY { get; set; }
        public double ElementWidth { get; set; }
        public double ElementHeight { get; set; }
    }

}