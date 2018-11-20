using System.Collections;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoPicker : Picker
    {
        public DemoPicker(string autmomationId, string title, IList itemSource)
        {
            AutomationId = autmomationId;

            Title = title;
            ItemsSource = itemSource;

            SelectedIndexChanged += (sender, e) => App.PageLog += $"picker: {SelectedItem.ToString()}";
        }
    }
}
