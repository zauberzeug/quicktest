using System;
using Xamarin.Forms;

namespace DemoApp
{
    public class MenuPage : ContentPage
    {
        public MenuPage()
        {
            Title = "Menu";

            Content = new ScrollView {
                Content = new StackLayout {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Children = {
                        new DemoLabel("Menu"),
                        CreateMenuButton("Navigation", () => new NavigationDemoPage()),
                        CreateMenuButton("Elements", () => new ElementDemoPage()),
                        CreateListViewButton(ListViewCachingStrategy.RetainElement),
                        CreateListViewButton(ListViewCachingStrategy.RecycleElement),
                        CreateListViewButton(ListViewCachingStrategy.RecycleElementAndDataTemplate),
                        CreateMenuButton("Binding", () => new BindingDemoPage()),
                        CreateMenuButton("Popups", () => new PopupDemoPage()),
                        CreateMenuButton("TabbedPage", () => new TabbedPageDemoPage()),
                        CreateMenuButton("TabbedPage inner navigation", () => new TabbedPageDemoPage(withInnerNavigation: true), withNavigationPage: false),
                        CreateMenuButton("CarouselPage", () => new CarouselDemoPage()),
                        CreateMenuButton("TitleViewPage", () => new TitleViewPage()),
                        CreateMenuButton("TabbedTitleViewPage", () => new TabbedTitleViewPage()),
                        CreateMenuButton("NestedTabbedPage", () => new NestedTabbedPage()),
                        CreateMenuButton("NestedTabbedPage inner navigation", () => new NestedTabbedPage(withInnerNavigation: true), withNavigationPage: false),
                        CreateMenuButton("Empty ContentPage", () => new ContentPage() { Title = "Page with no content", Content = null }),
                        new DemoButton("Show alert from menu") {
                            Command = new Command(() => DisplayAlert("Alert", "Message", "Ok"))
                        },
                        new DemoButton("Show action sheet from menu") {
                            Command = new Command(() => DisplayActionSheet("Alert", "Cancel", "Delete", "Keep"))
                        },
                    },
                }
            };
        }

        DemoButton CreateListViewButton(ListViewCachingStrategy cachingStrategy)
        {
            return CreateMenuButton($"ListViews ({cachingStrategy})", () => new ListViewDemoPage(cachingStrategy));
        }

        DemoButton CreateMenuButton(string title, Func<Page> pageCreator, bool withNavigationPage = true)
        {
            return new DemoButton(title) {
                Command = new Command(o => {
                    var mainPage = (Application.Current.MainPage as FlyoutPage);
                    if (withNavigationPage)
                        mainPage.Detail = new NavigationPage(pageCreator.Invoke()).AddPageLog();
                    else
                        mainPage.Detail = pageCreator.Invoke();
                    mainPage.IsPresented = false;
                }),
            };
        }
    }
}
