using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Platforms.Android.Presenters;

namespace MvvmCross.Plugin.Overlay.Platforms.Android.Presenters
{
    public class MvxOverlayAndroidViewPresenter : MvxAndroidViewPresenter
    {
        public MvxOverlayAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

		public override void RegisterAttributeTypes()
		{
            base.RegisterAttributeTypes();

            this.RegisterOverlayAttributeType();
		}
	}
}