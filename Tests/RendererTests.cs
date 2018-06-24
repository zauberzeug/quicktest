using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class RendererTests : IntegrationTest<App>
    {
        [Test]
        public void TestAlertRendering()
        {
            OpenMenu("Alert");
            ShouldSee("Message");

            Assert.That(Render(), Is.EqualTo(@"Alert
Message

[] [Ok]"));
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
    · 
      · search text (searchbar_automation_id)
      · Button
      · Label
      · first line
        second line
      · 
        · label in tap-able layout
      · 
        · Cell A
        · Cell B
        · Cell C
        · Cell D
      · 
        · label within ContentView
      · some text input (entry_automation_id)
      · editor content (editor_automation_id)
      · Countdown"));
        }

        [Test]
        public void TestListViewRendering()
        {
            OpenMenu("ListViews");
            Assert.That(Render(), Is.EqualTo(@"· ListView demo 
  · 
    · plain header
      - Item A1
      - Item B1
      - Item C1
      plain footer
      
    · · header label
      - · Item A2
      - · Item B2
      - · Item C2
      · footer label
      
    · - · Item A3
      - · Item B3
      - · Item C3
      
    · · Group 4
      - · A4
      - · B4
      - · C4
      · Group 5
      - · A5
      - · B5
      - · C5"));
        }
    }
}
