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
        public void SwitchBetweenTabs()
        {
            ShouldSee("Tab A");
            Tap("Tab B");
            ShouldSee("Tab B");
        }
    }
}
