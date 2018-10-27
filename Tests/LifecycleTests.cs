using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class LifecycleTests : QuickTest<App>
    {


        [Test]
        public void AppStartStop()
        {
            Launch(new App());
            Assert.That(App.LifecycleLog, Is.EqualTo("OnStart "));
        }

    }
}