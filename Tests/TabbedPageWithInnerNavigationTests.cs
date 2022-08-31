using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class TabbedPageWithInnerNavigationTests : QuickTest<App>
    {
        string expectedLog;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("TabbedPage inner navigation");

            expectedLog = "A(FlyoutPage) A(NavigationPage) A(Navigation) D(Navigation) D(NavigationPage) A(TabbedPage) A(Nav Tab A) A(Tab A) ";
            Assert.That(App.PageLog, Is.EqualTo(expectedLog));
        }

        [Test]
        public void SwitchTab()
        {
            //ShouldSee("Tab A", "Nav Tab A", "Nav Tab B", "This is content on tab A");
            ShouldSee("Tab A");
            ShouldSee("Nav Tab A");
            ShouldSee("Nav Tab B");
            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B", "ToolbarItem");

            Tap("Nav Tab B");

            ShouldSee("Tab B", "This is content on tab B");
            ShouldNotSee("Tab A", "This is content on tab A");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) D(Nav Tab A) A(Nav Tab B) A(Tab B) "));

            Tap("ToolbarItem");
            ShouldSee("ToolbarItem tapped");

            Tap("Ok");

            Tap("Nav Tab A");

            ShouldSee("Tab A", "This is content on tab A");
            ShouldNotSee("Tab B", "This is content on tab B");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab B) D(Nav Tab B) A(Nav Tab A) A(Tab A) "));
        }

        [Test]
        public void SwitchTabWithAutomationId()
        {
            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B");

            Tap("_Nav_Tab_B_AutomationId_");

            ShouldSee("This is content on tab B");
            ShouldNotSee("This is content on tab A");

            Tap("_Nav_Tab_A_AutomationId_");

            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B");
        }

        [Test]
        public void SwitchTabWithIconImageSource()
        {
            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B");

            Tap("two.png");

            ShouldSee("This is content on tab B");
            ShouldNotSee("This is content on tab A");

            Tap("one.png");

            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B");
        }

        [Test]
        public void ModalPageOverTabbedPage()
        {
            Assert.That(App.PageLog, Is.EqualTo(expectedLog));
            ShouldSee("Tab A");

            Tap("Open ModalPage");
            ShouldSee("This is a modal page");
            ShouldNotSee("Tab A");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) D(Nav Tab A) D(TabbedPage) D(FlyoutPage) A(Modal) "));
            Tap("Close");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Modal) A(FlyoutPage) A(TabbedPage) A(Nav Tab A) A(Tab A) "));
        }

        [Test]
        public void NavigationFromTabbedPage()
        {
            Assert.That(App.PageLog, Is.EqualTo(expectedLog));
            ShouldSee("Tab A");

            Tap("Open Subpage");
            ShouldSee("This is a sub page");
            ShouldNotSee("Tab A");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) A(Subpage) "));
            GoBack();
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Subpage) A(Tab A) "));
        }
    }
}
