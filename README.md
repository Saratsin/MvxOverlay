# MvxOverlay.Android

MvvmCross plugin for creating overlay views and show/close it with NavigationService

## Getting Started

First of all, make sure you really need such kind of stuff, system overlays are very uncommon thing for apps and it's always better to use Activities or Fragments. Also if you're sure, then welcome

### Prerequisites

Things you need:
* Xamarin.Android project
* MvvmCross (6.0 or higher)

### Installing

First of all, add an overlay permission to your AndroidManifest.xml:

```
<uses-permission android:name="android.permission.SYSTEM_ALERT_WINDOW" />
```

Before showing app overlay, you can check if your app has the overlay permissions (by default it disabled).

```
using System;
using MvvmCross.Plugin.Overlay.Platforms.Android.Services;

...

var overlayPermissionIsEnabled = OverlayPermissionService.Instance.CanDrawOverlays;

var overlayPermissionIsOrBecomeEnabled = await OverlayPermissionService.Instance.TryEnablePermissionIfDisabled(timeout: TimeSpan.FromMinutes(1), toastHintText: "Looks like we have no permissions :(");

```

If you won't do it, plugin will ask it by itself anyway.

### Overlay Example

Android view implementation

```
public class ExampleOverlay : MvxOverlay<ExampleViewModel>
{
    public ExampleOverlay(Context context) : base(context)
    { 
    }

    public override View CreateAndSetViewBindings()
    {
        var button = new Button(Context)
        {
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
        };

        var set = this.CreateBindingSet<ExampleOverlay, ExampleViewModel>();

        set.Bind(_button).For(v => v.Text).To(vm => vm.ButtonText);
        set.Bind(_button).For(nameof(View.Click)).To(vm => vm.ButtonClickedCommand);
        set.Bind(_button).For(nameof(View.LongClick)).To(vm => vm.ConnectButtonLongClickedCommand);

        set.Apply();

        return frame;
    }

    public override OverlayLocationParams CreateLocationParams()
    {
        return new OverlayLocationParams(
            gravity: GravityFlags.CenterHorizontal | GravityFlags.Top, 
            y: 50
        );
    }
}
```

ViewModel showing (inside the core project)

```
await Mvx.Resolve<IMvxNavigationService>().Navigate<ExampleViewModel>().ConfigureAwait(false);
```


## Authors

* **Taras Shevchuk** - *Initial work* - [Saratsin](https://github.com/Saratsin)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
