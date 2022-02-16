using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class CarouselPageTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("CarouselPage");
        }

        [Test]
        public void TestLabel()
        {
            ShouldSee("Label on a carouselpage");
        }
    }
}
