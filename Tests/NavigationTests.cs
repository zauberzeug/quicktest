using DemoApp;
using NUnit.Framework;

namespace Tests
{
    public class NavigationTests : IntegrationTest<App>
    {
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
        }

        [Test]
        public void TestModalStack()
        {
            Tap("PushModalAsync");
            ShouldSee("Navigation ^");
            ShouldNotSee("Navigation", "Menu");

            Tap("PushModalAsync");
            ShouldSee("Navigation ^ ^");

            Tap("PopModalAsync");
            ShouldSee("Navigation ^");
        }

        [Test]
        public void TestNavigationPageOnModalStack()
        {
            Tap("PushModalAsync NavigationPage");
            ShouldSee("Navigation ^");

            Tap("PushAsync");
            ShouldSee("Navigation ^ >");

            Tap("PopAsync");
            ShouldSee("Navigation ^");

            Tap("PopModalAsync");
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
        public void ToggleMainPageBetweenMasterDetailAndNavigation()
        {
            var expectedLog = "A(Navigation) ";

            Tap("Toggle MasterDetail MainPage");
            ShouldSee("Navigation");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation) "));

            Tap("PushAsync");
            ShouldSee("Navigation >");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation >) "));

            Tap("PopAsync");
            ShouldSee("Navigation");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation >) A(Navigation) "));

            Tap("Toggle MasterDetail MainPage");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) A(Navigation) "));

            OpenMenu("Elements");
            ShouldSee("Element demo");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Navigation) "));
        }
    }
}
