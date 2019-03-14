using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoListViewForRecyclingWithTemplateSelector : DemoListView
    {
        public DemoListViewForRecyclingWithTemplateSelector(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemsSource = new List<object> { new A(1), new A(2), new B(1), new B(2) };
            ItemTemplate = new Selector();

            BackgroundColor = Color.GhostWhite;

            Label header;
            Header = header = new Label() { Text = "Reverse" };
            header.GestureRecognizers.Add(new TapGestureRecognizer {
                Command = new Command(o => {
                    ItemsSource = (ItemsSource as IEnumerable<object>).Reverse().ToList();
                })
            });

            // Reset instance count for each test
            TemplateCell.InstanceCount = 0;
        }

        class A
        {
            public A(int suffix) { Text = $"{nameof(A)}{suffix}"; }

            public string Text { get; private set; }

            public override string ToString() { return Text; }
        }

        class B
        {
            public B(int suffix) { Text = $"{nameof(B)}{suffix}"; }

            public string Text { get; private set; }

            public override string ToString() { return Text; }
        }

        class Selector : DataTemplateSelector
        {
            DataTemplate templateOne = new DataTemplate(typeof(CellForA));
            DataTemplate templateTwo = new DataTemplate(typeof(CellForB));

            protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            {
                if (item is A)
                    return templateOne;
                else
                    return templateTwo;
            }
        }

        class CellForA : TemplateCell
        {
            public CellForA() : base("T1") { }
        }

        class CellForB : TemplateCell
        {
            public CellForB() : base("T2") { }
        }

        class TemplateCell : ViewCell
        {
            readonly int instanceNumber;
            string templateName;
            Label label;

            public TemplateCell(string templateName)
            {
                this.templateName = templateName;
                View = label = new Label();
                instanceNumber = ++InstanceCount;
            }

            public static int InstanceCount { get; set; } = 0;

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();
                label.Text = $"{BindingContext}:#{instanceNumber}-{templateName}";
            }
        }
    }
}