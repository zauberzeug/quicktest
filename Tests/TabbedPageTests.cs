using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class TabbedPageTests : QuickTest<App>
    {
        string expectedLog;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("TabbedPage");

            expectedLog = "A(FlyoutPage) A(NavigationPage) A(Navigation) D(Navigation) D(NavigationPage) A(NavigationPage) A(TabbedPage) A(Tab A) ";
            Assert.That(App.PageLog, Is.EqualTo(expectedLog));
        }

        [Test]
        public void SwitchTab()
        {
            ShouldSee("TabbedPage", "Tab A", "Tab B", "This is content on tab A");
            ShouldNotSee("This is content on tab B", "ToolbarItem");

            Tap("Tab B");

            ShouldSee("This is content on tab B");
            ShouldNotSee("This is content on tab A");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) A(Tab B) "));

            Tap("ToolbarItem");
            ShouldSee("ToolbarItem tapped");

            Tap("Ok");

            Tap("Tab A");

            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab B) A(Tab A) "));
        }

        [Test]
        public void SwitchTabWithAutomationId()
        {
            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B");

            Tap("_Tab_B_AutomationId_");

            ShouldSee("This is content on tab B");
            ShouldNotSee("This is content on tab A");

            Tap("_Tab_A_AutomationId_");

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
            ShouldSee("TabbedPage");

            Tap("Open ModalPage");
            ShouldSee("This is a modal page");
            ShouldNotSee("TabbedPage");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) D(TabbedPage) D(NavigationPage) D(FlyoutPage) A(Modal) "));
            Tap("Close");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Modal) A(FlyoutPage) A(NavigationPage) A(TabbedPage) A(Tab A) "));
        }

        [Test]
        public void NavigationFromTabbedPage()
        {
            Assert.That(App.PageLog, Is.EqualTo(expectedLog));
            ShouldSee("TabbedPage");

            Tap("Open Subpage");
            ShouldSee("This is a sub page");
            ShouldNotSee("TabbedPage");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) D(TabbedPage) A(Subpage) "));
            GoBack();
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Subpage) A(TabbedPage) A(Tab A) "));
        }

        // Quicktest must support currentpage of container pages to become null
        [Test]
        public void CanClearChildren()
        {
            ShouldSee("TabbedPage");
            Assert.DoesNotThrow(() => Tap("Clear children"));
            ShouldSee("TabbedPage");
        }
    }
}
