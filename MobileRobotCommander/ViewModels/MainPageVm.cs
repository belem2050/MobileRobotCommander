using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MobileRobotCommander.Models;
using MobileRobotCommander.service;

namespace MobileRobotCommander.ViewModels
{
    public partial class MainPageVm : ObservableObject
    {
        public ActionsCommand Command { get; private set; } = SystemManager.GetInstance().Command;
   
        public SpeechToTextService SpeechToText { get; private set; }  

        public MainPageVm(Frame frame)
        {
            SpeechToText = new SpeechToTextService(frame);
            onFirstAppearing();
        }

        private async void onFirstAppearing()
        {
            await Application.Current.MainPage.DisplayAlert("Welcome!", "Mobile Robot commander is meant to command a mobile robot with Rosbridge webserver running on it. \nYou can set up default settings in Settings flyout.", "OK");
        }

        [RelayCommand]
        public async Task Connect()
        {
            await Command.Connect().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task MicPressed()
        {
            if(!Command.IsConnected)
            {
                if(!await Application.Current.MainPage.DisplayAlert("No Connection Alert!", $"You're not connected to any robot!\nIf it is on purpose, just click on 'On Purpose' to see how it works.\nIf not Go on Connect button", "On Purpose", "Not On Purpose"))
                {
                    return;
                }
            }

            Command.IsHoldingOrListening = true;
            Command.StopButtonColor = Colors.Red;
            await SpeechToText.StartListening();
        }

        [RelayCommand]
        public async Task Forward()
        {
            Command.IsHoldingOrListening = true;
            await Command.Forward().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task ForwardLeft()
        {
            Command.IsHoldingOrListening = true;
            await Command.ForwardLeft().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task ForwardRight()
        {
            Command.IsHoldingOrListening = true;
            await Command.ForwardRight().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task RotateLeft()
        {
            Command.IsHoldingOrListening = true;
            await Command.RotateLeft().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task RotateRight()
        {
            Command.IsHoldingOrListening = true;
            await Command.RotateRight().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task Backward()
        {
            Command.IsHoldingOrListening = true;
            await Command.Backward().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task BackwardLeft()
        {
            Command.IsHoldingOrListening = true;
            await Command.BackwardLeft().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task BackwardRight()
        {
            Command.IsHoldingOrListening = true;
            await Command.BackwardRight().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task Stop()
        {
            Command.IsHoldingOrListening = false;
            await Command.Stop().ConfigureAwait(false);
            await SpeechToText.MicFrame?.ScaleTo(1, 200, Easing.SpringIn);
            SpeechToText.MicFrame.BackgroundColor = Color.FromArgb("#1976D2");
        }

        public async Task Release()
        {
            Command.IsHoldingOrListening = false;
            await Command.Release().ConfigureAwait(false);
        }
    }
}
