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
			Tap("Alert");
			ShouldSee("Message");

			Assert.That(Render(), Is.EqualTo("Alert\nMessage\n\n[] [Ok]"));
		}
	}
}
