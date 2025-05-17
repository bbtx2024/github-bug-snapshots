using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using CMTest2.Services;

namespace CMTest2.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : Screen
    {
        [Import]
        private LogService _logService;

        public ObservableCollection<string> Logs => _logService.Logs;


        //[ImportingConstructor]
        //public MainWindowViewModel(MainTask mainTask, LogService logService)
        //{
        //    _mainTask = mainTask;
        //    _logService = logService;
        //}


        public MainWindowViewModel()
        {
        }

        public void LogTest()
        {
            _logService.WriteLog("LogTest");
        }

        //public void StartUp()
        //{


        //}

        private readonly object _lockObj = new object();
        private bool _isRunning = false;

        public async void StartTasks()
        {
            if (_isRunning) return;
            _isRunning = true;

            Task.Run(() => Task1Loop("任务1"));
            Task.Run(() => Task2Loop("任务2"));
        }

        private void Task1Loop(string name)
        {
            while (true)
            {
                lock (_lockObj)
                {
                    _logService.WriteLog($"{DateTime.Now:HH:mm:ss.fff} - {name} 获取锁");

                }
                Task.Delay(1000).Wait();
            }
        }

        private void Task2Loop(string name)
        {
            while (true)
            {
                lock (_lockObj)
                {
                    _logService.WriteLog($"{DateTime.Now:HH:mm:ss.fff} - {name} 获取锁");

                }
                Task.Delay(1000).Wait();
            }
        }
    }
}
