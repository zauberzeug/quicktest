using System;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoEntry : Entry
    {
        public DemoEntry(string automationId, string placeholder = null, string text = null)
        {
            AutomationId = automationId;
            Placeholder = placeholder;
            Text = text;
            Completed += OnCompleted;
            Unfocused += OnUnfocused;
        }

        void OnCompleted(object sender, EventArgs args)
        {
            Text += "<completed>";
        }

        void OnUnfocused(object sender, EventArgs args)
        {
            Text += "<unfocused>";
        }
    }
}
