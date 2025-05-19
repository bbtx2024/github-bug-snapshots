using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading;
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


        public AutoResetEvent Line1Module1Event = new AutoResetEvent(false);
        public AutoResetEvent Line2Module1Event = new AutoResetEvent(false);
        public AutoResetEvent Line1FlowEvent = new AutoResetEvent(true);
        public AutoResetEvent Line2FlowEvent = new AutoResetEvent(true);
        private static readonly Random rand = new Random(); 
        public object Module1 = new object(); 

        public MainWindowViewModel()
        {
        }

        public void StartTasks()
        {
            var Thread1 = new Thread(Line1Mission);
            var Thread2 = new Thread(Line2Mission);
            var Thread3 = new Thread(Line1);
            var Thread4 = new Thread(Line2);

            Thread1.Start();
            Thread2.Start();
            Thread3.Start();
            Thread4.Start();

            _logService.WriteLog("已启动 4 个线程。");
        }

        private void Line1Mission()
        {
            while (true)
            {
                Line1Module1Event.WaitOne();
                lock (Module1)
                {
                    _logService.WriteLog($"Line1撕膜任务执行中");
                    Thread.Sleep(1000);
                    _logService.WriteLog($"Line1撕膜任务执行中");
                    Thread.Sleep(1000);
                    _logService.WriteLog($"Line1撕膜任务执行中");
                    Thread.Sleep(1000);
                    _logService.WriteLog($"Line1撕膜任务执行中");
                    Thread.Sleep(1000);
                    Line1FlowEvent.Set();
                }

            }
        }

        private void Line2Mission()
        {
            while (true)
            {
                Line2Module1Event.WaitOne();
                lock (Module1)
                {
                    _logService.WriteLog($"Line2撕膜任务执行中");
                    Thread.Sleep(1000);
                    _logService.WriteLog($"Line2撕膜任务执行中");
                    Thread.Sleep(1000);
                    _logService.WriteLog($"Line2撕膜任务执行中");
                    Thread.Sleep(1000);
                    _logService.WriteLog($"Line2撕膜任务执行中");
                    Thread.Sleep(1000);
                    Line2FlowEvent.Set();
                }

            }
        }

        private void Line1()
        {
            while (true)
            {
                Line1FlowEvent.WaitOne();

                int time = rand.Next(1, 6) * 1000; 
                Thread.Sleep(time);

                Line1Module1Event.Set();
            }
        }

        private void Line2()
        {
            while (true)
            {
                Line2FlowEvent.WaitOne();

                int time = rand.Next(1, 11) * 1000;
                Thread.Sleep(time);

                Line2Module1Event.Set();
            }
        }
    }
}
