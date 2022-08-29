using Xamarin.Forms;

namespace DemoApp
{
    public class TabbedPageDemoPage : TabbedPage
    {
        public TabbedPageDemoPage()
        {
            Title = "TabbedPage";
            PageExtensions.AddPageLog(this);

            var contentPageA = new ContentPage {
                Title = "Tab A",
                IconImageSource = "one.png",
                AutomationId = "_Tab_A_AutomationId_",
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is content on tab A"},
                        new Button{Text = "Open ModalPage", Command = new Command(OpenModalPage)},
                        new Button{Text = "Open Subpage", Command = new Command(OpenSubpage)},
                    }
                }
            }.AddPageLog();

            var contentPageB = new ContentPage {
                Title = "Tab B",
                IconImageSource = "two.png",
                AutomationId = "_Tab_B_AutomationId_",
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is content on tab B"}
                    },
                },
                ToolbarItems = { new DemoToolbarItem() }
            }.AddPageLog();

            Children.Add(contentPageA);
            Children.Add(contentPageB);
        }

        async void OpenModalPage()
        {
            var page = new ContentPage {
                Title = "Modal",
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is a modal page"},
                        new Button{Text = "Close", Command = new Command(() => Navigation.PopModalAsync())},
                    }
                }
            }.AddPageLog();
            await Navigation.PushModalAsync(page);
        }

        async void OpenSubpage()
        {
            var page = new ContentPage {
                Title = "Subpage",
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is a sub page"},
                    }
                }
            }.AddPageLog();
            await Navigation.PushAsync(page);
        }
    }
}