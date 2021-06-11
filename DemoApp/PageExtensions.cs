using System;
using Xamarin.Forms;

namespace DemoApp
{
    public static class PageExtensions
    {
        public static T AddPageLog<T>(this T page) where T : Page
        {
            page.Appearing += OnAppearing;
            page.Disappearing += OnDisappearing;
            return page;
        }

        static void OnAppearing(object sender, EventArgs e)
        {
            var page = sender as Page;
            var logMessage = $"A({GetLogName(page)})";
            App.PageLog += $"{logMessage} ";
            Console.WriteLine(logMessage);
        }

        static void OnDisappearing(object sender, EventArgs e)
        {
            var page = sender as Page;
            var logMessage = $"D({GetLogName(page)})";
            App.PageLog += $"{logMessage} ";
            Console.WriteLine(logMessage);
        }

        static string GetLogName(Page page) => page.Title ?? page.GetType().Name;
    }
}
