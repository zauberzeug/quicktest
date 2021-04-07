using System;
using DemoApp;
using NUnit.Framework;
using QuickTest;
using Xamarin.Forms;

namespace Tests
{
    public class UsageTests : QuickTest<App>
    {

        [Test]
        public void ErrorWhenNotLaunchingApp()
        {
            Assert.Throws<LaunchException>(() => ShouldSee("something"));
            Launch(new App());
            ShouldSee("Navigation");
        }

        [Test]
        public void SettingNavigationPageWhileModalPageIsShown()
        {
            Launch(new App());
            App.MainPage.Navigation.PushModalAsync(new ContentPage { Content = new Label { Text = "Modal Page" } });
            App.Flyout.Detail = new NavigationPage(new ContentPage { Content = new Label { Text = "New Page" } });
            ShouldSee("Modal Page");
        }

    }
}
