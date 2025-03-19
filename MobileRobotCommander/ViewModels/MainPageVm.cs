using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using System.Net;

namespace MobileRobotCommander.ViewModels
{
    public partial class MainPageVm : ObservableObject
    {
        private ClientWebSocket _webSocket;
        private bool isHolding = false;

        public SettingsPageVm  Settings { get; private set; } = SystemManager.GetInstance().Settings;

        [ObservableProperty]
        private string ipAdress ="192.168.1.33";

        [ObservableProperty]
        private string connectMessage = "Connect";

        public MainPageVm()
        {
            _webSocket = new ClientWebSocket();
            IpAdress = Settings.DefaultIpAdress;
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
            IPAddress iPAddress;

            try
            {
                if(!IPAddress.TryParse(IpAdress.ToString(), out iPAddress))
                {
                    await Application.Current.MainPage.DisplayAlert("Invalid IP", "Please enter a valid IP address.", "OK");
                    return;
                }

                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "",  CancellationToken.None);
                    ConnectMessage = "Connect";
                    return;
                }
          
                _webSocket = new ClientWebSocket();

                Uri uri = new Uri($"ws://{IpAdress.ToString()}:{Settings.Port}");

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
                angular = Math.Max(angular + 0.1, Settings.MaxAngularSpeed);

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
                angular = Math.Min(angular - 0.1, -Settings.MaxLinearSpeed);
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
                angular = Math.Max(angular + 0.1, Settings.MaxAngularSpeed);
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
                angular = Math.Min(angular - 0.1, -Settings.MaxAngularSpeed);
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
                speed = Math.Min(speed - 0.1, -Settings.MaxLinearSpeed);
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
                linear = Math.Min(linear - 0.1, -Settings.MaxLinearSpeed);
                angular = Math.Min(angular - 0.1, -Settings.MaxAngularSpeed);

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
                linear = Math.Max(linear + 0.1, Settings.MaxLinearSpeed);
                angular = Math.Max(angular + 0.1, Settings.MaxAngularSpeed);

                await SendVelocityCommand(linear, angular);
                await Task.Delay(200);
            }
        }

        [RelayCommand]
        public async Task Stop()
        {
            await SendVelocityCommand(0.0 , 0.0).ConfigureAwait(false);
        }

        public async Task Release()
        {
            isHolding = false;
            await SendVelocityCommand(0.0, 0.0).ConfigureAwait(false);
        }
    }
}
