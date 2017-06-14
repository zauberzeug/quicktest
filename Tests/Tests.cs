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
			Tap("ListViews");

			Tap("Item A1");
			ShouldSee("Item A1 tapped");

			Tap("Ok");
			ShouldSee("ListViews");

			Tap("Item A2");
			ShouldSee("Item A2 tapped");
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
		public void TestDisAppearingPage()
		{
			Tap("Dis-/Appearing");
			ShouldSee("Appeared!");

			GoBack();
			ShouldSee("Disappeared");

			Tap("Ok");
			ShouldSee("Demo page");

			Tap("Dis-/Appearing");
			App.MainPage.Navigation.PopToRootAsync();
			ShouldSee("Disappeared");

			Tap("Ok");
			ShouldSee("Demo page");
		}

		[Test]
		public void TestAlert()
		{
			Tap("Alert");
			ShouldSee("Message");
			ShouldNotSee("Demo page");

			Tap("Ok");
			ShouldSee("Demo page");
		}

		[Test]
		public void TestNavigation()
		{
			Tap("Navigation");
			ShouldSee("Navigation page");

			Tap("PushAsync");
			ShouldSee("Navigation page >");

			Tap("PushAsync");
			ShouldSee("Navigation page > >");

			Tap("PopAsync");
			ShouldSee("Navigation page >");

			Tap("PushModalAsync");
			ShouldSee("Navigation page > ^");

			Tap("PushModalAsync");
			ShouldSee("Navigation page > ^ ^");

			Tap("PopModalAsync");
			ShouldSee("Navigation page > ^");

			Tap("PopModalAsync");
			ShouldSee("Navigation page >");

			Tap("PopToRootAsync");
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
