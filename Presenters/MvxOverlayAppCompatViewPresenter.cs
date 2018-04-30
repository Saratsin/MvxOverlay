using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace MvvmCross.Plugin.Overlay.Platforms.Android.Presenters
{
    public class MvxOverlayAppCompatViewPresenter : MvxAppCompatViewPresenter
    {
        public MvxOverlayAppCompatViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

		public override void RegisterAttributeTypes()
		{
            base.RegisterAttributeTypes();

            this.RegisterOverlayAttributeType();
		}
	}
}
