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
			ShouldSee("Button tapped");
			GoBack();

			Click("Nested label");
			ShouldSee("StackLayout tapped");
			GoBack();

			Click("ToolbarItem");
			ShouldSee("ToolbarItem tapped");
			GoBack();
		}
	}
}
