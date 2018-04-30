using System;

namespace MvvmCross.Plugin.Overlay.Platforms.Android.EventSource
{	
    public interface IMvxEventSourceOverlay
    {
        event EventHandler ViewCreated;
        event EventHandler ViewWillAttachToWindow;
        event EventHandler ViewAttachedToWindow;
        event EventHandler ViewWillDetachFromWindow;
        event EventHandler ViewDetachedFromWindow;
        event EventHandler ViewDisposed;

        void RaiseViewCreated();
        void RaiseViewWillAttachToWindow();
        void RaiseViewAttachedToWindow();
        void RaiseViewWillDetachFromWindow();
        void RaiseViewDetachedFromWindow();
    }
}
