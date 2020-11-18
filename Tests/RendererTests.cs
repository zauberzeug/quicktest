using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class RendererTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
        }

        [Test]
        public void TestElementRendering()
        {
            OpenMenu("Elements");
            Input("entry_automation_id", "some text input");
            Input("searchbar_automation_id", "search text");
            Tap("Ok");
            Assert.That(Render(), Is.EqualTo(@"· Element demo [ToolbarItem]
  · 
    · (page-stack) 
      · (searchbar_automation_id) search text
      · Button
      · Label
      · first line
        second line
      · (tappable-stack) 
        · label in tap-able layout
      · 
        · Cell A
        · Cell B
        · Cell C
        · Cell D
      · 
        · label within ContentView
      · (entry_automation_id) some text input
      · (editor_automation_id) editor content
      · (slider_automation_id) --o---- 42
      · (picker_automation_id) Pick an item
      · Countdown
      · logo.png"));
        }

        [Test]
        public void TestListViewWithTextCell()
        {
            OpenMenu("ListViews (RetainElement)");

            Tap("DemoListViewWithTextCell");
            Assert.That(Render(), Is.EqualTo(@"· DemoListViewWithTextCell 
  · plain header
    - Item A1
    - Item B1
    - Item C1
    plain footer"));
            GoBack();
        }

        [Test]
        public void TestListViewWithStringViewCell()
        {
            OpenMenu("ListViews (RetainElement)");

            Tap("DemoListViewWithStringViewCell");
            Assert.That(Render(), Is.EqualTo(@"· DemoListViewWithStringViewCell 
  · · header label
    - · Item A2
    - · Item B2
    - · Item C2
    · footer label"));
            GoBack();

        }

        [Test]
        public void TestListViewWithItemViewCell()
        {
            OpenMenu("ListViews (RetainElement)");

            Tap("DemoListViewWithItemViewCell");
            Assert.That(Render(), Is.EqualTo(@"· DemoListViewWithItemViewCell 
  · - · Item A3
    - · Item B3
    - · Item C3"));
            GoBack();

        }

        [Test]
        public void TestListViewWithGroups()
        {
            OpenMenu("ListViews (RetainElement)");

            Tap("DemoListViewWithGroups");
            Assert.That(Render(), Is.EqualTo(@"· DemoListViewWithGroups 
  · · Group 4
    - · A4
    - · B4
    - · C4
    · Group 5
    - · A5
    - · B5
    - · C5"));
            GoBack();

        }

        [Test]
        public void TestListViewWithGroupsAndHeaderTemplate()
        {
            OpenMenu("ListViews (RetainElement)");

            Tap("DemoListViewWithGroupsAndHeaderTemplate");
            Assert.That(Render(), Is.EqualTo(@"· DemoListViewWithGroupsAndHeaderTemplate 
  · · 
      · Group 6
    - · A6
    - · B6
    - · C6
    · 
      · Group 7
    - · A7
    - · B7
    - · C7"));
            GoBack();
        }

        [Test]
        public void TestTitleRendering()
        {
            OpenMenu("Navigation");
            Assert.That(Render(), Does.StartWith("· Navigation \n"));

            Tap("PushAsync");
            Assert.That(Render(), Does.StartWith("· Navigation > \n"));

            Tap("PopAsync");
            Assert.That(Render(), Does.StartWith("· Navigation \n"));

            Tap("PushModalAsync");
            Assert.That(Render(), Does.StartWith("·  \n  · \n    · Title: Navigation ^\n    · Navigation stack:\n"), "modal pages without navigation do not show a title");

            Tap("PushModalAsync NavigationPage");
            Assert.That(Render(), Does.StartWith("· Navigation ^ ^ \n"));
        }

        [Test]
        public void TestTabbedPageRendering()
        {
            OpenMenu("TabbedPage");
            var tabbedPageRendering = "· TabbedPage \n  |> Tab A <| Tab B |\n  · \n    · This is content on tab A\n";
            Assert.That(Render(), Does.StartWith(tabbedPageRendering));
            Tap("Open ModalPage");
            Assert.That(Render(), Is.EqualTo("·  \n  · \n    · This is a modal page\n    · Close"));
            Tap("Close");
            Assert.That(Render(), Does.StartWith(tabbedPageRendering));
        }

        [Test]
        public void TestPickerRendering()
        {
            OpenMenu("Elements");
            var rendering = Render();
            Assert.That(rendering.Contains("· (picker_automation_id) Pick an item"), Is.True);

            Pick("picker_automation_id", "Item B");
            rendering = Render();
            Assert.That(rendering.Contains("· (picker_automation_id) Pick an item"), Is.False);
            Assert.That(rendering.Contains("· (picker_automation_id) Item B"), Is.True);
        }

        [Test]
        public void TestTitleViewRendering()
        {
            OpenMenu("TabbedTitleViewPage");
            Assert.That(Render(), Is.EqualTo(@"· * TitleViewLabel TitleViewButton *
  |> TitleViewPage <| ContentPage |
  · 
    ·"));
        }
    }
}