using CommunityToolkit.Mvvm.ComponentModel;

namespace MobileRobotCommander.ViewModels
{
    public partial class AboutPageVm : ObservableObject
    {
        [ObservableProperty]
        string appVersion = AppInfo.VersionString;

        public AboutPageVm() 
        { }
    }
}
