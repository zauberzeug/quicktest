namespace FormsTest
{
	public class AppTester : Tester
	{
		public override void RunTest()
		{
			LogPage();
			ShouldSee("Label");
			LogPage();
			Click("Button");
			LogPage();
		}
	}
}
