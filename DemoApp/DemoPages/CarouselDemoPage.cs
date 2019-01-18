using Xamarin.Forms;

namespace DemoApp
{
    public class CarouselDemoPage : CarouselPage
    {
        public CarouselDemoPage()
        {
            Title = "CarouselPage";

            Children.Add(new ContentPage() {
                Content = new StackLayout {
                    Children = {
                                new Label{ Text = "Label on a carouselpage" }
                            }
                }
            });
        }
    }
}
