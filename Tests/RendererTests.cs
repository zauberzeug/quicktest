using DemoApp;
using NUnit.Framework;
using UserFlow;

namespace Tests
{
	public class RendererTests : UserTest<App>
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
			Assert.That(Render(), Is.EqualTo(@"· Element demo [ToolbarItem]
  · 
    · 
      · Button
      · Label
      · 
        · label in tap-able layout
      · 
        · Cell A
        · Cell B
        · Cell C
        · Cell D
      ·"));
		}
	}
}
