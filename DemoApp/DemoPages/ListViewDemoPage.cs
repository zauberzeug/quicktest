using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
    public class ListViewDemoPage : ContentPage
    {
        public ListViewDemoPage()
        {
            Title = "ListView demos";

            Content = new StackLayout {
                Children = {
                    CreateSubpageButton(new DemoListViewWithTextCell(){Header = "plain header", Footer = "plain footer"}),
                    CreateSubpageButton(new DemoListViewWithStringViewCell(){Header = new DemoLabel("header label"), Footer = new DemoLabel("footer label")}),
                    CreateSubpageButton(new DemoListViewWithItemViewCell()),
                    CreateSubpageButton(new DemoListViewWithGroups()),
                    CreateSubpageButton(new DemoListViewWithGroupsAndHeaderTemplate()),
                    CreateSubpageButton(new DemoListViewWithGestureRecognizers()),
                    CreateSubpageButton(new DemoListViewWithRecycling()),
                    CreateSubpageButton(new DemoListViewWithRecyclingAndTemplateSelector()),
                },
            };
        }

        DemoButton CreateSubpageButton(ListView listView)
        {
            return new DemoButton(listView.GetType().Name) {
                Command = new Command(o => Navigation.PushAsync(new NavigationDemoPage(listView.GetType().Name) { Content = listView })),
            };
        }
    }

    public class DemoListView : ListView
    {
        public DemoListView() : base(ConstructionCachingStrategy)
        {
            // ListView ignores caching stragey on platforms where recycling is not supported.
            // ListView will use caching strategy when runtimePlatform is null ("unit test mode"),
            // which can be set when initialising Xamarin.Forms.Mocks.
            if (CachingStrategy != ConstructionCachingStrategy)
                throw new ArgumentException("Caching strategy must not be ignored.");
        }

        public static ListViewCachingStrategy ConstructionCachingStrategy { get; set; }
    }

    public class DemoListViewWithTextCell : DemoListView
    {
        public DemoListViewWithTextCell()
        {
            ItemsSource = new List<string> { "Item A1", "Item B1", "Item C1" };
            ItemTemplate = new DataTemplate(typeof(TextCell));

            BackgroundColor = Color.GhostWhite;

            ItemTemplate.SetBinding(TextCell.TextProperty, ".");
            ItemTapped += (sender, e) => App.ShowMessage("Success", e.Item + " tapped");
        }
    }

    public class DemoListViewWithStringViewCell : DemoListView
    {
        public DemoListViewWithStringViewCell()
        {
            ItemsSource = new List<string> { "Item A2", "Item B2", "Item C2" };
            ItemTemplate = new DataTemplate(typeof(StringDemoCell));

            BackgroundColor = Color.GhostWhite;
            HeightRequest = 200;

            ItemTapped += (sender, e) => App.ShowMessage("Success", e.Item + " tapped");
        }

        class StringDemoCell : ViewCell
        {
            public StringDemoCell()
            {
                var label = new DemoLabel();
                label.SetBinding(Label.TextProperty, ".");
                View = label;
            }
        }
    }

    public class DemoListViewWithItemViewCell : DemoListView
    {
        public DemoListViewWithItemViewCell()
        {
            ItemsSource = new List<Item> { new Item { Name = "Item A3" }, new Item { Name = "Item B3" }, new Item { Name = "Item C3" } };
            ItemTemplate = new DataTemplate(typeof(ItemDemoCell));

            BackgroundColor = Color.GhostWhite;

            ItemTapped += (sender, e) => App.ShowMessage("Success", (e.Item as Item).Name + " tapped");
        }

        class Item : BindableObject
        {
            public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(Item), null);

            public string Name {
                get { return (string)GetValue(NameProperty); }
                set { SetValue(NameProperty, value); }
            }
        }

        class ItemDemoCell : ViewCell
        {
            public ItemDemoCell()
            {
                var label = new DemoLabel();
                label.SetBinding(Label.TextProperty, nameof(Item.Name));
                View = label;
            }
        }
    }

    public class DemoListViewWithGroups : DemoListView
    {
        public DemoListViewWithGroups()
        {
            ItemsSource = new List<List<string>> {
                new StringGroup(new [] { "A4", "B4", "C4" }, "Group 4"),
                new StringGroup(new [] { "A5", "B5", "C5" }, "Group 5"),
            };
            ItemTemplate = new DataTemplate(typeof(StringDemoCell));
            IsGroupingEnabled = true;
            GroupDisplayBinding = new Binding(nameof(StringGroup.Title));

            BackgroundColor = Color.GhostWhite;

            ItemTapped += (sender, e) => App.ShowMessage("Success", (e.Item as string) + " tapped");
        }
    }

    public class DemoListViewWithGroupsAndHeaderTemplate : DemoListView
    {
        public DemoListViewWithGroupsAndHeaderTemplate()
        {
            ItemsSource = new List<List<string>> {
                new StringGroup(new [] { "A6", "B6", "C6" }, "Group 6"),
                new StringGroup(new [] { "A7", "B7", "C7" }, "Group 7"),
            };
            ItemTemplate = new DataTemplate(typeof(StringDemoCell));
            IsGroupingEnabled = true;
            GroupHeaderTemplate = new DataTemplate(typeof(GroupHeaderCell));
            BackgroundColor = Color.GhostWhite;

            ItemTapped += (sender, e) => App.ShowMessage("Success", (e.Item as string) + " tapped");
        }
    }


    public class StringDemoCell : ViewCell
    {
        public StringDemoCell()
        {
            var label = new DemoLabel();
            label.SetBinding(Label.TextProperty, ".");
            View = label;
        }
    }

    public class GroupHeaderCell : ViewCell
    {
        public GroupHeaderCell()
        {
            var label = new DemoLabel();
            label.SetBinding(Label.TextProperty, "Title");
            View = label;
        }
    }

    class StringGroup : List<string>
    {
        public string Title { get; private set; }

        public StringGroup(IEnumerable<string> items, string title)
        {
            Title = title;
            AddRange(items);
        }
    }

    public class DemoListViewWithGestureRecognizers : DemoListView
    {
        public DemoListViewWithGestureRecognizers()
        {
            ItemsSource = new List<Item> { new Item { Name = "Item" } };
            ItemTemplate = new DataTemplate(typeof(ItemGestureCell));

            BackgroundColor = Color.GhostWhite;

            ItemTapped += (sender, e) => App.ShowMessage("ListView Cell", (e.Item as Item).Name + " tapped");
        }

        class Item : BindableObject
        {
            public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(Item), null);

            public string Name {
                get { return (string)GetValue(NameProperty); }
                set { SetValue(NameProperty, value); }
            }
        }

        class ItemGestureCell : ViewCell
        {
            public ItemGestureCell()
            {
                var name = new DemoLabel();
                name.SetBinding(Label.TextProperty, nameof(Item.Name));

                var tappable = new DemoLabel { BackgroundColor = Color.DimGray, Text = "tap me", Margin = new Thickness(10, 0) };
                tappable.GestureRecognizers.Add(new TapGestureRecognizer {
                    Command = new Command(o => {
                        tappable.Text += "!";
                    })
                });

                View = new FlexLayout {
                    Margin = new Thickness(20, 0),
                    Children ={
                        name,
                        tappable,
                        }
                };
            }
        }
    }

    public class DemoListViewWithRecycling : DemoListView
    {
        public DemoListViewWithRecycling()
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
    }

    public class RecyclingCell : ViewCell
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

    public class DemoListViewWithRecyclingAndTemplateSelector : DemoListView
    {
        public DemoListViewWithRecyclingAndTemplateSelector()
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