using System;
using System.Runtime.InteropServices;
using FormsTest.iOS;
using Foundation;
using QuickTestShared;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenshotService))]
namespace FormsTest.iOS
{
    public class ScreenshotService : IScreenshotService
    {
        public byte[] Capture()
        {
            var capture = UIScreen.MainScreen.Capture();
            using (NSData data = capture.AsPNG()) {
                var bytes = new byte[data.Length];
                Marshal.Copy(data.Bytes, bytes, 0, Convert.ToInt32(data.Length));
                return bytes;
            }
        }

        public void Save(string filename, byte[] image)
        {

        }
    }
}
