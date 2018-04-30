using System;

namespace MvvmCross.Plugin.Overlay.Platforms.Android.UI
{
    public abstract class MvxEventSourceOverlay : IMvxEventSourceOverlay, IDisposable
    {
        protected MvxEventSourceOverlay()
        {
        }

        public event EventHandler ViewCreated;

		public event EventHandler ViewWillAttachToWindow;

		public event EventHandler ViewAttachedToWindow;

        public event EventHandler ViewWillDetachFromWindow;

        public event EventHandler ViewDetachedFromWindow;

        public event EventHandler ViewDisposed;

        void IMvxEventSourceOverlay.RaiseViewCreated()
        {
			OnViewCreated();
            var viewCreated = ViewCreated;
            viewCreated?.Invoke(this, EventArgs.Empty);
        }

        void IMvxEventSourceOverlay.RaiseViewWillAttachToWindow()
        {
            OnViewWillAttachToWindow();
            var viewWillAttachToWindow = ViewWillAttachToWindow;
            viewWillAttachToWindow?.Invoke(this, EventArgs.Empty);
        }

        void IMvxEventSourceOverlay.RaiseViewAttachedToWindow()
        {
            OnViewAttachedToWindow();
            var viewAttachedToWindow = ViewAttachedToWindow;
            viewAttachedToWindow?.Invoke(this, EventArgs.Empty);
        }

        void IMvxEventSourceOverlay.RaiseViewWillDetachFromWindow()
        {
            OnViewWillDetachFromWindow();
            var viewWillDetachFromWindow = ViewWillDetachFromWindow;
            viewWillDetachFromWindow?.Invoke(this, EventArgs.Empty);
        }

        void IMvxEventSourceOverlay.RaiseViewDetachedFromWindow()
        {
            OnViewDetachedFromWindow();
            var viewDetachedFromWindow = ViewDetachedFromWindow;
            viewDetachedFromWindow?.Invoke(this, EventArgs.Empty);
        }


        protected virtual void OnViewCreated()
        {
        }

        protected virtual void OnViewWillAttachToWindow()
        {
        }

        protected virtual void OnViewAttachedToWindow()
        {
        }

        protected virtual void OnViewWillDetachFromWindow()
        {
        }

        protected virtual void OnViewDetachedFromWindow()
        {
        }


        #region IDisposable implementation
        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);

            var viewDisposed = ViewDisposed;
            viewDisposed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
