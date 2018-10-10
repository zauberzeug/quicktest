using System;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoSlider : Slider
    {
        public DemoSlider(string automationId, int min, int max, int value)
        {
            AutomationId = automationId;

            Minimum = min;
            Maximum = max;
            Value = value;
            ValueChanged += (s, e) => App.PageLog += $"slider:{(int)e.NewValue} ";
        }
    }
}
