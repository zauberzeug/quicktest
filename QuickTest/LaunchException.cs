using System;
namespace QuickTest
{
    public class LaunchException : Exception
    {
        public LaunchException() : base("App has not been launched. Call 'Launch(new App())' before using Quicktest")
        {
        }
    }
}
