using System;
using System.Reflection;
using DemoApp;
using FormsTest;
using NUnit.Framework;
using Xamarin.Forms;

namespace NUnitTest
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
		public void TestListView()
		{
			Tap("A");
			ShouldSee("A tapped");
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
			ShouldSee("DemoPage");
		}

		[Test]
		public void TestPrintCurrentPage()
		{
			PrintCurrentPage();
		}

		[Test]
		[Ignore("This is not a test")]
		public void ScanForSendMethods()
		{
			var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
			var mscorlib = typeof(Page).Assembly;
			foreach (var type in mscorlib.GetTypes())
				foreach (var method in type.GetMethods(flags))
					if (method.Name.StartsWith("Send", StringComparison.Ordinal))
						Console.WriteLine(type.Name + "." + method.Name);
		}
	}
}
