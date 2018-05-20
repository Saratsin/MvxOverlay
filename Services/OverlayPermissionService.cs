using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Widget;
using AUri = Android.Net.Uri;
using MvvmCross.Base;

namespace MvvmCross.Plugin.Overlay.Platforms.Android.Services
{
    public class OverlayPermissionService
    {
        private OverlayPermissionService()
        {
        }

        public static OverlayPermissionService Instance { get; } = new OverlayPermissionService();

        public bool CanDrawOverlays => Settings.CanDrawOverlays(Application.Context);

        private async Task WaitTillPermissionIsEnabled(TimeSpan timeout)
        {
            var cts = new CancellationTokenSource(timeout);

            while (!CanDrawOverlays || cts.Token.IsCancellationRequested)
                await Task.Delay(500).ConfigureAwait(false);
        }

        private void ShowOverlayPermissionScreen()
        {
            var intent = new Intent(Settings.ActionManageOverlayPermission, AUri.FromParts("package", Application.Context.PackageName, null));
            intent.AddFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }

        private Task ShowToastAsync(string text, ToastLength duration)
        {
            return Mvx.Resolve<IMvxMainThreadAsyncDispatcher>()
                      .ExecuteOnMainThreadAsync(() => Toast.MakeText(Application.Context, text, duration))
                      .ContinueWith(t => Task.Delay(duration.ToTimeSpan()));
        }

        public async Task<bool> TryEnablePermissionIfDisabled(TimeSpan timeout, string toastHintText = null)
        {
            if (CanDrawOverlays)
                return true;

			if(!string.IsNullOrEmpty(toastHintText))
				await ShowToastAsync(toastHintText, ToastLength.Long).ConfigureAwait(false);

			ShowOverlayPermissionScreen();

            await WaitTillPermissionIsEnabled(timeout).ConfigureAwait(false);

            return CanDrawOverlays;
        }
    }
}
