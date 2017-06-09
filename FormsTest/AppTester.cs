namespace FormsTest
{
	public class AppTester : Tester
	{
		protected override void RunTest()
		{
			Click("Label");
			ShouldSee("Label tapped");

			Click("Button");
			ShouldSee("Button clicked");

			LogPage();
		}
	}
}
