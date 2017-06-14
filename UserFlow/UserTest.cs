using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace UserFlow
{
	[TestFixture]
	public abstract class UserTest<T> where T : Application, new()
	{
		User user;

		[SetUp]
		protected void Init()
		{
			MockForms.Init();

			user = new User(new T());
		}

		protected void Tap(string text)
		{
			user.Tap(text);
		}

		protected void ShouldSee(string text)
		{
			Assert.That(user.CanSee(text), $"User can't see \"{text}\"");
		}

		protected void ShouldNotSee(string text)
		{
			Assert.That(user.CanSee(text), Is.False, $"User can see \"{text}\"");
		}

		protected void GoBack()
		{
			user.GoBack();
		}

		[TearDown]
		public virtual void TearDown()
		{
			//user?.PrintCurrentPage();
		}
	}
}
