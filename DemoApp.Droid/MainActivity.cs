using Android.App;
using Android.Content.PM;
using Android.OS;
using DemoApp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FormsTest.Droid
{
    [Activity(
        Label = nameof(DemoApp),
        Icon = "@drawable/icon",
        Theme = "@style/MyTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            LoadApplication(new App());

            Instance = this;
            DependencyService.Register<ScreenshotService>();
        }
    }
}
