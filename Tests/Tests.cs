using System;
using System.Reflection;
using DemoApp;
using UserFlow;
using NUnit.Framework;
using Xamarin.Forms;

namespace Tests
{
	[TestFixture]
	public class Tests : UserTest<App>
	{
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
			Tap("Nested label");
			ShouldSee("StackLayout tapped");
		}

		[Test]
		public void TestListViews()
		{
			Tap("A1");
			ShouldSee("A1 tapped");

			GoBack();
			ShouldSee("Demo page");

			Tap("A2");
			ShouldSee("A2 tapped");
		}

		[Test]
		public void TestGrid()
		{
			Tap("D");
			ShouldSee("D tapped");
		}

		[Test]
		public void TestToolbarItem()
		{
			Tap("ToolbarItem");
			ShouldSee("ToolbarItem tapped");
		}

		[Test]
		public void TestGoBack()
		{
			Tap("Label");
			GoBack();
			ShouldSee("Demo page");
		}

		[Test]
		public void TestDisAppearingPage()
		{
			Tap("DemoDisAppearing");
			ShouldSee("Appeared!");

			GoBack();
			ShouldSee("Disappeard");
		}

		[Test]
		public void TestAlert()
		{
			Tap("DemoAlert");
			ShouldSee("Message");
			//ShouldNotSee("Demo page"); // TODO

			Tap("Ok");
			ShouldSee("Demo page");
		}

		[Test]
		public void TestModalPage()
		{
			Tap("DemoModalPage");
			ShouldSee("Modal page pushed");

			Tap("Close");
			ShouldSee("Demo page");
		}

		[Test]
		[Ignore("This is not a test")]
		public void ScanForSendMethods()
		{
			var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;
			foreach (var type in typeof(Page).Assembly.GetTypes())
				foreach (var method in type.GetMethods(flags))
					if (method.Name.Contains("Send") || method.Name.Contains("Notify"))
						Console.WriteLine(type.Name + ": " + method.Name);
		}
	}
}
