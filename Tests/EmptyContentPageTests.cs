using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    [TestFixture]
    public class EmptyContentPageTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("Empty ContentPage");
        }

        [Test]
        public void CanSeeTitleOfContentPageWithNoContent()
        {
            ShouldSee("Page with no content");
        }
    }
}
