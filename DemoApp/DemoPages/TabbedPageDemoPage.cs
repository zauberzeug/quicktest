using Xamarin.Forms;

namespace DemoApp
{
    public class TabbedPageDemoPage : TabbedPage
    {
        public TabbedPageDemoPage()
        {
            Title = "TabbedPage";

            Appearing += (s, e) => App.PageLog += $"A({Title}) ";
            Disappearing += (s, e) => App.PageLog += $"D({Title}) ";

            var contentPageA = new ContentPage {
                Title = "Tab A",
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is content on tab A"},
                        new Button{Text = "Open ModalPage", Command = new Command(OpenModalPage)},
                        new Button{Text = "Open Subpage", Command = new Command(OpenSubpage)},
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
                    },
                },
                ToolbarItems = { new DemoToolbarItem() }
            };
            contentPageB.Appearing += (s, e) => App.PageLog += $"A(Tab B) ";
            contentPageB.Disappearing += (s, e) => App.PageLog += $"D(Tab B) ";

            Children.Add(contentPageA);
            Children.Add(contentPageB);
        }

        async void OpenModalPage()
        {
            var page = new ContentPage {
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is a modal page"},
                        new Button{Text = "Close", Command = new Command(() => Navigation.PopModalAsync())},
                    }
                }
            };
            page.Appearing += (s, e) => App.PageLog += $"A(Modal) ";
            page.Disappearing += (s, e) => App.PageLog += $"D(Modal) ";
            await Navigation.PushModalAsync(page);
        }

        async void OpenSubpage()
        {
            var page = new ContentPage {
                Title = "Some Subpage",
                Content = new StackLayout {
                    Children = {
                        new Label{Text = "This is a sub page"},
                    }
                }
            };
            page.Appearing += (s, e) => App.PageLog += $"A(Subpage) ";
            page.Disappearing += (s, e) => App.PageLog += $"D(Subpage) ";
            await Navigation.PushAsync(page);
        }
    }
}