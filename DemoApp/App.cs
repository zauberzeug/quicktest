using Xamarin.Forms;

namespace DemoApp
{
    public class App : Application
    {
        public static string PageLog;
        public static string LifecycleLog;

        public static App Instance;

        public App()
        {
            PageLog = LifecycleLog = "";

            ToggleFlyout();
            Instance = this;
        }


        public FlyoutPage Flyout { get => MainPage as FlyoutPage; }

        public void ToggleFlyout()
        {
            if (MainPage is FlyoutPage)
                MainPage = new NavigationPage(new NavigationDemoPage()).AddPageLog();
            else
                MainPage = new FlyoutPage {
                    Flyout = new MenuPage(),
                    Detail = new NavigationPage(new NavigationDemoPage()).AddPageLog(),
                }.AddPageLog();
        }

        public static void ShowMessage(string title, string message)
        {
            Current.MainPage.DisplayAlert(title, message, "Ok");
        }

        protected override void OnStart()
        {
            base.OnStart();
            LifecycleLog += "OnStart ";
        }
    }
}
