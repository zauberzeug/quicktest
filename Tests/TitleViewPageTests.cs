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
        public void ShouldSeeTitleView()
        {
            ShouldSee("TitleViewLabel");
        }

        [Test]
        public void TapOnButton()
        {
            ShouldSee("TitleViewButton");
            Tap("TitleViewButton");
        }
    }
}
