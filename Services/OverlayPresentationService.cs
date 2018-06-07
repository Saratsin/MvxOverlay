using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Plugin.Overlay.Platforms.Android.EventSource;

namespace MvvmCross.Plugin.Overlay.Platforms.Android.Services
{
    internal class OverlayPresentationService
    {
		private readonly List<MvxOverlay> _overlays = new List<MvxOverlay>();

        private OverlayPresentationService()
        {
        }

        public static OverlayPresentationService Instance { get; } = new OverlayPresentationService();

        private static Context _overlayContext;
        private static Context OverlayContext
        {
            get
            {
                if (_overlayContext == null)
                    _overlayContext = new ContextThemeWrapper(Application.Context, Resource.Style.OverlayTheme);

                return _overlayContext;
            }
        }

        private IWindowManager _windowManager;
        private IWindowManager WindowManager
        {
            get
            {
                if (_windowManager == null)
                    _windowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

                return _windowManager;
            }
        }

        public void ShowOverlayView(Type viewType, MvxBasePresentationAttribute attribute, MvxViewModelRequest request)
        {
            ShowOverlayView(viewType, request);
        }

        public void ShowOverlayView(Type viewType, MvxViewModelRequest request)
        {
            if (!OverlayPermissionService.Instance.CanDrawOverlays)
            {
                return;
            }

            if (!viewType.IsSubclassOf(typeof(MvxOverlay)))
                throw new NotSupportedException($"View type should be derived from {nameof(MvxOverlay)}");

            var viewInstance = (MvxOverlay)Activator.CreateInstance(viewType, OverlayContext);

            if (request is MvxViewModelInstanceRequest instanceRequest)
                viewInstance.ViewModel = instanceRequest.ViewModelInstance;


            ((IMvxEventSourceOverlay)viewInstance).RaiseViewCreated();

            var parameters = viewInstance.LocationParams.ToWindowManagerLayoutParams();

            ((IMvxEventSourceOverlay)viewInstance).RaiseViewWillAttachToWindow();

            WindowManager.AddView(viewInstance.View, parameters);
            _overlays.Add(viewInstance);

            ((IMvxEventSourceOverlay)viewInstance).RaiseViewAttachedToWindow();
        }

        public bool CloseOverlayView(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute)
        {
            return CloseOverlayView(viewModel);
        }

        public bool CloseOverlayView(IMvxViewModel viewModel)
        {
            var viewToClose = _overlays.Find(view => view.ViewModel == viewModel);

            if (viewToClose == null)
            {
                Mvx.Resolve<IMvxLog>().Warn($"No view found for view model: {viewModel.GetType().Name}");
                return false;
            }

            ((IMvxEventSourceOverlay)viewToClose).RaiseViewWillDetachFromWindow();

            _overlays.Remove(viewToClose);
            WindowManager.RemoveView(viewToClose.View);

            ((IMvxEventSourceOverlay)viewToClose).RaiseViewDetachedFromWindow();

            viewToClose.Dispose();

            return true;
        }
    }
}
