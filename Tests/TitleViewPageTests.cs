using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class TitleViewPageTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("TitleViewPage");
        }

        [Test]
        public void ShouldSeeTitleViewLabel()
        {
            ShouldSee("TitleViewLabel");
        }

        [Test]
        public void ShouldSeeTitleViewButton()
        {
            ShouldSee("TitleViewButton");
        }

        [Test]
        public void TapOnLabel()
        {
            Tap("TitleViewLabel");
            ShouldSee("Tapped on Label");
        }

        [Test]
        public void TapOnButton()
        {
            Tap("TitleViewButton");
            ShouldSee("Tapped on Button");
        }
    }
}
