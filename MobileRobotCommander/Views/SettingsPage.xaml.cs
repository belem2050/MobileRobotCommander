using MobileRobotCommander.ViewModels;

namespace MobileRobotCommander.Views;

public partial class SettingsPage : ContentPage
{

	public SettingsPage(SettingsPageVm vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}