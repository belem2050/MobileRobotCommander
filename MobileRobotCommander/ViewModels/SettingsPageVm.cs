using CommunityToolkit.Mvvm.ComponentModel;

namespace MobileRobotCommander.ViewModels
{



    public partial class SettingsPageVm : ObservableObject
    {
        [ObservableProperty]
        private string cmdVelocityCommandTopic = "/cmd_vel";

        [ObservableProperty]
        private int port = 9090;
        [ObservableProperty]
        private double maxLinearSpeed = 1.0;

        [ObservableProperty]
        private double minLinearSpeed = -1.0;

        [ObservableProperty]
        private double maxAngularSpeed = 1.0;

        [ObservableProperty]
        private double minAngularSpeed = -1.0;


        public SettingsPageVm() 
        { } 
    }
}
