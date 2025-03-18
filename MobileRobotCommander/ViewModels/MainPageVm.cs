using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;

namespace MobileRobotCommander.ViewModels
{
    public partial class MainPageVm : ObservableObject
    {
        private ClientWebSocket _webSocket;
        private bool isHolding = false;

        public SettingsPageVm  Settings { get; private set; } = SystemManager.GetInstance().Settings;

        [ObservableProperty]
        private string ipAdress = string.Empty;

        [ObservableProperty]
        private string connectMessage = "Connect";

        public MainPageVm()
        {
            _webSocket = new ClientWebSocket();
        }

        private async Task SendVelocityCommand(double linearX, double angularZ)
        {
            if (_webSocket.State != WebSocketState.Open)
                return;

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

        [RelayCommand]
        public async Task Connect()
        {
            try
            {
                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "",  CancellationToken.None);
                    ConnectMessage = "Connect";
                    return;
                }

                if (string.IsNullOrEmpty(IpAdress))
                    return;
                _webSocket = new ClientWebSocket();

                Uri uri = new Uri($"ws://{IpAdress}:{Settings.Port}");

                ConnectMessage = "Connecting";

                await _webSocket.ConnectAsync(uri, new CancellationTokenSource(10000).Token);

                ConnectMessage = (_webSocket.State == WebSocketState.Open) ? "Disconnect" : "Connect" ;
            }
            catch (Exception ex)
            {
                ConnectMessage = "Connect";
            }
        }

        [RelayCommand]
        public async Task Mic()
        {
            await Task.Delay(10).ConfigureAwait(false);
        }

        [RelayCommand]
        public async Task Forward()
        {
            isHolding = true;
            double speed = 0;

            while (isHolding)
            {
                speed = Math.Max(speed + 0.1, Settings.MaxLinearSpeed);
                await SendVelocityCommand(speed, 0.0);
                await Task.Delay(200);
            }
        }

        [RelayCommand]
        public async Task ForwardLeft()
        {
            isHolding = true;
            double linear = 0;
            double angular = 0;

            while (isHolding)
            {
                linear = Math.Max(linear + 0.1, Settings.MaxLinearSpeed);
                angular = Math.Max(angular - 0.1, Settings.MinAngularSpeed);

                await SendVelocityCommand(linear, angular);
                await Task.Delay(200);
            }
        }

        [RelayCommand]
        public async Task ForwardRight()
        {
            isHolding = true;
            double linear = 0;
            double angular = 0;

            while (isHolding)
            {
                linear = Math.Max(linear + 0.1, Settings.MaxLinearSpeed);
                angular = Math.Max(angular + 0.1, Settings.MaxAngularSpeed);

                await SendVelocityCommand(linear, angular);
                await Task.Delay(200);
            }
        }

        [RelayCommand]
        public async Task RotateLeft()
        {
            isHolding = true;
            double angular = 0;

            while (isHolding)
            {
                angular = Math.Max(angular - 0.1, Settings.MinAngularSpeed);

                await SendVelocityCommand(0.0, angular);
                await Task.Delay(200);
            }

        }

        [RelayCommand]
        public async Task RotateRight()
        {
            isHolding = true;
            double angular = 0;

            while (isHolding)
            {
                angular = Math.Max(angular + 0.1, Settings.MaxAngularSpeed);

                await SendVelocityCommand(0.0, angular);
                await Task.Delay(200);
            }
        }

        [RelayCommand]
        public async Task Backward()
        {
            isHolding = true;
            double speed = 0;

            while (isHolding)
            {
                speed = Math.Max(speed - 0.1, Settings.MinLinearSpeed);
                await SendVelocityCommand(speed, 0.0);
                await Task.Delay(200);
            }
        }

        [RelayCommand]
        public async Task BackwardLeft()
        {
            isHolding = true;
            double linear = 0;
            double angular = 0;

            while (isHolding)
            {
                linear = Math.Max(linear - 0.1, Settings.MinLinearSpeed);
                angular = Math.Max(angular - 0.1, Settings.MinAngularSpeed);

                await SendVelocityCommand(linear, angular);
                await Task.Delay(200);
            }
        }

        [RelayCommand]
        public async Task BackwardRight()
        {
            isHolding = true;
            double linear = 0;
            double angular = 0;

            while (isHolding)
            {
                linear = Math.Max(linear + 0.1, Settings.MinLinearSpeed);
                angular = Math.Max(angular + 0.1, Settings.MinAngularSpeed);

                await SendVelocityCommand(linear, angular);
                await Task.Delay(200);
            }
        }

        [RelayCommand]
        public async Task Stop()
        {
            await SendVelocityCommand(0.0 , 0.0).ConfigureAwait(false);
        }

        public void Release()
        {
            isHolding = false;
        }
    }
}
