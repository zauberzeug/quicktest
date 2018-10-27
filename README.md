# Xamarin.Quicktest

Quicktest provides infrastructure to write acceptance and integration tests with NUnit for Xamarin.Forms Apps. With a little care (mocking HTTP, etc) you can archive a very fast executing set of tests to ensure high level requirements are working as expected.

```csharp

public class ToolingTests : QuickTest<App>
{
    [SetUp]
    protected override void SetUp()
    {
        base.SetUp();

        Launch(new App());
    }

    [Test]
    public void LoginShouldGreetUser()
    {
        Input("Username", "test user");
        Input("Password", "mysecret");
        Tap("Login");
        ShouldSee("Welcome test user");
    }
}
```

We created this project for internal purposes but wanted to share it with the commuity as soon as possible. There is no documentation right now, but we think that looking at our tests provided with the source code you should be able to figure things out.
