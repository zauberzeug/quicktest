using System;
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
                    new DemoLabel("Flyout:"),
                    new DemoButton("Toggle Flyout MainPage") {
                        Command = new Command(obj => (Application.Current as App).ToggleFlyout()),
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
    }
}
