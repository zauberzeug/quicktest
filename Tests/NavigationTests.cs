using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class NavigationTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
        }

        [Test]
        public void TestNavigationStack()
        {
            Tap("PushAsync");
            ShouldSee("Navigation >");
            ShouldNotSee("Navigation", "Menu");

            Tap("PushAsync");
            ShouldSee("Navigation > >");

            Tap("PopAsync");
            ShouldSee("Navigation >");

            Tap("PopAsync");
            ShouldSee("Navigation");

        }

        [Test]
        public void TestModalStack()
        {
            Tap("PushModalAsync");
            ShouldSee("Title: Navigation ^"); // label is visible
            ShouldNotSee("Navigation ^"); // title is not visible because we have no navigation page with title bar
            ShouldNotSee("Navigation", "Menu");

            Tap("PushModalAsync");
            ShouldSee("Title: Navigation ^ ^");

            Tap("PopModalAsync");
            ShouldSee("Title: Navigation ^");
        }

        [Test]
        public void TestNavigationPageOnModalStack()
        {
            Tap("PushModalAsync NavigationPage");
            ShouldSee("Navigation ^");
            ShouldSee("Title: Navigation ^");

            Tap("PushAsync");
            ShouldSee("Navigation ^ >");

            Tap("PopAsync");
            ShouldSee("Navigation ^");

            Tap("PopModalAsync");
            ShouldSee("Navigation");
        }

        [Test]
        public void TestGoBackOnNavigationPageOnModalStack()
        {
            Tap("PushModalAsync NavigationPage");
            ShouldSee("Navigation ^");

            Tap("PushAsync");
            ShouldSee("Navigation ^ >");

            Tap("PushAsync");
            ShouldSee("Navigation ^ > >");

            GoBack();
            ShouldSee("Navigation ^ >");

            GoBack();
            ShouldSee("Navigation ^");

            GoBack();
            ShouldSee("Navigation");
        }

        [Test]
        public void TestGoBackOnPageOnModalStack()
        {
            Tap("PushModalAsync");
            ShouldSee("Title: Navigation ^");

            GoBack();
            ShouldSee("Navigation");
        }

        [Test]
        public void TestPopToRoot()
        {
            Tap("PushAsync");
            Tap("PushAsync");
            Tap("PushAsync");
            ShouldSee("Navigation > > >");

            Tap("PopToRootAsync");
            ShouldSee("Navigation");
        }

        [Test]
        public void TestPageAppearingOnAppStart()
        {
            Assert.That(App.PageLog, Is.EqualTo("A(Navigation) "));
        }

        [Test]
        public void TestPageDisAppearingOnPushPop()
        {
            var expectedLog = "A(Navigation) ";

            Tap("PushAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation >) "));

            GoBack();
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation >) A(Navigation) "));

            Tap("PushModalAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation ^) "));
        }

        [Test]
        public void TestPageDisAppearingOnMenuChange()
        {
            var expectedLog = "A(Navigation) ";

            OpenMenu("Elements");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) "));

            OpenMenu("Navigation");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "A(Navigation) "));

            Tap("PushAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation >) "), "normal navigation should still be possible after menu change");
        }

        [Test]
        public void TestPopToRootEvent()
        {
            var expectedLog = "A(Navigation) ";

            Tap("PushAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation >) "));

            Tap("PopToRootAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation >) A(Navigation) "));
        }

        [Test]
        public void TestModalPopToRootEvent()
        {
            var expectedLog = "A(Navigation) ";

            Tap("PushModalAsync NavigationPage");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation ^) "));

            Tap("PushAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation ^) A(Navigation ^ >) "));

            Tap("PopToRootAsync");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation ^ >) A(Navigation ^) "));
        }

        [Test]
        public void ToggleMainPageBetweenFlyoutAndNavigation()
        {
            var expectedLog = "A(Navigation) ";

            Tap("Toggle Flyout MainPage");
            ShouldSee("Navigation");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation) "));

            Tap("PushAsync");
            ShouldSee("Navigation >");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation >) "));

            Tap("PopAsync");
            ShouldSee("Navigation");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation >) A(Navigation) "));

            Tap("Toggle Flyout MainPage");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation) "));

            OpenMenu("Elements");
            ShouldSee("Element demo");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) "));
        }

        [Test]
        public void CanDisplayAlertOnModalPageWithoutFurtherNavigation()
        {
            Tap("PushModalAsync NavigationPage");
            ShouldSee("Navigation ^");

            Tap("Show Alert");
            ShouldSee("Alert title", "Alert message", "Ok");
            Tap("Ok");
        }
    }
}
