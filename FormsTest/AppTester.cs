namespace FormsTest
{
	public class AppTester : Tester
	{
		public override void RunTest()
		{
			ShouldSee("Label");
			Click("Button");
			ShouldSee("Ok");
			LogPage();
		}
	}
}
