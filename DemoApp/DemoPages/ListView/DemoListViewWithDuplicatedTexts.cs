using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
    // Multiple occurrences of the same text inside of list view headers, footers, cell, and group headers.
    public class DemoListViewWithDuplicatedTexts : DemoListView
    {
        public DemoListViewWithDuplicatedTexts(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemsSource = new List<List<string>> {
                new StringGroup(new [] { "A6", "B6", "C6" }, "Group 6"),
                new StringGroup(new [] { "A7", "B7", "C7" }, "Group 7"),
            };
            Header = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    new Label { Text = "Header" },
                    new Label { Text = "Header" },
                },
            };
            Footer = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    new Label { Text = "Footer" },
                    new Label { Text = "Footer" },
                },
            };
            ItemTemplate = new DataTemplate(() => new DuplicatedLabelCell("."));
            IsGroupingEnabled = true;
            GroupHeaderTemplate = new DataTemplate(() => new DuplicatedLabelCell("Title"));
            BackgroundColor = Color.GhostWhite;

            ItemTapped += (sender, e) => App.ShowMessage("Success", (e.Item as string) + " tapped");
        }

        class DuplicatedLabelCell : ViewCell
        {
            public DuplicatedLabelCell(string bindingPath)
            {
                var label1 = new DemoLabel();
                label1.SetBinding(Label.TextProperty, bindingPath);
                var label2 = new DemoLabel();
                label2.SetBinding(Label.TextProperty, bindingPath);
                View = new StackLayout {
                    Orientation = StackOrientation.Horizontal,
                    Children = {
                        label1,
                        label2,
                    }
                };
            }
        }
    }
}

