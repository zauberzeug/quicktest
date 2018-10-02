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
            ShouldSeeCurrentTab("Tab A");

            Tap("Tab B");

            ShouldSeeCurrentTab("Tab B");
        }
    }
}
