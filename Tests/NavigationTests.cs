using DemoApp;
using NUnit.Framework;
using UserFlow;

namespace Tests
{
	public class NavigationTests : UserTest<App>
	{
		[Test]
		public void TestNavigationStack()
		{
			OpenMenu("Navigation");
			ShouldSee("Navigation demo");

			Tap("PushAsync");
			ShouldSee("Navigation demo >");
			ShouldNotSee("Navigation demo", "Menu");

			Tap("PushAsync");
			ShouldSee("Navigation demo > >");

			Tap("PopAsync");
			ShouldSee("Navigation demo >");
		}

		[Test]
		public void TestModalStack()
		{
			OpenMenu("Navigation");
			ShouldSee("Navigation demo");

			Tap("PushModalAsync");
			ShouldSee("Navigation demo ^");
			ShouldNotSee("Navigation demo", "Menu");

			Tap("PushModalAsync");
			ShouldSee("Navigation demo ^ ^");

			Tap("PopModalAsync");
			ShouldSee("Navigation demo ^");
		}

		[Test]
		public void TestPopToRoot()
		{
			OpenMenu("Navigation");
			ShouldSee("Navigation demo");

			Tap("PushAsync");
			Tap("PushAsync");
			Tap("PushAsync");
			ShouldSee("Navigation demo > > >");

			Tap("PopToRootAsync");
			ShouldSee("Navigation demo");
		}

		[Test]
		public void TestPageAppearingOnAppStart()
		{
			Assert.That(App.PageLog, Is.EqualTo(" Appeared"));
		}

		[Test]
		public void TestPageDisAppearingOnPushPop()
		{
			Tap("PushAsync");
			Assert.That(App.PageLog, Is.EqualTo(" Appeared Disappeared Appeared"));

			GoBack();
			Assert.That(App.PageLog, Is.EqualTo(" Appeared Disappeared Appeared Disappeared Appeared"));
		}

		[Test]
		public void TestPageDisAppearingOnMenuChange()
		{
			OpenMenu("Elements");
			OpenMenu("Navigation");
			Assert.That(App.PageLog, Is.EqualTo(" Appeared Disappeared Appeared"));
		}

		[Test]
		[Ignore("Not working yet")]
		public void TestPopToRootEvent()
		{
			Tap("Dis-/Appearing");
			App.MainPage.Navigation.PopToRootAsync();
			ShouldSee("Disappeared");

			Tap("Ok");
			ShouldSee("Demo page");
		}

		[Test]
		public void ToggleMainPageBetweenMasterDetailAndNavigation()
		{
			Tap("Toggle MasterDetail MainPage");
			ShouldSee("Navigation demo");
			Assert.That(App.PageLog, Is.EqualTo(" Appeared"));

			Tap("PushAsync");
			ShouldSee("Navigation demo >");
			//Assert.That(App.PageLog, Is.EqualTo(" Appeared Disappeared Appeared"));

			Tap("PopAsync");
			ShouldSee("Navigation demo");
			//Assert.That(App.PageLog, Is.EqualTo(" Appeared Disappeared Appeared Disappeared Appeared"));

			Tap("Toggle MasterDetail MainPage");
			Assert.That(App.PageLog, Is.EqualTo(" Appeared"));

			OpenMenu("Elements");
			ShouldSee("Element demo");
		}
	}
}
