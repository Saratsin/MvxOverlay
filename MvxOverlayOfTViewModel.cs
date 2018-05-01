using Android.Content;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Plugin.Overlay.Platforms.Android
{
    public abstract class MvxOverlay<TViewModel> : MvxOverlay, IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxOverlay(Context context) : base(context)
        {
        }

        public new TViewModel ViewModel
        {
            get => base.ViewModel as TViewModel;
            set => base.ViewModel = value;
        }
    }
}
