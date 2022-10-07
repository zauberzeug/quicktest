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
                        new Button { Text = "Clear children", Command = new Command(() => Children.Clear()) },
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

            ToolbarItems.Add(new ToolbarItem {
                Text = "Add tabs",
                Command = new Command(AddTabs),
            });
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

        void Test()
        {
            var childCount = Children.Count;
            var dummyPage = new ContentPage();
            for (int i = 0; i < childCount; i++)
                Children.RemoveAt(i);
            // add new pages
            Children.Remove(dummyPage);
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

        void AddTabs()
        {
            Children.Add(new ContentPage { Title = "New tab A", Content = new Label { Text = "Content new tab A" } });
            Children.Add(new ContentPage { Title = "New tab B", Content = new Label { Text = "Content new tab B" } });
        }
    }
}