using System.Collections.Generic;
using DemoApp;
using NUnit.Framework;
using QuickTest;
using Xamarin.Forms;

namespace Tests
{
    [TestFixture(ListViewCachingStrategy.RetainElement)]
    [TestFixture(ListViewCachingStrategy.RecycleElement)]
    [TestFixture(ListViewCachingStrategy.RecycleElementAndDataTemplate)]
    public class ListViewTests : QuickTest<App>
    {
        ListViewCachingStrategy cachingStrategy;

        public ListViewTests(ListViewCachingStrategy cachingStrategy)
        {
            this.cachingStrategy = cachingStrategy;
        }

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu($"ListViews ({cachingStrategy})");
            ShouldSee("ListView demos");
        }

        protected override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestTextCell()
        {
            Tap("DemoListViewWithTextCell");
            Assert.That(FindFirst("Item A1"), Is.Not.Null);
            TapCell("Item A1");
        }

        [Test]
        public void TestStringViewCell()
        {
            Tap("DemoListViewWithStringViewCell");
            Assert.That(FindFirst("Item A2"), Is.Not.Null);
            TapCell("Item A2");
        }

        [Test]
        public void TestItemViewCell()
        {
            Tap("DemoListViewWithItemViewCell");
            Assert.That(FindFirst("Item A3"), Is.Not.Null);
            TapCell("Item A3");
        }

        [Test]
        public void TestGroups()
        {
            Tap("DemoListViewWithGroups");

            ShouldSee("Group 4");
            TapCell("A4");

            ShouldSee("Group 5");
            TapCell("A5");
        }

        [Test]
        public void TestGroupsWithHeaderTemplate()
        {
            Tap("DemoListViewWithGroupsAndHeaderTemplate");

            ShouldSee("Group 6");
            TapCell("A6");

            ShouldSee("Group 7");
            TapCell("A7");
        }

        [Test]
        public void TestHeadersAndFooters()
        {
            Tap("DemoListViewWithTextCell");
            ShouldSee("plain header");
            ShouldSee("plain footer");
            GoBack();

            Tap("DemoListViewWithStringViewCell");
            ShouldSee("header label");
            ShouldSee("footer label");
        }

        [Test]
        public void TestChangingSource()
        {
            Tap("DemoListViewWithTextCell");
            FindFirst("Item A1").FindParent<ListView>().ItemsSource = new List<string> { "new 1", "new 2" };
            ShouldSee("new 1");
        }

        [Test]
        public void TestTappingGestureRecognizersWithinListViews()
        {
            Tap("DemoListViewWithGestureRecognizers");
            TapCell("Item");
            Tap("tap me");
            Tap("tap me!");
            Tap("tap me!!");
            ShouldSee("tap me!!!");
            TapCell("Item");
        }

        // The cells contain information in the following format:
        // "Item:#N-CtxN-AppearN-DisappN"
        //
        // - Item - The displayed string
        // - #N - Identifies the cell instance
        // - CtxN- Number of OnBindingContextChanged calls
        // - AppearN - Number of OnAppearing calls
        // - DisappN - Number of OnDisappearing calls
        //
        // For each view hierarchy traversal by QuickTest, cells are reused once, which causes
        // an OnDisappearing and an OnAppearing call.
        // If (and only if) a cell is reused for a different object, an OnBindingContextChanged call is executed.
        [Test]
        public void TestCellReuse()
        {
            Tap("DemoListViewForRecycling");

            if (cachingStrategy == ListViewCachingStrategy.RetainElement) {
                // no reuse expected
                ShouldSee("A:#1-Ctx1-Appear0-Disapp0");
                ShouldSee("B:#2-Ctx1-Appear0-Disapp0");
                ShouldSee("C:#3-Ctx1-Appear0-Disapp0");
                Tap("Reload Same");
                ShouldSee("A:#4-Ctx1-Appear0-Disapp0");
                ShouldSee("B:#5-Ctx1-Appear0-Disapp0");
                ShouldSee("C:#6-Ctx1-Appear0-Disapp0");
                Tap("Reload Different");
                ShouldSee("D:#7-Ctx1-Appear0-Disapp0");
                ShouldSee("E:#8-Ctx1-Appear0-Disapp0");
            } else if ((cachingStrategy & ListViewCachingStrategy.RecycleElement) != 0) {
                ShouldSee("A:#1-Ctx1-Appear1-Disapp0");
                ShouldSee("B:#2-Ctx1-Appear2-Disapp1");
                ShouldSee("C:#3-Ctx1-Appear3-Disapp2");
                Tap("Reload Same"); // tapping traverses the hierarchy twice
                ShouldSee("A:#1-Ctx1-Appear6-Disapp5");
                ShouldSee("B:#2-Ctx1-Appear7-Disapp6");
                ShouldSee("C:#3-Ctx1-Appear8-Disapp7");
                Tap("Reload Different");
                ShouldSee("D:#1-Ctx2-Appear11-Disapp10");
                ShouldSee("E:#2-Ctx2-Appear12-Disapp11");
            }
        }

        // The cells contain information in the following format:
        // "Item:#N-Template"
        //
        // - Item - The displayed string
        // - #N - Identifies the cell instance
        // - Template- Identifies the cell template
        [Test]
        public void TestCellReuseWithTemplateSelector()
        {
            Tap("DemoListViewForRecyclingWithTemplateSelector");
            ShouldSee("A1:#1-T1");
            ShouldSee("A2:#2-T1");
            ShouldSee("B1:#3-T2");
            ShouldSee("B2:#4-T2");
            Tap("Reverse");
            if (cachingStrategy == ListViewCachingStrategy.RetainElement) {
                // no reuse expected
                ShouldSee("B2:#5-T2");
                ShouldSee("B1:#6-T2");
                ShouldSee("A2:#7-T1");
                ShouldSee("A1:#8-T1");
            } else if ((cachingStrategy & ListViewCachingStrategy.RecycleElement) != 0) {
                ShouldSee("B2:#3-T2");
                ShouldSee("B1:#4-T2");
                ShouldSee("A2:#1-T1");
                ShouldSee("A1:#2-T1");
            }
        }

        // The cells contain information in the following format:
        // "Item:#N"
        //
        // - Item - The displayed string
        // - #N - Identifies the cell instance
        [Test]
        public void TestCellReuseWithGroups()
        {
            Tap("DemoListViewForRecyclingWithGroups");
            ShouldSee("Group 4");
            ShouldSee("A4:#1");
            ShouldSee("B4:#2");
            ShouldSee("Group 5");
            ShouldSee("A5:#3");
            ShouldSee("B5:#4");
            Tap("Reverse");
            if (cachingStrategy == ListViewCachingStrategy.RetainElement) {
                // no reuse expected
                ShouldSee("Group 5");
                ShouldSee("B5:#5");
                ShouldSee("A5:#6");
                ShouldSee("Group 4");
                ShouldSee("B4:#7");
                ShouldSee("A4:#8");
            } else if ((cachingStrategy & ListViewCachingStrategy.RecycleElement) != 0) {
                ShouldSee("Group 5");
                ShouldSee("B5:#1");
                ShouldSee("A5:#2");
                ShouldSee("Group 4");
                ShouldSee("B4:#3");
                ShouldSee("A4:#4");
            }
        }

        void TapCell(string name)
        {
            Tap(name);
            ShouldSee($"{name} tapped");
            Tap("Ok");
        }
    }
}
