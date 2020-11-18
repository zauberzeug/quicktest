using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    [TestFixture]
    public class PopupTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("Popups");
            ShouldSee("Popup demo");
        }

        [TestCase("Yes", "Alert result: True")]
        [TestCase("No", "Alert result: False")]
        public void TestYesNoAlert(string alertButton, string expectedResult)
        {
            Tap("Show yes/no alert");
            ShouldSee("Alert", "Message", "Yes", "No");
            ShouldNotSee("Popup demo");

            Tap(alertButton);
            ShouldSee(expectedResult);
            ShouldSee("Popup demo");
        }

        [Test]
        public void TestOkAlert()
        {
            Tap("Show ok alert");
            ShouldSee("Alert", "Message", "Ok");
            ShouldNotSee("Popup demo");

            Tap("Ok");
            ShouldSee("Alert result: Ok");
            ShouldSee("Popup demo");
        }

        [Test]
        public void TestYesNoAlertRendering()
        {
            Tap("Show yes/no alert");
            Assert.That(Render(), Is.EqualTo(@"Alert
Message

[Yes] [No]"));
        }

        [Test]
        public void TestOkAlertRendering()
        {
            Tap("Show ok alert");
            Assert.That(Render(), Is.EqualTo(@"Alert
Message

[] [Ok]"));
        }
    }
}
