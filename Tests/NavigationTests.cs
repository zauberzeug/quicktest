using DemoApp;
using NUnit.Framework;
using UserFlow;
using Xamarin.Forms;

namespace Tests
{
    public class NavigationTests : IntegrationTest<App>
    {
        [Test]
        public void TestNavigationStack()
        {
            Tap("PushAsync");
            ShouldSee("Navigation demo >");
            ShouldNotSee("Navigation demo", "Menu");

            Tap("PushAsync");
            ShouldSee("Navigation demo > >");

            Tap("PopAsync");
            ShouldSee("Navigation demo >");
        }

        [Test]
        public void TestModalStack()
        {
            Tap("PushModalAsync");
            ShouldSee("Navigation demo ^");
            ShouldNotSee("Navigation demo", "Menu");

            Tap("PushModalAsync");
            ShouldSee("Navigation demo ^ ^");

            Tap("PopModalAsync");
            ShouldSee("Navigation demo ^");
        }

        [Test]
        public void TestNavigationPageOnModalStack()
        {
            Tap("PushModalAsync NavigationPage");
            ShouldSee("Navigation demo ^");

            Tap("PushAsync");
            ShouldSee("Navigation demo ^ >");

            Tap("PopAsync");
            ShouldSee("Navigation demo ^");

            Tap("PopModalAsync");
            ShouldSee("Navigation demo");
        }

        [Test]
        public void TestPopToRoot()
        {
            Tap("PushAsync");
            Tap("PushAsync");
            Tap("PushAsync");
            ShouldSee("Navigation demo > > >");

            Tap("PopToRootAsync");
            ShouldSee("Navigation demo");
        }

        [Test]
        public void TestPageAppearingOnAppStart()
        {
            Assert.That(App.PageLog, Is.EqualTo(" Appeared"));
        }

        [Test]
        public void TestPageDisAppearingOnPushPop()
        {
            var expectedLog = " Appeared";

            Tap("PushAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));

            GoBack();
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));

            Tap("PushModalAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));
        }

        [Test]
        public void TestPageDisAppearingOnMenuChange()
        {
            var expectedLog = " Appeared";

            OpenMenu("Elements");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared"));

            OpenMenu("Navigation");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Appeared"));
        }

        [Test]
        public void TestPopToRootEvent()
        {
            var expectedLog = " Appeared";

            Tap("PushAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));

            Tap("PopToRootAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));
        }

        [Test]
        public void TestModalPopToRootEvent()
        {
            var expectedLog = " Appeared";

            Tap("PushModalAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));

            Tap("PushAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));

            Tap("PopToRootAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));
        }

        [Test]
        public void ToggleMainPageBetweenMasterDetailAndNavigation()
        {
            Tap("Toggle MasterDetail MainPage");
            ShouldSee("Navigation demo");

            var expectedLog = " Appeared Disappeared Appeared";
            Assert.That(App.PageLog, Is.EqualTo(expectedLog));

            Tap("PushAsync");
            ShouldSee("Navigation demo >");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));

            Tap("PopAsync");
            ShouldSee("Navigation demo");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));

            Tap("Toggle MasterDetail MainPage");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += " Disappeared Appeared"));

            OpenMenu("Elements");
            ShouldSee("Element demo");
        }

        [Test]
        public void TestOnlyContentPagesAreSupported()
        {
            App.CurrentNavigationPage.PushAsync(new Page());
            ShouldSee("The expected page is not of type \"ContentPage\"");
        }
    }
}
