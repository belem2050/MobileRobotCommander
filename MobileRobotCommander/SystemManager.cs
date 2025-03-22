using CommunityToolkit.Mvvm.ComponentModel;
using MobileRobotCommander.Models;
using MobileRobotCommander.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRobotCommander
{
    public sealed class SystemManager : ObservableObject
    {
        private static object _lockInstance = new object();
        static private SystemManager? _instance = null;

        public ActionsCommand Command { get; private set; } = new ActionsCommand();

        private SystemManager()
        {
            _instance = this;
        }

        static public SystemManager GetInstance()
        {
            lock (_lockInstance)
            {
                if (_instance is null)
                {
                    return _instance = new SystemManager();
                }
                return _instance;
            }
        }
    }
}


