# Quicktest for MAUI

This repository contains Quicktest for Xamarin.Forms. Quicktest for .NET MAUI can be found in [a separate repository](https://github.com/zauberzeug/quicktest-maui).

# Xamarin.Quicktest

Quicktest provides infrastructure to write acceptance and integration tests with NUnit for Xamarin.Forms apps. With a little care (mocking HTTP, etc) you can achieve a very fast executing set of tests to ensure high level requirements are working as expected.

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

We created this project for internal purposes but wanted to share it with the community as soon as possible. There is no documentation right now, but we think that looking at our tests provided with the source code you should be able to figure things out.

# NuGet

The library can be installed via nuget.org (https://www.nuget.org/packages/Xamarin.Forms.QuickTest).
