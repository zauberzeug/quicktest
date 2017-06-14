using DemoApp;
using NUnit.Framework;
using UserFlow;

namespace Tests
{
	public class NavigationTests : UserTest<App>
	{
		[Test]
		public void TestNavigation()
		{
			Tap("Navigation");
			ShouldSee("Navigation page");

			Tap("PushAsync");
			ShouldSee("Navigation page >");

			Tap("PushAsync");
			ShouldSee("Navigation page > >");

			Tap("PopAsync");
			ShouldSee("Navigation page >");

			Tap("PushModalAsync");
			ShouldSee("Navigation page > ^");

			Tap("PushModalAsync");
			ShouldSee("Navigation page > ^ ^");

			Tap("PopModalAsync");
			ShouldSee("Navigation page > ^");

			Tap("PopModalAsync");
			ShouldSee("Navigation page >");

			Tap("PopToRootAsync");
			ShouldSee("Demo page");
		}

		[Test]
		public void TestDisAppearingPage()
		{
			Tap("Dis-/Appearing");
			ShouldSee("Appeared!");

			GoBack();
			ShouldSee("Disappeared");

			Tap("Ok");
			ShouldSee("Demo page");
		}

		[Test]
		[Ignore("Not working yet")]
		public void TestPopToRoot()
		{
			Tap("Dis-/Appearing");
			App.MainPage.Navigation.PopToRootAsync();
			ShouldSee("Disappeared");

			Tap("Ok");
			ShouldSee("Demo page");
		}
	}
}
