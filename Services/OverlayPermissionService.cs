using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Widget;
using AUri = Android.Net.Uri;

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

        private async Task ShowOverlayPermissionScreenAsync()
        {
            await ShowToastAsync(Resource.String.no_overlay_permission_toast, ToastLength.Long).ConfigureAwait(false);
            var intent = new Intent(Settings.ActionManageOverlayPermission, AUri.FromParts("package", Application.Context.PackageName, null));
            intent.AddFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }

        private Task ShowToastAsync(int textResId, ToastLength duration)
        {
            Toast.MakeText(Application.Context, textResId, duration);
            return Task.Delay(duration.ToTimeSpan());
        }

        public async Task<bool> TryEnablePermissionIfDisabled(TimeSpan timeout)
        {
            if (CanDrawOverlays)
                return true;

            await ShowOverlayPermissionScreenAsync().ConfigureAwait(false);

            await WaitTillPermissionIsEnabled(timeout).ConfigureAwait(false);

            return CanDrawOverlays;
        }
    }
}
