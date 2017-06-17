using System;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace UserFlow
{
    public abstract class IntegrationTest<T> where T : Application, new()
    {
        User user;

        protected T App { get; private set; }

        [SetUp]
        protected virtual void SetUp()
        {
            MockForms.Init();

            App = new T();
            user = new User(App);
        }

        protected void Tap(params string[] texts)
        {
            Now.Tap(texts);
        }

        protected void Input(string automationId, string text)
        {
            user.Input(automationId, text);
        }

        protected void ShouldSee(params string[] texts)
        {
            Now.ShouldSee(texts);
        }

        protected void ShouldNotSee(params string[] texts)
        {
            Now.ShouldNotSee(texts);
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

        protected string Render()
        {
            return user.Render();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            user?.Print();
        }

        protected PatientUser After(double seconds)
        {
            return new PatientUser(user, TimeSpan.FromSeconds(seconds));
        }

        protected PatientUser Now {
            get { return new PatientUser(user, TimeSpan.FromSeconds(0.1)); }
        }
    }
}
