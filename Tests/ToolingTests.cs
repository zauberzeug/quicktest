using System;
using System.Linq;
using DemoApp;
using NUnit.Framework;
using QuickTest;
using Xamarin.Forms;

namespace Tests
{
    public class ToolingTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("Navigation");
        }

        [Test]
        public void TestShouldSee()
        {
            ShouldSee("PushAsync", "PopAsync");
        }

        [Test]
        public void TestShouldSeeOnce()
        {
            OpenMenu("Elements");
            ShouldSeeOnce("editor content");
            Input("entry_automation_id", "editor content");
            ShouldSee("editor content");
            ShouldFail(() => ShouldSeeOnce("editor content"));
        }

        [Test]
        public void TestShouldSeeWithCount()
        {
            OpenMenu("Elements");
            ShouldSee("editor content", 1);
            Input("entry_automation_id", "editor content");
            ShouldFail(() => ShouldSee("editor content", 1));
            ShouldSee("editor content", 2);
            ShouldFail(() => ShouldSee("editor content", 3));
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
        public void TestTapElementWithIdenticalTitle()
        {
            OpenMenu("Elements");

            // set itentical title for button and page title
            (Find("Button").First() as DemoButton).Text = "Element demo";
            ShouldSee("Element demo", 2);

            var elements = Find("Element demo");
            Assert.That(elements.First(), Is.TypeOf<ElementDemoPage>());
            Assert.That(elements.Last(), Is.TypeOf<DemoButton>());

            TapNth("Element demo", 1);
            Tap("Ok");
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
        public void FindWithNullObjectFails()
        {
            Element element = null;
            Assert.That(() => element.Find("PushAsync"), Throws.Exception);
        }

        [Test]
        public void FindIsEmptyWhenNothingIsFound()
        {
            var elements = Find("some text which is not on the screen");
            Assert.That(elements, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestFindFirst()
        {
            var element = FindFirst("PushAsync");
            Assert.That(element, Is.Not.Null);
            Assert.That(element, Is.TypeOf<DemoButton>());
            Assert.That((element as DemoButton).Text, Is.EqualTo("PushAsync"));
        }

        [Test]
        public void FindFirstIsNullWhenNothingIsFound()
        {
            var element = FindFirst("some text which is not on the screen");
            Assert.That(element, Is.Null);
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
        public void TestFindFirstPredicate()
        {
            OpenMenu("Elements");
            var element = FindFirst(e => (e as VisualElement)?.IsVisible == false);
            Assert.That(element, Is.Not.Null);
            Assert.That(element, Is.TypeOf<DemoLabel>());
        }

        [TestCase("ListViews (RetainElement)")]
        [TestCase("ListViews (RecycleElement)")]
        [TestCase("ListViews (RecycleElementAndDataTemplate)")]
        public void TestFindParentInListViews(string menuEntry)
        {
            OpenMenu(menuEntry);
            Tap("DemoListViewWithGroups");
            var label = FindFirst("B4");
            Assert.That(label, Is.Not.Null);
            Assert.That(label.FindParent<ListView>(), Is.Not.Null);
            var cell = label.FindParent<StringDemoCell>();

            Assert.That(cell, Is.Not.Null);
            Assert.That(cell.View, Is.TypeOf(typeof(DemoLabel)));

            Assert.That(FindFirst("B5").FindParent<BoxView>(), Is.Null);
        }

        [Test]
        public void TestFindParentWithNestedStackLayouts()
        {
            OpenMenu("Elements");
            var tappableStack = FindFirst("label in tap-able layout").FindParent<StackLayout>();
            Assert.That(tappableStack.AutomationId, Is.EqualTo("tappable-stack"));
            var pageStack = tappableStack.FindParent<StackLayout>();
            Assert.That(pageStack.AutomationId, Is.EqualTo("page-stack"));
        }

        [Test]
        public void TestDelay()
        {
            OpenMenu("Elements");
            Tap("Countdown");
            ShouldNotSee("Countdown");
            var time = DateTime.Now;
            After(2).ShouldSee("2");
            After(2).Tap("1");
            After(2).ShouldNotSee("1");
            Assert.That(DateTime.Now - time, Is.GreaterThan(TimeSpan.FromSeconds(1)));
        }

        void ShouldFail(TestDelegate code) => Assert.Throws<AssertionException>(code);
    }
}
