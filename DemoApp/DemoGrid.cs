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
			Children.Add(new Label { Text = "A" }, 0, 0);
			Children.Add(new Label { Text = "B" }, 0, 1);
			Children.Add(new Label { Text = "C" }, 1, 0);
			Children.Add(new Button { Text = "D", Command = new Command(o => App.PushMessagePage("D tapped")) }, 1, 1);
		}
	}
}
