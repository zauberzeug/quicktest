using System;
using System.Reflection;
using DemoApp;
using FormsTest;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace NUnitTest
{
	[TestFixture]
	public class Tests
	{
		[SetUp]
		public void Init()
		{
			MockForms.Init();
		}

		[Test]
		public void TestApp()
		{
			var app = new App();

			var tester = new Tester(app);

			tester.Click("Label");
			Assert.That(tester.Contains("Label tapped"));
			tester.GoBack();

			tester.Click("Button");
			Assert.That(tester.Contains("Button tapped"));
			tester.GoBack();

			tester.Click("Nested label");
			Assert.That(tester.Contains("StackLayout tapped"));
			tester.GoBack();

			tester.Click("A");
			Assert.That(tester.Contains("A tapped"));
			tester.GoBack();

			tester.Click("ToolbarItem");
			Assert.That(tester.Contains("ToolbarItem tapped"));
			tester.GoBack();
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
