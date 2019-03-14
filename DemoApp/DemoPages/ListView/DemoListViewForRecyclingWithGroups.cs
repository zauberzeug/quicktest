using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoListViewForRecyclingWithGroups : DemoListView
    {
        public DemoListViewForRecyclingWithGroups(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemsSource = new List<StringGroup> {
                new StringGroup(new [] { "A4", "B4" }, "Group 4"),
                new StringGroup(new [] { "A5", "B5" }, "Group 5"),
            };
            ItemTemplate = new DataTemplate(typeof(GroupRecycleCell));
            IsGroupingEnabled = true;
            GroupDisplayBinding = new Binding(nameof(StringGroup.Title));

            BackgroundColor = Color.GhostWhite;

            Label header;
            Header = header = new Label() { Text = "Reverse" };
            header.GestureRecognizers.Add(new TapGestureRecognizer {
                Command = new Command(o => {
                    ItemsSource = (ItemsSource as IEnumerable<StringGroup>)
                                  .Reverse()
                                  .Select(sg => new StringGroup((sg as IEnumerable<string>).Reverse(), sg.Title))
                                  .ToList();
                })
            });

            // Reset instance count for each test
            GroupRecycleCell.InstanceCount = 0;
        }

        class GroupRecycleCell : ViewCell
        {
            readonly int instanceNumber;
            Label label;

            public GroupRecycleCell()
            {
                View = label = new Label();
                instanceNumber = ++InstanceCount;
            }

            public static int InstanceCount { get; set; } = 0;

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();
                label.Text = $"{BindingContext}:#{instanceNumber}";
            }
        }
    }
}
