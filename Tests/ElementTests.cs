using DemoApp;
using NUnit.Framework;
using UserFlow;

namespace Tests
{
    public class ElementTests : IntegrationTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            OpenMenu("Elements");
        }

        [Test]
        public void TestLabel()
        {
            Tap("Label");
            ShouldSee("Label tapped");
        }

        [Test]
        public void TestButton()
        {
            Tap("Button");
            ShouldSee("Button tapped");
        }

        [Test]
        public void TestNestedLabel()
        {
            Tap("label in tap-able layout");
            ShouldSee("StackLayout tapped");
        }

        [Test]
        public void TestListViews()
        {
            OpenMenu("ListViews");
            ShouldSee("ListView demo");

            Tap("Item A1");
            ShouldSee("Item A1 tapped");

            Tap("Ok");
            ShouldSee("ListView demo");

            Tap("Item A2");
            ShouldSee("Item A2 tapped");
        }

        [Test]
        public void TestGrid()
        {
            Tap("Cell D");
            ShouldSee("Cell D tapped");
        }

        [Test]
        public void TestInvisible()
        {
            ShouldNotSee("Invisible Label");
        }

        [Test]
        public void TestBinding()
        {
            ShouldSee("bound text 2");
        }

        [Test]
        public void TestEntry()
        {
            ShouldSee("Placeholder", "entry_automation_id");

            Input("Placeholder", "Text1");
            ShouldSee("Text1");
            ShouldNotSee("Placeholder");

            Input("entry_automation_id", "Text2");
            ShouldSee("Text2");

            Input("Text2", "Text3");
            ShouldSee("Text3");
        }

        [Test]
        public void TestEditor()
        {
            ShouldSee("editor_automation_id");

            Input("editor_automation_id", "Text1");
            ShouldSee("Text1");

            Input("Text1", "Text2");
            ShouldSee("Text2");
        }

        [Test]
        public void TestToolbarItem()
        {
            Tap("ToolbarItem");
            ShouldSee("ToolbarItem tapped");
        }

        [Test]
        public void TestAlert()
        {
            OpenMenu("Alert");
            ShouldSee("Alert", "Message", "Ok");
            ShouldNotSee("Demo page");

            Tap("Ok");
            ShouldSee("Menu");
        }
    }
}
