using Xamarin.Forms;

namespace DemoApp
{
    public class TabbedPageDemoPage : TabbedPage
    {
        public TabbedPageDemoPage(bool withInnerNavigation = false)
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
                        new Button { Text = "Remove titles", Command = new Command(RemoveTitles) },
                        new Button { Text = "Remove icons", Command = new Command(RemoveIcons) },
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


            AddChild(contentPageA, withInnerNavigation);
            AddChild(contentPageB, withInnerNavigation);
        }

        void AddChild(Page page, bool withInnerNavigation)
        {
            if (withInnerNavigation)
                Children.Add(new NavigationPage(page) {
                    Title = $"Nav {page.Title}",
                    IconImageSource = page.IconImageSource,
                    AutomationId = $"_Nav{page.AutomationId}",
                }.AddPageLog());
            else
                Children.Add(page);
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
            await CurrentPage.Navigation.PushModalAsync(page);
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
            await CurrentPage.Navigation.PushAsync(page);
        }


        void RemoveTitles()
        {
            foreach (var c in Children)
                c.Title = "";
        }

        void RemoveIcons()
        {
            foreach (var c in Children)
                c.IconImageSource = null;
        }
    }
}