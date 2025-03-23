namespace MobileRobotCommander.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

    private async void OnLabelTapped(object sender, EventArgs e)
    {
        string url = ((Label)sender).Text;

        if(url.Contains("@"))
        {
            url = $"mailto:{url}";
        }
        try
        {
            await Launcher.OpenAsync(url);
        }catch
        {
        }
    }
}