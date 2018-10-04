using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class TabbedPageTests : IntegrationTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            OpenMenu("TabbedPage");
        }

        [Test]
        public void SwitchTab()
        {
            var expectedLog = "A(Navigation) D(Navigation) ";

            ShouldSee("TabbedPage", "Tab A", "Tab B", "This is content on tab A");
            ShouldNotSee("This is content on tab B", "ToolbarItem");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "A(TabbedPage) A(Tab A) "));

            Tap("Tab B");

            ShouldSee("This is content on tab B");
            ShouldNotSee("This is content on tab A");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) A(Tab B) "));

            Tap("ToolbarItem");
            ShouldSee("ToolbarItem tapped");
        }

        [Test]
        public void ModalPageOverTabbedPage()
        {
            var expectedLog = "A(Navigation) D(Navigation) A(TabbedPage) A(Tab A) ";
            Assert.That(App.PageLog, Is.EqualTo(expectedLog));
            ShouldSee("TabbedPage");

            Tap("Open ModalPage");
            ShouldSee("This is a modal page");
            ShouldNotSee("TabbedPage");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) A(Modal) "));
            Tap("Close");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Modal) A(Tab A) "));
        }
    }
}
