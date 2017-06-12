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
			Click("Label");
			ShouldSee("Label tapped");
		}

		[Test]
		public void TestButton()
		{
			Click("Button");
			ShouldSee("Button tapped");
		}

		[Test]
		public void TestNestedLabel()
		{
			Click("Nested label");
			ShouldSee("StackLayout tapped");
		}

		[Test]
		public void TestListView()
		{
			Click("A");
			ShouldSee("A tapped");
		}

		[Test]
		public void TestToolbarItem()
		{
			Click("ToolbarItem");
			ShouldSee("ToolbarItem tapped");
		}

		[Test]
		public void TestGoBack()
		{
			Click("Label");
			GoBack();
			ShouldSee("DemoPage");
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
