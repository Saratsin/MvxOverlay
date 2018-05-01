using System;
using System.Linq;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Plugin.Overlay.Platforms.Android
{
    internal class MvxOverlayAdapter : IDisposable
    {
        private readonly MvxOverlay _view;

        public MvxOverlayAdapter(MvxOverlay view)
        {
            _view = view;

            _view.ViewCreated += OnViewCreated;
            _view.ViewWillAttachToWindow += OnViewWillAttachToWindow;
            _view.ViewAttachedToWindow += OnViewAttachedToWindow;
            _view.ViewWillDetachFromWindow += OnViewWillDetachFromWindow;
            _view.ViewDetachedFromWindow += OnViewDetachedFromWindow;
            _view.ViewDisposed += OnViewDisposed;
        }

        private static IMvxViewModel LoadViewModel(IMvxView androidView)
        {
            var viewModelType = androidView.GetType()
                                           .GetProperties()
                                           .Where(p => p.Name == nameof(androidView.ViewModel))
                                           .First(vmp => vmp.PropertyType != typeof(IMvxViewModel))
                                           .PropertyType;

            var request = new MvxViewModelRequest(viewModelType);
            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(request, null);

            if (viewModel == null)
                Mvx.Resolve<IMvxLog>().Warn("ViewModel not loaded for {0}", request.ViewModelType.FullName);

            return viewModel;
        }

        private void OnViewCreated(object sender, EventArgs e)
        {
            if(_view.ViewModel == null)
                _view.ViewModel = LoadViewModel(_view);
            
            _view.ViewModel.ViewCreated();
        }

        private void OnViewWillAttachToWindow(object sender, EventArgs e)
        {
            _view.ViewModel.ViewAppearing();
        }

        private void OnViewAttachedToWindow(object sender, EventArgs e)
        {
            _view.ViewModel.ViewAppeared();
        }

        private void OnViewWillDetachFromWindow(object sender, EventArgs e)
        {
            _view.ViewModel.ViewDisappearing();
        }

        private void OnViewDetachedFromWindow(object sender, EventArgs e)
        {
            _view.ViewModel.ViewDisappeared();
        }

        private void OnViewDisposed(object sender, EventArgs eventArgs)
        {
            _view.ClearAllBindings();

			_view.ViewModel.ViewDestroy(true);
            _view.ViewModel.DisposeIfDisposable();
        }

        #region IDisposable Support
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _view.ViewCreated -= OnViewCreated;
            _view.ViewWillAttachToWindow -= OnViewWillAttachToWindow;
            _view.ViewAttachedToWindow -= OnViewAttachedToWindow;
            _view.ViewWillDetachFromWindow -= OnViewWillDetachFromWindow;
            _view.ViewDetachedFromWindow -= OnViewDetachedFromWindow;
            _view.ViewDisposed -= OnViewDisposed;
            
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}