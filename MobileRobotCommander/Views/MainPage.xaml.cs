
using MobileRobotCommander.service;
using MobileRobotCommander.ViewModels;

namespace MobileRobotCommander.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVm(MicFrame);
        }

        private async void ForwardLeftPressed(object sender, EventArgs e)
        {
            await((MainPageVm)BindingContext).ForwardLeftCommand.ExecuteAsync(null);
        }
        
        private async void ForwardPressed(object sender, EventArgs e)
        {
            await ((MainPageVm)BindingContext).ForwardCommand.ExecuteAsync(null);
        }

        private async void ForwardRightPressed(object sender, EventArgs e)
        {
            await((MainPageVm)BindingContext).ForwardRightCommand.ExecuteAsync(null);
        }    

        private async void RotateLeftPressed(object sender, EventArgs e)
        {
            await((MainPageVm)BindingContext).RotateLeftCommand.ExecuteAsync(null);
        }  

        private async void RotateRightPressed(object sender, EventArgs e)
        {
            await((MainPageVm)BindingContext).RotateRightCommand.ExecuteAsync(null);
        }   

        private async void BackwardLeftPressed(object sender, EventArgs e)
        {
            await((MainPageVm)BindingContext).BackwardLeftCommand.ExecuteAsync(null);
        } 
        private async void BackwardRightPressed(object sender, EventArgs e)
        {
            await((MainPageVm)BindingContext).BackwardRightCommand.ExecuteAsync(null);
        } 
        
        private async void BackwardPressed(object sender, EventArgs e)
        {
            await((MainPageVm)BindingContext).BackwardCommand.ExecuteAsync(null);
        }  

        private async void StopPressed(object sender, EventArgs e)
        {
            await((MainPageVm)BindingContext).StopCommand.ExecuteAsync(null);
        }

        private async void ButtonReleased(object sender, EventArgs e)
        {
            await((MainPageVm)BindingContext).Release();
        }

        
    }
}
