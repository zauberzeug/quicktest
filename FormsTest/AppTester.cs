namespace FormsTest
{
	public class AppTester : Tester
	{
		public override void RunTest()
		{
			Click("Label");
			ShouldSee("Label tapped");

			Click("Button");
			ShouldSee("Button clicked");

			LogPage();
		}
	}
}
