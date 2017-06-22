using System;
using Xamarin.Forms.Internals;
namespace QuickTest
{
    public static class AlertArgumentExtensions
    {
        public static string Render(this AlertArguments alert)
        {
            return $"{alert.Title}\n{alert.Message}\n\n[{alert.Accept}] [{alert.Cancel}]";
        }
    }
}
