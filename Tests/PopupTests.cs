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
        public void TestYesNoAlertRendering()
        {
            Tap("Show yes/no alert");
            Assert.That(Render(), Is.EqualTo(@"Alert
Message

[Yes] [No]"));
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
        public void TestOkAlertRendering()
        {
            Tap("Show ok alert");
            Assert.That(Render(), Is.EqualTo(@"Alert
Message

[Ok]"));
        }

        [TestCase("Cancel")]
        [TestCase("Destroy")]
        [TestCase("Option 1")]
        [TestCase("Option 2")]
        public void TestActionSheet(string buttonToTap)
        {
            Tap("Show action sheet");
            ShouldSee("Action sheet");
            ShouldSee(buttonToTap);
            ShouldNotSee("Popup demo");

            Tap(buttonToTap);
            ShouldSee($"Action sheet result: {buttonToTap}");
            ShouldSee("Popup demo");
        }

        [Test]
        public void TestActionSheetRendering()
        {
            Tap("Show action sheet");
            Assert.That(Render(), Is.EqualTo(@"Action sheet
[Destroy]
[Option 1]
[Option 2]
[Cancel]"));
        }

        [Test]
        public void TestActionSheetWithoutCancel()
        {
            Tap("Show action sheet without cancel");
            ShouldSee("Action sheet without cancel", "Destroy", "Option 1");
            Tap("Option 1");
        }

        [Test]
        public void TestActionSheetWithoutCancelRendering()
        {
            Tap("Show action sheet without cancel");
            Assert.That(Render(), Is.EqualTo(@"Action sheet without cancel
[Destroy]
[Option 1]"));
        }

        [Test]
        public void TestActionSheetWithoutDestruction()
        {
            Tap("Show action sheet without destruction");
            ShouldSee("Action sheet without destruction", "Cancel");
            Tap("Cancel");
        }

        [Test]
        public void TestActionSheetWithoutDestructionRendering()
        {
            Tap("Show action sheet without destruction");
            Assert.That(Render(), Is.EqualTo(@"Action sheet without destruction
[Cancel]"));
        }

        [Test]
        public void TestSeesAlert()
        {
            Assert.That(SeesAlert(), Is.False);
            Assert.That(SeesActionSheet(), Is.False);
            Tap("Show ok alert");
            Assert.That(SeesAlert(), Is.True);
            Assert.That(SeesActionSheet(), Is.False);
        }

        [Test]
        public void TestSeesActionSheet()
        {
            Assert.That(SeesActionSheet(), Is.False);
            Assert.That(SeesAlert(), Is.False);
            Tap("Show action sheet");
            Assert.That(SeesActionSheet(), Is.True);
            Assert.That(SeesAlert(), Is.False);
        }

        [Test]
        public void CanCountInAlerts()
        {
            Tap("Show ok alert with repeated text");
            Assert.Throws<AssertionException>(() => ShouldSee("Message", 2));
            ShouldSee("Message", 3);
            Assert.Throws<AssertionException>(() => ShouldSee("Message", 4));
        }

        [Test]
        public void CanCountInActionSheets()
        {
            Tap("Show action sheet with repeated text");
            Assert.Throws<AssertionException>(() => ShouldSee("Message", 4));
            ShouldSee("Message", 5);
            Assert.Throws<AssertionException>(() => ShouldSee("Message", 6));
        }

        [Test]
        public void AlertHidesPage()
        {
            ShouldSee("Some text");
            Tap("Show ok alert");
            Assert.Throws<AssertionException>(() => ShouldSee("Some text"));
            Assert.Throws<AssertionException>(() => ShouldSeeOnce("Some text"));
            Assert.Throws<AssertionException>(() => ShouldSee("Some text", 1));
        }

        [Test]
        public void ActionSheetHidesPage()
        {
            ShouldSee("Some text");
            Tap("Show action sheet");
            Assert.Throws<AssertionException>(() => ShouldSee("Some text"));
            Assert.Throws<AssertionException>(() => ShouldSeeOnce("Some text"));
            Assert.Throws<AssertionException>(() => ShouldSee("Some text", 1));
        }
    }
}
