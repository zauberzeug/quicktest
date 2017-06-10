using DemoApp;
using FormsTest;
using NUnit.Framework;
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
	}
}
