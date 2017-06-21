using DemoApp;
using NUnit.Framework;
using UserFlow;

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
            Assert.That(Render(), Is.EqualTo(@"· Element demo [ToolbarItem]
  · 
    · 
      · search text (searchbar_automation_id)
      · Button
      · Label
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
      · bound text 2
      · Countdown"));
        }
    }
}
