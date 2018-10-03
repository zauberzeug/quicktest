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
            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B");

            Tap("Tab B");

            ShouldSee("This is content on tab B");
            ShouldNotSee("This is content on tab A");
        }
    }
}
