using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace UserFlow
{
	public abstract class UserTest<T> where T : Application, new()
	{
		User user;

		public T App { get; private set; }

		[SetUp]
		protected void SetUp()
		{
			MockForms.Init();

			App = new T();
			user = new User(App);
		}

		protected void Tap(params string[] texts)
		{
			foreach (var text in texts)
				user.Tap(text);
		}

		protected void Input(string automationId, string text)
		{
			user.Input(automationId, text);
		}

		protected void ShouldSee(params string[] texts)
		{
			foreach (var text in texts)
				Assert.That(user.CanSee(text), $"User can't see \"{text}\"");
		}

		protected void ShouldNotSee(params string[] texts)
		{
			foreach (var text in texts)
				Assert.That(user.CanSee(text), Is.False, $"User can see \"{text}\"");
		}

		protected void OpenMenu(string textToTap = null)
		{
			user.OpenMenu();

			if (textToTap != null)
				Tap(textToTap);
		}

		protected void GoBack()
		{
			user.GoBack();
		}

		public string Render()
		{
			return user.Render();
		}

		[TearDown]
		public virtual void TearDown()
		{
			user?.Print();
		}
	}
}
