using System;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Plugin.Overlay.Platforms.Android.Services;
using MvvmCross.Presenters;

namespace MvvmCross.Plugin.Overlay.Platforms.Android
{
    public static class Extensions
    {
        internal static TimeSpan ToTimeSpan(this ToastLength duration)
        {
            switch (duration)
            {
                case ToastLength.Short:
                    return TimeSpan.FromMilliseconds(2000);
                default:
                    return TimeSpan.FromMilliseconds(3500);
            }
        }

        internal static WindowManagerLayoutParams ToWindowManagerLayoutParams(this OverlayLocationParams locationParams)
        {
            var type = WindowManagerTypes.Phone;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                type = WindowManagerTypes.ApplicationOverlay;


            return new WindowManagerLayoutParams(w: locationParams.Width,
                                                 h: locationParams.Height,
                                                 xpos: locationParams.X,
                                                 ypos: locationParams.Y,
                                                 _type: type,
                                                 _flags: WindowManagerFlags.NotFocusable | WindowManagerFlags.NotTouchModal,
                                                 _format: Format.Translucent)
            {
                Gravity = locationParams.Gravity
            };
        }

        public static void RegisterOverlayAttributeType(this IMvxAttributeViewPresenter presenter)
        {
            presenter.AttributeTypesToActionsDictionary.Add(
                typeof(MvxOverlayPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = OverlayPresentationService.Instance.ShowOverlayView,
                    CloseAction = OverlayPresentationService.Instance.CloseOverlayView
                }
            );
        }
    }
}
