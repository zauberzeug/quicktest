using Xamarin.Forms;

namespace DemoApp
{
    public class TabbedPageDemoPage : TabbedPage
    {
        public TabbedPageDemoPage()
        {
            var contentPageA = new ContentPage { Title = "Content Page A" };
            var contentPageB = new ContentPage { Title = "Content Page B" };

            Children.Add(contentPageA);
            Children.Add(contentPageB);
        }
    }
}