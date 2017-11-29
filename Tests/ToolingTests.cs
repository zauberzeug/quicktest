using System;
using System.Linq;
using DemoApp;
using NUnit.Framework;
using QuickTest;
using Xamarin.Forms;

namespace Tests
{
    public class ToolingTests : IntegrationTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            OpenMenu("Navigation");
        }

        [Test]
        public void TestShouldSee()
        {
            ShouldSee("PushAsync", "PopAsync");
        }

        [Test]
        public void TestTap()
        {
            Tap("PushAsync", "PopAsync");
            ShouldSee("Navigation");
        }

        [Test]
        public void TestTapFailsForNotTappableElements()
        {
            Assert.Throws<InvalidOperationException>(() => Tap("Navigation stack:"));
        }

        [Test]
        public void TestTapFailsWhenTappingTitle()
        {
            Assert.Throws<InvalidOperationException>(() => Tap("Navigation"));
        }

        [Test]
        public void TestInputFailsOnLabel()
        {
            Assert.Throws<InvalidOperationException>(() => Input("Navigation stack:", "some text"));
        }

        [Test]
        public void TestTapNth()
        {
            OpenMenu("Elements");
            Input("Placeholder", "Label");
            TapNth("Label", 0);
        }

        [Test]
        public void TestFind()
        {
            var elements = Find("PushAsync");
            Assert.That(elements, Has.Count.EqualTo(1));
            Assert.That(elements.First(), Is.TypeOf<DemoButton>());
            Assert.That((elements.First() as DemoButton).Text, Is.EqualTo("PushAsync"));
        }

        [Test]
        public void TestFindPredicate()
        {
            OpenMenu("Elements");
            var elements = Find(e => (e as VisualElement)?.IsVisible == false);
            Assert.That(elements, Has.Count.EqualTo(1));
            Assert.That(elements.First(), Is.TypeOf<DemoLabel>());
        }

        [Test]
        public void TestDelay()
        {
            OpenMenu("Elements");
            Tap("Countdown");
            ShouldNotSee("Countdown");
            var time = DateTime.Now;
            After(1).ShouldSee("2");
            After(1.5).Tap("1");
            After(1).ShouldNotSee("1");
            Assert.That(DateTime.Now - time, Is.GreaterThan(TimeSpan.FromSeconds(1)));
        }
    }
}
