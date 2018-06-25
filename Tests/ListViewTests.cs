using System.Collections.Generic;
using System.Linq;
using DemoApp;
using NUnit.Framework;
using QuickTest;
using Xamarin.Forms;

namespace Tests
{
    public class ListViewTests : IntegrationTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

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

        void TapCell(string name)
        {
            Tap(name);
            ShouldSee($"{name} tapped");
            Tap("Ok");
        }
    }
}
