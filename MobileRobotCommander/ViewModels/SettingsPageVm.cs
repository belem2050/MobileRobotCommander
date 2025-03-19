using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MobileRobotCommander.ViewModels
{
    public partial class SettingsPageVm : ObservableObject
    {
        [ObservableProperty]
        private string cmdVelocityCommandTopic = "/cmd_vel";

        [ObservableProperty]
        private string defaultIpAdress = "192.168.1.33";

        [ObservableProperty]
        private int port = 9090;

        [ObservableProperty]
        private double maxLinearSpeed = 10;

        [ObservableProperty]
        private double maxAngularSpeed = 10;

        [ObservableProperty]
        private string saveButtonText = "Saved ✅"; // Default text

        [ObservableProperty]
        private string saveButtonColor = "Gray"; // Default color


        private bool _hasChanged;
        public bool HasChanged
        {
            get => _hasChanged;
            set
            {
                SetProperty(ref _hasChanged, value);
                SaveCommand.NotifyCanExecuteChanged(); 
                UpdateSaveButton();
            }
        }

        public SettingsPageVm()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            CmdVelocityCommandTopic = Preferences.Get("CmdVelocityCommandTopic", "/cmd_vel");
            DefaultIpAdress = Preferences.Get("DefaultIpAdress", "192.168.1.33");
            Port = Preferences.Get("Port", 9090);
            MaxLinearSpeed = Preferences.Get("MaxLinearSpeed", 10.0);
            MaxAngularSpeed = Preferences.Get("MaxAngularSpeed", 10.0);
            HasChanged = false;
        }

        [RelayCommand(CanExecute = nameof(HasChanged))]
        private void Save()
        {
            Preferences.Set("CmdVelocityCommandTopic", CmdVelocityCommandTopic);
            Preferences.Set("DefaultIpAdress", DefaultIpAdress);
            Preferences.Set("Port", Port);
            Preferences.Set("MaxLinearSpeed", MaxLinearSpeed);
            Preferences.Set("MaxAngularSpeed", MaxAngularSpeed);

            HasChanged = false;
        }

        [RelayCommand]
        public async Task Reset()
        {
            var result = await Application.Current.MainPage.DisplayAlert(
                "Attention",
                "Do you really want to reset settings?",
                "Yes",
                "No");

            if (!result)
            {
                return;
            }
          
            CmdVelocityCommandTopic = "/cmd_vel";
            DefaultIpAdress = "192.168.1.33";
            Port = 9090;
            MaxLinearSpeed = 10;
            MaxAngularSpeed = 10;
            HasChanged = false;
            Save();
        }

        private void UpdateSaveButton()
        {
            SaveButtonText = HasChanged ? "Save Changes ✨" : "Saved ✅";
            SaveButtonColor = HasChanged ? "Green" : "Gray";
        }

        partial void OnCmdVelocityCommandTopicChanged(string value) => HasChanged = true;
        partial void OnDefaultIpAdressChanged(string value) => HasChanged = true;
        partial void OnPortChanged(int value) => HasChanged = true;
        partial void OnMaxLinearSpeedChanged(double value) => HasChanged = true;
        partial void OnMaxAngularSpeedChanged(double value) => HasChanged = true;
    }
}
