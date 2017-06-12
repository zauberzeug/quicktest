using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace FormsTest
{
	[TestFixture]
	public abstract class UserTest<T> where T : Application, new()
	{
		User tester;

		[SetUp]
		protected void Init()
		{
			MockForms.Init();

			tester = new User(new T());
		}

		protected void Tap(string text)
		{
			tester.Tap(text);
		}

		protected void ShouldSee(string text)
		{
			Assert.That(tester.Contains(text));
		}

		protected void GoBack()
		{
			tester.GoBack();
		}

		[TearDown]
		public virtual void TearDown()
		{
			if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
				tester.PrintCurrentPage();
		}
	}
}
