using System;
using System.Reflection;
using DemoApp;
using FormsTest;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace NUnitTest
{
	[TestFixture]
	public class Tests
	{
		Tester tester;

		[SetUp]
		public void Init()
		{
			MockForms.Init();

			tester = new Tester(new App());
		}

		[Test]
		public void TestLabel()
		{
			tester.Click("Label");
			Assert.That(tester.Contains("Label tapped"));
		}

		[Test]
		public void TestButton()
		{
			tester.Click("Button");
			Assert.That(tester.Contains("Button tapped"));
		}

		[Test]
		public void TestNestedLabel()
		{
			tester.Click("Nested label");
			Assert.That(tester.Contains("StackLayout tapped"));
		}

		[Test]
		public void TestListView()
		{
			tester.Click("A");
			Assert.That(tester.Contains("A tapped"));
		}

		[Test]
		public void TestToolbarItem()
		{
			tester.Click("ToolbarItem");
			Assert.That(tester.Contains("ToolbarItem tapped"));
		}

		[Test]
		public void TestGoBack()
		{
			tester.Click("Label");
			tester.GoBack();
			Assert.That(tester.Contains("DemoPage"));
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

		[TearDown]
		public virtual void TearDown()
		{
			if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
				tester.PrintCurrentPage();
		}
	}
}
