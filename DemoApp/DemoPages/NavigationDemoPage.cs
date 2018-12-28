using System;
using QuickTest;
using QuickTestShared;
using Xamarin.Forms;

namespace DemoApp
{
    public class NavigationDemoPage : ContentPage
    {
        public NavigationDemoPage(string title = "Navigation")
        {
            Title = title;

            Content = new StackLayout {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = {
                    new DemoLabel("Navigation stack:"),
                    new DemoButton("PushAsync") {
                        Command = new Command(o => Navigation.PushAsync(new NavigationDemoPage(title + " >"))),
                    },
                    new DemoButton("PopAsync") {
                        Command = new Command(obj => Navigation.PopAsync()),
                    },
                    new DemoButton("PopToRootAsync") {
                        Command = new Command(obj => Navigation.PopToRootAsync()),
                    },
                    new DemoLabel("Modal stack:"),
                    new DemoButton("PushModalAsync") {
                        Command = new Command(o => Navigation.PushModalAsync(new NavigationDemoPage(title + " ^"))),
                    },
                    new DemoButton("PopModalAsync") {
                        Command = new Command(obj => Navigation.PopModalAsync()),
                    },
                    new DemoButton("PushModalAsync NavigationPage") {
                        Command = new Command(o => Navigation.PushModalAsync(new NavigationPage(new NavigationDemoPage(title + " ^")))),
                    },
                    new DemoLabel("MasterDetail:"),
                    new DemoButton("Toggle MasterDetail MainPage") {
                        Command = new Command(obj => (Application.Current as App).ToggleMasterDetail()),
                    },
                    new DemoButton("Walkthrough") {
                        Command = new Command(obj => WalkThroughApp()),
                    },
                },
            };

            if (title.EndsWith("^", StringComparison.Ordinal))
                (Content as StackLayout).Children.Insert(0, new DemoLabel("Title: " + title));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            App.PageLog += $"A({Title}) ";
        }

        protected override void OnDisappearing()
        {
            App.PageLog += $"D({Title}) ";

            base.OnDisappearing();
        }

        private void WalkThroughApp()
        {
            var user = (Application.Current as App).User;

            user.Tap("PushAsync");
            user.Tap("PopAsync");
            user.Tap("PushModalAsync");
            user.Tap("PopModalAsync");

            user.OpenMenu();
            user.Tap("Elements");
            var screenshotData = DependencyService.Get<IScreenshotService>().Capture();
            DependencyService.Get<IScreenshotService>().Save("test.jpg", screenshotData);

            user.OpenMenu();
            user.Tap("ListViews");
            user.Tap("DemoListViewWithTextCell");
            user.GoBack();

            user.OpenMenu();
            user.Tap("Binding");

            //Alerts don't work yet: How to dismiss alert programmatically?
            //user.OpenMenu();
            //user.Tap("Alert");
            //user.Tap("Ok");

            user.OpenMenu();
            user.Tap("TabbedPage");
            user.Tap("Tab B");
            user.Tap("Tab A");
            user.Tap("Open ModalPage");
            user.Tap("Close");
            user.Tap("Open Subpage");
            user.GoBack();
        }
    }
}
