using Xamarin.Forms;

namespace DemoApp
{
	public class DemoGrid : Grid
	{
		public DemoGrid()
		{
			RowDefinitions = new RowDefinitionCollection {
				new RowDefinition(),
				new RowDefinition(),
			};
			ColumnDefinitions = new ColumnDefinitionCollection {
				new ColumnDefinition(),
				new ColumnDefinition(),
			};
			Children.Add(new DemoLabel("A").WithGestureRecognizer(), 0, 0);
			Children.Add(new DemoLabel("B").WithGestureRecognizer(), 0, 1);
			Children.Add(new DemoLabel("C").WithGestureRecognizer(), 1, 0);
			Children.Add(new DemoLabel("D").WithGestureRecognizer(), 1, 1);
		}
	}
}
