namespace FormsTest
{
	public class AppTester : Tester
	{
		protected override void RunTest()
		{
			Click("Label");
			ShouldSee("Label tapped");
			GoBack();

			Click("Button");
			ShouldSee("Button clicked");
			GoBack();

			Click("Nested label");
			ShouldSee("StackLayout tapped");
			GoBack();
		}
	}
}
