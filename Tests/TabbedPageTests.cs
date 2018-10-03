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

            ShouldSee("This is content on tab A");
            ShouldNotSee("This is content on tab B");
            Assert.That(App.PageLog, Is.EqualTo(expectedLog += "A(Tab A) "));

            Tap("Tab B");

            ShouldSee("This is content on tab B");
            ShouldNotSee("This is content on tab A");
            //Assert.That(App.PageLog, Is.EqualTo(expectedLog += "D(Tab A) A(Tab B)"));
        }
    }
}
