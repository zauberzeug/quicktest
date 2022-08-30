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
        public void TitleViewIsVisibleWithTabbedPages()
        {
            ShouldNotSee("TabbedTitleViewPage"); // normal title is not visible
            Tap("TitleViewPage");
            Tap("TitleViewLabel");
            ShouldSee("Tapped on Label");

            Tap("ContentPage");
            ShouldSee("Some content");
            ShouldNotSee("Tapped on Label");
        }
    }
}
