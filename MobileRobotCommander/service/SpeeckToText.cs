using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileRobotCommander.Data;
using MobileRobotCommander.Models;

namespace MobileRobotCommander.service
{
    public partial class SpeechToTextService : ObservableObject
    {
        private object _lock = new object();

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
                _ = Task.Run(async () => await processCommand(e.RecognitionResult.Text));
            }

            if (Command.IsListening)
            {
                await Task.Delay(100).ConfigureAwait(true);
                 await StartListening();

                lock(_lock)
                {
                    if(Command.IsListening)
                    {
                        MicFrame.ScaleTo(1, 200, Easing.SpringIn);
                        Task.Delay(250).ConfigureAwait(true);
                        MicFrame.ScaleTo(1.6, 200, Easing.SpringOut);

                    }
                }
            }
            else
            {
                await MicFrame.ScaleTo(1, 200, Easing.SpringIn);
                MicFrame.BackgroundColor = Color.FromArgb("#1976D2");
            }
        }

        private async Task processCommand(string cmd1)
        {
            string cmd = cmd1.ToLower();

            if (Grammar.Forward.Contains(cmd))
            {
                await Command.Forward().ConfigureAwait(true);
            }

            else if (Grammar.TurnLeft.Contains(cmd))
            {
                await Command.ForwardLeft().ConfigureAwait(true);
            }
            else if (Grammar.TurnRight.Contains(cmd))
            {
                await Command.ForwardRight().ConfigureAwait(true);
            }
            else if (Grammar.RotateLeft.Contains(cmd))
            {
                await Command.RotateLeft().ConfigureAwait(true);
            }

            else if (Grammar.RotateRight.Contains(cmd))
            {
                await Command.RotateRight().ConfigureAwait(true);
            }

            else if (Grammar.Backward.Contains(cmd))
            {
                await Command.Backward().ConfigureAwait(true);
            }
            else if (Grammar.BackwardLeft.Contains(cmd))
            {
                await Command.BackwardLeft().ConfigureAwait(true);
            }
            else if (Grammar.BackwardRight.Contains(cmd))
            {
                await Command.BackwardRight().ConfigureAwait(true);
            }
            else if (Grammar.Stop.Contains(cmd))
            {
                await Command.Stop().ConfigureAwait(true);

                lock(_lock)
                {
                    MicFrame?.ScaleTo(1, 200, Easing.SpringIn);
                    MicFrame.BackgroundColor = Color.FromArgb("#1976D2");
                }
            }
        }
    }
}
