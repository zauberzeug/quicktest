using DemoApp;
using NUnit.Framework;
using UserFlow;

namespace Tests
{
	public class ToolingTests : UserTest<App>
	{
		[SetUp]
		public void SetUp()
		{
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
			ShouldSee("Navigation demo");
		}
	}
}
