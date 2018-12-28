using System;

namespace QuickTestShared
{
    public interface IScreenshotService
    {
        byte[] Capture();

        void Save(string filename, byte[] image);
    }
}
