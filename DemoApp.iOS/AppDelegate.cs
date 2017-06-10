using DemoApp;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FormsTest.iOS
{
	[Register(nameof(AppDelegate))]
	public class AppDelegate : FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
			Forms.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(uiApplication, launchOptions);
		}
	}
}
