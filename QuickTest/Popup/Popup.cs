namespace QuickTest
{
    public abstract class Popup
    {
        public abstract bool Contains(string text);

        public abstract bool Tap(string text);

        public abstract string Render();
    }
}
