using Xamarin.Forms;

namespace DemoApp
{
    public class TabbedPageDemoPage : TabbedPage
    {
        public TabbedPageDemoPage()
        {
            Title = "TabbedPage";

            var contentPageA = new ContentPage {
                Title = "Tab A",
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is content on tab A"}
                    }
                }
            };
            var contentPageB = new ContentPage {
                Title = "Tab B",
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is content on tab B"}
                    }
                }
            };
            Children.Add(contentPageA);
            Children.Add(contentPageB);
        }
    }
}