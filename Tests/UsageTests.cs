using System;
using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class UsageTests : QuickTest<App>
    {

        [Test]
        public void ErrorWhenNotLaunchingApp()
        {
            Assert.Throws<LaunchException>(() => ShouldSee("something"));
            Launch(new App());
            ShouldSee("Navigation");
        }
    }
}
