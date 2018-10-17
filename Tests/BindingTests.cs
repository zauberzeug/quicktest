using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class BindingTests : IntegrationTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();
            LaunchApp();

            OpenMenu("Binding");
        }

        [Test]
        public void TestBinding()
        {
            ShouldSee("updated bound text");
        }
    }
}
