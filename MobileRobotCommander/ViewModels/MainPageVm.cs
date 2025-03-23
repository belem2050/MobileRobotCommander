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
   
        private SpeechToTextService _speechToText;

        public MainPageVm(Frame frame)
        {
            _speechToText = new SpeechToTextService(frame);
            onFirstAppearing();
        }

        private async void onFirstAppearing()
        {
            await Application.Current.MainPage.DisplayAlert("Welcome!", "Mobile Robot commander is meant to command a mobile with Rosbridge webserver running on it. You can set up default settings in Settings flyout.", "OK");
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

            Command.IsListening = true;
            Command.StopButtonColor = Colors.Red;
            await _speechToText.StartListening();
        }

        [RelayCommand]
        public async Task Forward()
        {
            Command.IsHolding = true;
            await Command.Forward().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task ForwardLeft()
        {
            Command.IsHolding = true;
            await Command.ForwardLeft().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task ForwardRight()
        {
            Command.IsHolding = true;
            await Command.ForwardRight().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task RotateLeft()
        {
            Command.IsHolding = true;
            await Command.RotateLeft().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task RotateRight()
        {
            Command.IsHolding = true;
            await Command.RotateRight().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task Backward()
        {
            Command.IsHolding = true;
            await Command.Backward().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task BackwardLeft()
        {
            Command.IsHolding = true;
            await Command.BackwardLeft().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task BackwardRight()
        {
            Command.IsHolding = true;
            await Command.BackwardRight().ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task Stop()
        {
            Command.IsHolding = false;
            Command.IsListening = false;
            await Command.Stop().ConfigureAwait(false);
            await _speechToText.MicFrame?.ScaleTo(1, 200, Easing.SpringIn);
            _speechToText.MicFrame.BackgroundColor = Color.FromArgb("#1976D2");
        }

        public async Task Release()
        {
            Command.IsHolding = false;
            await Command.Release().ConfigureAwait(false);
        }
    }
}
