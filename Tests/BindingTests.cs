using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class BindingTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();
            Launch(new App());

            OpenMenu("Binding");
        }

        [Test]
        public void TestBinding()
        {
            ShouldSee("updated bound text");
        }
    }
}
