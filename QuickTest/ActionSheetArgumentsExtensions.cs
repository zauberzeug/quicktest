using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace QuickTest
{
    public static class ActionSheetArgumentsExtensions
    {
        public static string Render(this ActionSheetArguments actionSheet)
        {
            var parts = new List<string> { actionSheet.Title };
            if (actionSheet.Destruction != null)
                parts.Add($"[{actionSheet.Destruction}]");
            foreach (var button in actionSheet.Buttons)
                parts.Add($"[{button}]");
            if (actionSheet.Cancel != null)
                parts.Add($"[{actionSheet.Cancel}]");
            return string.Join("\n", parts);
        }
    }
}
