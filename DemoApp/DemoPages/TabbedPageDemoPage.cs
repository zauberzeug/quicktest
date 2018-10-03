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
            contentPageA.Appearing += (s, e) => App.PageLog += $"A(Tab A) ";
            contentPageA.Disappearing += (s, e) => App.PageLog += $"D(Tab A) ";

            var contentPageB = new ContentPage {
                Title = "Tab B",
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is content on tab B"}
                    }
                }
            };
            contentPageB.Appearing += (s, e) => App.PageLog += $"A(Tab B) ";
            contentPageB.Disappearing += (s, e) => App.PageLog += $"D(Tab B) ";

            Children.Add(contentPageA);
            Children.Add(contentPageB);
        }
    }
}