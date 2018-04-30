using Android.Content;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using AView = Android.Views.View;

namespace MvvmCross.Plugin.Overlay.Platforms.Android.UI
{
    public abstract class MvxOverlay : MvxEventSourceOverlay, IMvxView, IMvxBindingContextOwner, IMvxLayoutInflaterHolder
    {
        private readonly MvxOverlayAdapter _viewAdapter;

        protected MvxOverlay(Context context)
        {
            Context = MvxContextWrapper.Wrap(context, this);

            BindingContext = new MvxAndroidBindingContext(Context, this);

            _viewAdapter = new MvxOverlayAdapter(this);
        }

        public AView View { get; private set; }

        public OverlayLocationParams LocationParams { get; private set; }

        public Context Context { get; }

        public IMvxBindingContext BindingContext { get; set; }

        public object DataContext
        {
            get => BindingContext.DataContext;
            set => BindingContext.DataContext = value;
        }

        public IMvxViewModel ViewModel
        {
            get => DataContext as IMvxViewModel;
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        private LayoutInflater _layoutInflater;
        public LayoutInflater LayoutInflater
        {
            get
            {
                if (_layoutInflater == null)
                    _layoutInflater = LayoutInflater.From(Context);

                return _layoutInflater;
            }
        }

        public abstract AView CreateAndSetViewBindings();

        public abstract OverlayLocationParams CreateLocationParams();

        protected virtual void OnViewModelSet()
        {
        }

        protected override void OnViewCreated()
        {
            LocationParams = CreateLocationParams();
        }

        protected override void OnViewWillAttachToWindow()
        {
            View = CreateAndSetViewBindings();
        }

        protected override void Dispose(bool disposing)
        {
            _viewAdapter.Dispose();
            View.Dispose();
            base.Dispose(disposing);
        }
    }
}
