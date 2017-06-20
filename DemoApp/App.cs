using Xamarin.Forms;

namespace DemoApp
{
    public class App : Application
    {
        public static string PageLog;

        public App()
        {
            PageLog = "";

            ToggleMasterDetail();
        }

        public void ToggleMasterDetail()
        {
            if (MainPage is MasterDetailPage)
                MainPage = new NavigationPage(new NavigationDemoPage());
            else
                MainPage = new MasterDetailPage {
                    Master = new MenuPage(),
                    Detail = new NavigationPage(new NavigationDemoPage()),
                };
        }

        public static void ShowMessage(string title, string message)
        {
            Current.MainPage.DisplayAlert(title, message, "Ok");
        }
    }
}
