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
		public void AppLaunches()
		{
			var app = new App();

			var tester = new AppTester {
				Page = app.MainPage,
			};

			tester.TryRunTest();
		}

		[Test]
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
