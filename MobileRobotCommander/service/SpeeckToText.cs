using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileRobotCommander.Data;
using MobileRobotCommander.Models;

namespace MobileRobotCommander.service
{
    public partial class SpeechToTextService : ObservableObject
    {
        public Frame? MicFrame { get; set; } = null;

        public ActionsCommand Command { get; private set; } = SystemManager.GetInstance().Command;

        private SpeechToTextOptions _options = new SpeechToTextOptions
        {
            Culture = System.Globalization.CultureInfo.CurrentCulture,
            ShouldReportPartialResults = false,
        };

        public string Message { get; set; } = string.Empty;

        public SpeechToTextService(Frame micFrame)
        {
            MicFrame = micFrame;
            SpeechToText.Default.RecognitionResultCompleted += Default_RecognitionResultCompleted;
        }

        public async Task StartListening()
        {
            MicFrame.BackgroundColor = Colors.Green;
            await MicFrame.ScaleTo(1.6, 200, Easing.SpringOut);

            try
            {
                var status = await Permissions.RequestAsync<Permissions.Microphone>();
                if (status != PermissionStatus.Granted)
                {
                    return;
                }
                await SpeechToText.StartListenAsync(_options);
            }
            catch (Exception ex)
            {
            }
        }

        private async void Default_RecognitionResultCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs e)
        {
            if (e.RecognitionResult.IsSuccessful)
            {
                Message = e.RecognitionResult.Text;
                await processCommand();
            }

            if(Command.IsListening)
            {
                await Task.Delay(100).ConfigureAwait(true);
                await StartListening();

                await MicFrame.ScaleTo(1, 200, Easing.SpringIn);
                await Task.Delay(250).ConfigureAwait(true);
                await MicFrame.ScaleTo(1.6, 200, Easing.SpringOut);
            }
            else
            {
                await MicFrame.ScaleTo(1, 200, Easing.SpringIn);
                MicFrame.BackgroundColor = Color.FromArgb("#1976D2");
            }
        }

        private async Task processCommand()
        {
            string command = Message.ToLower();

            if (Grammar.Forward.Contains(command))
            {
                await Command.Forward().ConfigureAwait(false);
            }

            else if (Grammar.TurnLeft.Contains(command))
            {
                await Command.ForwardLeft().ConfigureAwait(false);
            }
            else if (Grammar.TurnRight.Contains(command))
            {
                await Command.ForwardRight().ConfigureAwait(false);
            }
            else if (Grammar.RotateLeft.Contains(command))
            {
                await Command.RotateLeft().ConfigureAwait(false);
            }

            else if (Grammar.RotateRight.Contains(command))
            {
                await Command.RotateRight().ConfigureAwait(false);
            }

            else if (Grammar.Backward.Contains(command))
            {
                await Command.Backward().ConfigureAwait(false);
            }
            else if (Grammar.BackwardLeft.Contains(command))
            {
                await Command.BackwardLeft().ConfigureAwait(false);
            }
            else if (Grammar.BackwardRight.Contains(command))
            {
                await Command.BackwardRight().ConfigureAwait(false);
            }
            else if (Grammar.Stop.Contains(command))
            {
                Command.IsListening = false;
                await Command.Stop().ConfigureAwait(false);
            }
        }
    }
}
