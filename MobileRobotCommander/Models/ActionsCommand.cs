using CommunityToolkit.Mvvm.ComponentModel;
using MobileRobotCommander.ViewModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace MobileRobotCommander.Models
{
    public partial class ActionsCommand : ObservableObject
    {
        private ClientWebSocket _webSocket;

        private CancellationTokenSource _cst;

        [ObservableProperty]
        public Color stopButtonColor = Colors.Gray;

        [ObservableProperty]
        public Color connectButtonColor = Colors.Gray;

        [ObservableProperty]
        private string ipAdress = "0.0.0.0";

        [ObservableProperty]
        private string connectMessage = "Connect";

        public bool IsHolding { get; set; } = false;
        public bool IsListening { get; set; } = false;
        public bool IsConnected { get; set; } = false;

        public SettingsPageVm Settings { get; set; } = new SettingsPageVm();

        public ActionsCommand()
        {
            IpAdress = Settings.DefaultIpAdress;
            _cst = new CancellationTokenSource();
        }

        public async Task Connect()
        {
            _webSocket = new ClientWebSocket();
            IPAddress iPAddress;
            try
            {
                if (!IPAddress.TryParse(IpAdress.ToString(), out iPAddress))
                {
                    await Application.Current.MainPage.DisplayAlert("Invalid IP", "Please enter a valid IP address.", "OK");
                    return;
                }

                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    ConnectMessage = "Connect";
                    ConnectButtonColor = Colors.Gray;
                    return;
                }

                _webSocket = new ClientWebSocket();

                Uri uri = new Uri($"ws://{IpAdress.ToString()}:{Settings.Port}");

                ConnectMessage = "Connecting";

                await _webSocket.ConnectAsync(uri, new CancellationTokenSource(10000).Token);
                ConnectMessage = (_webSocket.State == WebSocketState.Open) ? "Disconnect" : "Connect";
                IsConnected = true;
                ConnectButtonColor = Colors.Red;

                new Thread(new ThreadStart(async () =>
                {
                    while (IsConnected)
                    {
                        IsConnected = !((_webSocket.State == WebSocketState.Aborted) ||
                                        (_webSocket.State == WebSocketState.Closed) ||
                                        (_webSocket.State == WebSocketState.CloseSent) ||
                                        (_webSocket.State == WebSocketState.CloseReceived)
                                        );
                        ConnectMessage = IsConnected ? "Disconnect" : "Connect";
                        ConnectButtonColor = IsConnected ? Colors.Red : Colors.Gray;

                        Thread.Sleep(1000);
                    }
                })).Start();
            }
            catch (Exception ex)
            {
                ConnectMessage = "Connect";
                await Application.Current.MainPage.DisplayAlert("Connection Failed!", $"Make sure you have the robot right IP address and Rosbridge webserver is running on the robot and is listening to your set port in default settings, which is {Settings.Port}.", "OK");
                IsConnected = false;
                ConnectButtonColor = Colors.Gray;
            }
        }

        public async Task Forward()
        {
            double speed = 0;

            _cst.Cancel();
            await Task.Delay(250);
            _cst = new CancellationTokenSource();

            while (!_cst.Token.IsCancellationRequested &&
                   (IsHolding || IsListening))
            {
                speed = Math.Min(speed + 0.1, Settings.MaxLinearSpeed);
                await SendVelocityCommand(speed, 0.0);
                await Task.Delay(200);
            }
        }

        public async Task ForwardLeft()
        {
            double linear = 0;
            double angular = 0;

            _cst.Cancel();

            await Task.Delay(250);

            _cst = new CancellationTokenSource();

            while (!_cst.Token.IsCancellationRequested &&
                   (IsHolding || IsListening))
            {
                linear = Math.Min(linear + 0.1, Settings.MaxLinearSpeed);
                angular = Math.Min(angular + 0.1, Settings.MaxAngularSpeed);

                await SendVelocityCommand(linear, angular);
                await Task.Delay(200);
            }
        }

        public async Task ForwardRight()
        {
            double linear = 0;
            double angular = 0;

            _cst.Cancel();
            await Task.Delay(250);

            _cst = new CancellationTokenSource();

            while (!_cst.Token.IsCancellationRequested &&
                   (IsHolding || IsListening))
            {
                linear = Math.Min(linear + 0.1, Settings.MaxLinearSpeed);
                angular = Math.Max(angular - 0.1, -Settings.MaxAngularSpeed);
                await SendVelocityCommand(linear, angular);
                await Task.Delay(200);
            }
        }

        public async Task RotateLeft()
        {
            double angular = 0;

            _cst.Cancel();
            await Task.Delay(250);
            _cst = new CancellationTokenSource();

            while (!_cst.Token.IsCancellationRequested &&
                   (IsHolding || IsListening))
            {
                angular = Math.Min(angular + 0.1, Settings.MaxAngularSpeed);
                await SendVelocityCommand(0.0, angular);
                await Task.Delay(200);
            }
        }

        public async Task RotateRight()
        {
            double angular = 0;

            _cst.Cancel();
            await Task.Delay(250);
            _cst = new CancellationTokenSource();

            while (!_cst.Token.IsCancellationRequested &&
                   (IsHolding || IsListening))
            {
                angular = Math.Max(angular - 0.1, -Settings.MaxAngularSpeed);
                await SendVelocityCommand(0.0, angular);
                await Task.Delay(200);
            }
        }

        public async Task Backward()
        {
            double speed = 0;

            _cst.Cancel();
            await Task.Delay(250);

            _cst = new CancellationTokenSource();

            while (!_cst.Token.IsCancellationRequested &&
                   (IsHolding || IsListening))
            {
                speed = Math.Max(speed - 0.1, -Settings.MaxLinearSpeed);
                await SendVelocityCommand(speed, 0.0);
                await Task.Delay(200);
            }
        }

        public async Task BackwardLeft()
        {
            double linear = 0;
            double angular = 0;

            _cst.Cancel();
            await Task.Delay(250);

            _cst = new CancellationTokenSource();

            while (!_cst.Token.IsCancellationRequested &&
                   (IsHolding || IsListening))
            {
                linear = Math.Max(linear - 0.1, -Settings.MaxLinearSpeed);
                angular = Math.Max(angular - 0.1, -Settings.MaxAngularSpeed);

                await SendVelocityCommand(linear, angular);
                await Task.Delay(200);
            }
        }

        public async Task BackwardRight()
        {
            double linear = 0;
            double angular = 0;

            _cst.Cancel();
            await Task.Delay(250);
            _cst = new CancellationTokenSource();

            while (!_cst.Token.IsCancellationRequested &&
                   (IsHolding || IsListening))
            {
                linear = Math.Min(linear + 0.1, Settings.MaxLinearSpeed);
                angular = Math.Min(angular + 0.1, Settings.MaxAngularSpeed);

                await SendVelocityCommand(linear, angular);
                await Task.Delay(200);
            }
        }

        public async Task Stop()
        {
            _cst.Cancel();
            await Task.Delay(250);
            _cst = new CancellationTokenSource();

            StopButtonColor = Colors.Gray;
            await SendVelocityCommand(0.0, 0.0).ConfigureAwait(false);
            IsListening = false;
        }

        public async Task Release()
        {
            StopButtonColor = Colors.Gray;
            IsHolding = false;
            await SendVelocityCommand(0.0, 0.0).ConfigureAwait(false);
        }

        private async Task SendVelocityCommand(double linearX, double angularZ)
        {
            object message = new
            {
                op = "publish",
                topic = Settings.CmdVelocityCommandTopic,
                msg = new
                {
                    linear = new { x = linearX, y = 0.0, z = 0.0 },
                    angular = new { x = 0.0, y = 0.0, z = angularZ }
                }
            };

            string jsonMessage = JsonConvert.SerializeObject(message);
            var bytes = Encoding.UTF8.GetBytes(jsonMessage);
            var buffer = new ArraySegment<byte>(bytes);

            try
            {
                await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch
            { }
        }
    }
}
