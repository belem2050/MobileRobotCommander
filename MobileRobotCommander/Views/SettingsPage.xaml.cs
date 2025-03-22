using MobileRobotCommander.ViewModels;

namespace MobileRobotCommander.Views;

public partial class SettingsPage : ContentPage
{

    public SettingsPage()
	{
		InitializeComponent();
        BindingContext = SystemManager.GetInstance().Command.Settings;
    }
}