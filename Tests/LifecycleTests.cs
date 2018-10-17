using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class LifecycleTests : IntegrationTest<App>
    {


        [Test]
        public void AppStartStop()
        {
            LaunchApp();
            Assert.That(App.LifecycleLog, Is.EqualTo("OnStart "));
        }

    }
}