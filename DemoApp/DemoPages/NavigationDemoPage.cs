using System;
using Xamarin.Forms;

namespace DemoApp
{
    public class NavigationDemoPage : ContentPage
    {
        public NavigationDemoPage(string title = "Navigation")
        {
            Title = title;
            PageExtensions.AddPageLog(this);

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
                        Command = new Command(o => Navigation.PushModalAsync(new NavigationPage(new NavigationDemoPage(title + " ^")).AddPageLog())),
                    },
                    new DemoLabel("Flyout:"),
                    new DemoButton("Toggle Flyout MainPage") {
                        Command = new Command(obj => (Application.Current as App).ToggleFlyout()),
                    },
                    new DemoButton("Show Alert") {
                        Command = new Command(obj => ShowAlert()),
                    },
                },
            };

            if (title.EndsWith("^", StringComparison.Ordinal))
                (Content as StackLayout).Children.Insert(0, new DemoLabel("Title: " + title));
        }

        async void ShowAlert()
        {
            await DisplayAlert("Alert title", "Alert message", "Ok");
        }
    }
}
