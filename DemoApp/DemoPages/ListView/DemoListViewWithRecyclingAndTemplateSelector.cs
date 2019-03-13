using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoListViewWithRecyclingAndTemplateSelector : DemoListView
    {
        public DemoListViewWithRecyclingAndTemplateSelector(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemsSource = new List<string> { "A1", "A2", "B1", "B2" };
            ItemTemplate = new Selector();

            BackgroundColor = Color.GhostWhite;

            Label header;
            Header = header = new Label() { Text = "Reverse" };
            header.GestureRecognizers.Add(new TapGestureRecognizer {
                Command = new Command(o => {
                    ItemsSource = new List<string> { "B2", "B1", "A2", "A1" };
                })
            });

            // Reset instance count for each test
            TemplateCell.InstanceCount = 0;
        }

        class Selector : DataTemplateSelector
        {
            DataTemplate templateOne = new DataTemplate(() => { return new TemplateCell("T1"); });
            DataTemplate templateTwo = new DataTemplate(() => { return new TemplateCell("T2"); });

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                var s = item as string;
                if (s.StartsWith("A", StringComparison.InvariantCulture))
                    return templateOne;
                else
                    return templateTwo;
            }
        }

        class TemplateCell : ViewCell
        {
            readonly int instanceNumber;
            string prefix;
            Label label;

            public TemplateCell(string prefix)
            {
                this.prefix = prefix;
                View = label = new Label();
                instanceNumber = ++InstanceCount;
            }

            public static int InstanceCount { get; set; } = 0;

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();
                label.Text = $"I{instanceNumber}:{prefix}:{BindingContext as string}";
            }
        }
    }
}