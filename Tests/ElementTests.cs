using DemoApp;
using NUnit.Framework;
using UserFlow;

namespace Tests
{
	public class ElementTests : UserTest<App>
	{
		[SetUp]
		public void SetUp()
		{
			OpenMenu("Elements");
		}

		[Test]
		public void TestLabel()
		{
			Tap("Label");
			ShouldSee("Label tapped");
		}

		[Test]
		public void TestButton()
		{
			Tap("Button");
			ShouldSee("Button tapped");
		}

		[Test]
		public void TestNestedLabel()
		{
			Tap("label in tap-able layout");
			ShouldSee("StackLayout tapped");
		}

		[Test]
		public void TestListViews()
		{
			OpenMenu("ListViews");
			ShouldSee("ListView demo");

			Tap("Item A1");
			ShouldSee("Item A1 tapped");

			Tap("Ok");
			ShouldSee("ListView demo");

			Tap("Item A2");
			ShouldSee("Item A2 tapped");
		}

		[Test]
		public void TestGrid()
		{
			Tap("Cell D");
			ShouldSee("Cell D tapped");
		}

		[Test]
		public void TestToolbarItem()
		{
			Tap("ToolbarItem");
			ShouldSee("ToolbarItem tapped");
		}

		[Test]
		public void TestAlert()
		{
			OpenMenu("Alert");
			ShouldSee("Message");
			ShouldNotSee("Demo page");

			Tap("Ok");
			ShouldSee("Menu");
		}
	}
}
