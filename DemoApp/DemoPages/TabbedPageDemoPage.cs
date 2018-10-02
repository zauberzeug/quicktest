using Xamarin.Forms;

namespace DemoApp
{
    public class TabbedPageDemoPage : TabbedPage
    {
        public TabbedPageDemoPage()
        {
            Title = "TabbedPage";

            var contentPageA = new ContentPage { Title = "Tab A" };
            var contentPageB = new ContentPage { Title = "Tab B" };

            Children.Add(contentPageA);
            Children.Add(contentPageB);
        }
    }
}