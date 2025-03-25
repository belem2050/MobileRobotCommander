using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using MobileRobotCommander.Data;
using MobileRobotCommander.Models;

namespace MobileRobotCommander.service
{
    public partial class SpeechToTextService : ObservableObject
    {
        [ObservableProperty]
        private string message = string.Empty;

        private SpeechToTextOptions _options = new SpeechToTextOptions
        {
            Culture = System.Globalization.CultureInfo.CurrentCulture,
            ShouldReportPartialResults = false,
        };

        public Frame? MicFrame { get; set; } = null;

        public ActionsCommand Command { get; private set; } = SystemManager.GetInstance().Command;

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
                _ = Task.Run(async () => await processCommand(e.RecognitionResult.Text));
            }

            await Task.Delay(1000).ConfigureAwait(true);

            if (Command.IsHoldingOrListening)
            {
                 await StartListening();

                if(Command.IsHoldingOrListening)
                {
                    await MicFrame.ScaleTo(1, 200, Easing.SpringIn);
                    await Task.Delay(250).ConfigureAwait(true);
                    await MicFrame.ScaleTo(1.6, 200, Easing.SpringOut);

                }
            }
            else
            {
                Message = string.Empty;
                await MicFrame.ScaleTo(1, 200, Easing.SpringIn);
                MicFrame.BackgroundColor = Color.FromArgb("#1976D2");
            }
        }

        private async Task processCommand(string cmd1)
        {
            string cmd = cmd1.ToLower();

            if (Grammar.Forward.Contains(cmd) 
                || cmd.Contains("forward"))
            {
                await Command.Forward().ConfigureAwait(true);
            }
            else if (Grammar.TurnLeft.Contains(cmd)
                || cmd.Contains("turn left"))
            {
                await Command.ForwardLeft().ConfigureAwait(true);
            }
            else if (Grammar.TurnRight.Contains(cmd)
                || cmd.Contains("turn right"))
            {
                await Command.ForwardRight().ConfigureAwait(true);
            }
            else if (Grammar.RotateLeft.Contains(cmd)
                || cmd.Contains("rotate left"))
            {
                await Command.RotateLeft().ConfigureAwait(true);
            }

            else if (Grammar.RotateRight.Contains(cmd)
                || cmd.Contains("rotate right"))
            {
                await Command.RotateRight().ConfigureAwait(true);
            }

            else if (Grammar.Backward.Contains(cmd)
                || cmd.Contains("backwrd"))
            {
                await Command.Backward().ConfigureAwait(true);
            }
            else if (Grammar.BackwardLeft.Contains(cmd)
                || cmd.Contains("backward left"))
            {
                await Command.BackwardLeft().ConfigureAwait(true);
            }
            else if (Grammar.BackwardRight.Contains(cmd)
                || cmd.Contains("backward right"))
            {
                await Command.BackwardRight().ConfigureAwait(true);
            }
            else if (Grammar.Stop.Contains(cmd)
                || cmd.Contains("stop"))
            {
                await Command.Stop().ConfigureAwait(true);

                await MicFrame?.ScaleTo(1, 200, Easing.SpringIn);
                MicFrame.BackgroundColor = Color.FromArgb("#1976D2");
                Message = string.Empty;

            }
        }
    }
}
