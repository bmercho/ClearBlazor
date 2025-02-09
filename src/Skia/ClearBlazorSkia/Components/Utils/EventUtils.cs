namespace ClearBlazor
{
    public static class EventUtils
    {
        public enum DomEvent
        {
            MouseOver, MouseOut, MouseMove, MouseDown,
            MouseUp, Click, DblClick, ContextMenu, Wheel, MouseWheel,

            KeyDown, KeyUp, KeyPress,

            TouchCancel, TouchEnd, TouchMove, TouchStart, TouchEnter, TouchLeave,

            PointerCancel, PointerDown, PointerEnter, PointerLeave, PointerMove,
            PointerOut, PointerOver,  PointerUp, GotPointerCapture, LostPointerCapture
        }

        public enum MouseEvent
        {
            MouseOver = DomEvent.MouseOver,
            MouseOut = DomEvent.MouseOut,
            MouseMove = DomEvent.MouseMove,
            MouseDown = DomEvent.MouseDown,
            MouseUp = DomEvent.MouseUp,
            Click = DomEvent.Click,
            DblClick = DomEvent.DblClick,
            ContextMenu = DomEvent.ContextMenu,
            Wheel = DomEvent.Wheel,
            MouseWheel = DomEvent.MouseWheel
        }
        public enum KeyboardEvent
        {
            KeyDown = DomEvent.KeyDown,
            KeyUp = DomEvent.KeyUp,
            KeyPress = DomEvent.KeyPress
        }

        public enum TouchEvent
        {
            TouchCancel = DomEvent.TouchCancel,
            TouchEnd = DomEvent.TouchEnd,
            TouchMove = DomEvent.TouchMove,
            TouchStart = DomEvent.TouchStart,
            TouchEnter = DomEvent.TouchEnter,
            TouchLeave = DomEvent.TouchLeave
        }

        public enum PointerEvent
        {
            PointerCancel = DomEvent.PointerCancel,
            PointerDown = DomEvent.PointerDown,
            PointerEnter = DomEvent.PointerEnter,
            PointerLeave = DomEvent.PointerLeave,
            PointerMove = DomEvent.PointerMove,
            PointerOut = DomEvent.PointerOut,
            PointerOver = DomEvent.PointerOver,
            PointerUp = DomEvent.PointerUp,
            GotPointerCapture = DomEvent.GotPointerCapture,
            LostPointerCapture = DomEvent.LostPointerCapture
        }

        public static bool IsMouseEvent(DomEvent domEvent)
        {
            switch (domEvent)
            {
                case DomEvent.MouseOver:
                case DomEvent.MouseOut:
                case DomEvent.MouseMove:
                case DomEvent.MouseDown:
                case DomEvent.MouseUp:
                case DomEvent.Click:
                case DomEvent.DblClick:
                case DomEvent.ContextMenu:
                case DomEvent.Wheel:
                case DomEvent.MouseWheel:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsKeyboardEvent(DomEvent domEvent)
        {
            switch (domEvent)
            {
                case DomEvent.KeyDown:
                case DomEvent.KeyUp:
                case DomEvent.KeyPress:
                    return true;
                default:
                    return false;
            }
        }
        public static bool IsTouchEvent(DomEvent domEvent)
        {
            switch (domEvent)
            {
                case DomEvent.TouchCancel:
                case DomEvent.TouchEnd:
                case DomEvent.TouchMove:
                case DomEvent.TouchStart:
                case DomEvent.TouchEnter:
                case DomEvent.TouchLeave:
                    return true;
                default:
                    return false;
            }
        }
        public static bool IsPointerEvent(DomEvent domEvent)
        {
            switch (domEvent)
            {
                case DomEvent.PointerCancel:
                case DomEvent.PointerDown:
                case DomEvent.PointerEnter:
                case DomEvent.PointerLeave:
                case DomEvent.PointerMove:
                case DomEvent.PointerOut:
                case DomEvent.PointerOver:
                case DomEvent.PointerUp:
                case DomEvent.GotPointerCapture:
                case DomEvent.LostPointerCapture:
                    return true;
                default:
                    return false;
            }
        }
    }
}
