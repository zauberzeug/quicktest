﻿using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoListViewWithRecycling : DemoListView
    {
        public DemoListViewWithRecycling(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemsSource = new List<string> { "Item1", "Item2", "Item3" };
            ItemTemplate = new DataTemplate(typeof(RecyclingCell));
            BackgroundColor = Color.GhostWhite;

            Label header;
            Header = header = new Label() { Text = "Reload Same" };
            header.GestureRecognizers.Add(new TapGestureRecognizer {
                Command = new Command(o => {
                    ItemsSource = new List<string> { "Item1", "Item2", "Item3" };
                })
            });

            Label footer;
            Footer = footer = new Label() { Text = "Reload Different" };
            footer.GestureRecognizers.Add(new TapGestureRecognizer {
                Command = new Command(o => {
                    ItemsSource = new List<string> { "Item4", "Item5" };
                })
            });

            // Reset instance count for each test
            RecyclingCell.InstanceCount = 0;
        }

        class RecyclingCell : ViewCell
        {
            DemoLabel label;
            readonly int instanceNumber;
            int bindingContextChangeCount = 0;
            int appearingCount = 0;
            int disappearingCount = 0;

            public RecyclingCell()
            {
                instanceNumber = ++InstanceCount;
                label = new DemoLabel();
                View = label;
            }

            public static int InstanceCount { get; set; } = 0;

            void UpdateText()
            {
                label.Text = $"Instance{instanceNumber}:{BindingContext as string}-OBC{bindingContextChangeCount}-OA{appearingCount}-OD{disappearingCount}";
            }

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();
                bindingContextChangeCount++;
                UpdateText();
            }

            protected override void OnAppearing()
            {
                base.OnAppearing();
                appearingCount++;
                UpdateText();
            }

            protected override void OnDisappearing()
            {
                base.OnDisappearing();
                disappearingCount++;
                UpdateText();
            }
        }
    }
}