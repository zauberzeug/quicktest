using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class TabbedTitleViewPageTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("TabbedTitleViewPage");
        }

        [Test]
        public void Test()
        {
            Tap("TitleViewPage");
            Tap("TitleViewLabel");
            ShouldSee("Tapped on Label");
        }
    }
}
