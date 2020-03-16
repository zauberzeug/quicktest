using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class MasterDetailPagetests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();
            Launch(new App());
        }

        [Test]
        public void ToggleMenu()
        {
            ShouldSee("Navigation stack:");
            ShouldNotSee("Menu");

            OpenMenu();

            ShouldNotSee("Navigation stack:");
            ShouldSee("Menu");

            CloseMenu();

            ShouldSee("Navigation stack:");
            ShouldNotSee("Menu");
        }

        [Test]
        public void OpenMenuEntry()
        {
            ShouldSee("Navigation stack:");

            OpenMenu("Binding");
            ShouldNotSee("Navigation stack:");
            ShouldSee("Binding demo");

            OpenMenu("Navigation");
            ShouldNotSee("Binding demo");
            ShouldSee("Navigation stack:");
        }
    }
}