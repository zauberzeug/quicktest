using DemoApp;
using NUnit.Framework;

namespace Tests
{
    public class BindingTests : IntegrationTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            OpenMenu("Binding");
        }

        [Test]
        public void TestBinding()
        {
            ShouldSee("updated bound text");
        }
    }
}
