using System.Collections.Generic;
using DemoApp;
using NUnit.Framework;
using QuickTest;
using Xamarin.Forms;

namespace Tests
{
    [TestFixture(ListViewCachingStrategy.RetainElement)]
    [TestFixture(ListViewCachingStrategy.RecycleElement)]
    public class ListViewTests : QuickTest<App>
    {
        ListViewCachingStrategy cachingStrategy;

        public ListViewTests(ListViewCachingStrategy cachingStrategy)
        {
            DemoListView.ConstructionCachingStrategy = this.cachingStrategy = cachingStrategy;
        }

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
            OpenMenu("ListViews");
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

        [Test]
        public void TestRecycling()
        {
            Tap("DemoListViewWithRecycling");

            if (cachingStrategy == ListViewCachingStrategy.RetainElement) {
                ShouldSee("Instance1:Item1-OBC1-OA0-OD0");
                ShouldSee("Instance2:Item2-OBC1-OA0-OD0");
                ShouldSee("Instance3:Item3-OBC1-OA0-OD0");
                Tap("Reload Same");
                ShouldSee("Instance4:Item1-OBC1-OA0-OD0");
                ShouldSee("Instance5:Item2-OBC1-OA0-OD0");
                ShouldSee("Instance6:Item3-OBC1-OA0-OD0");
                Tap("Reload Different");
                ShouldSee("Instance7:Item4-OBC1-OA0-OD0");
                ShouldSee("Instance8:Item5-OBC1-OA0-OD0");
            } else if ((cachingStrategy & ListViewCachingStrategy.RecycleElement) != 0) {
                ShouldSee("Instance1:Item1-OBC1-OA0-OD0");
                ShouldSee("Instance2:Item2-OBC1-OA1-OD1");
                ShouldSee("Instance3:Item3-OBC1-OA2-OD2");
                Tap("Reload Same"); // tapping traverses the hierarchy twice
                ShouldSee("Instance1:Item1-OBC1-OA5-OD5");
                ShouldSee("Instance2:Item2-OBC1-OA6-OD6");
                ShouldSee("Instance3:Item3-OBC1-OA7-OD7");
                Tap("Reload Different");
                ShouldSee("Instance1:Item4-OBC2-OA10-OD10");
                ShouldSee("Instance2:Item5-OBC2-OA11-OD11");
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
