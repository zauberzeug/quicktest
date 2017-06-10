using DemoApp;
using NUnit.Framework;

namespace NUnitTest
{
	[TestFixture]
	public class Tests
	{
		[SetUp]
		public void Init()
		{
		}

		[Test]
		public void AppLaunches()
		{
			var app = new App();
		}
	}
}
