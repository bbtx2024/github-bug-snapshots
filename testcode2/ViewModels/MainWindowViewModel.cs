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

        public AutoResetEvent Module1WorkingEvent = new AutoResetEvent(false);
        public AutoResetEvent Module1WaitingEvent = new AutoResetEvent(false);

        private static readonly Random rand = new Random();

        public MainWindowViewModel() { }

        public void StartTasks()
        {
            var moduleThread = new Thread(ModuleTask);
            var rollThread = new Thread(RollTask);

            moduleThread.Start();
            rollThread.Start();

            _logService.WriteLog("已启动 2 个线程（模组+卷放卷收）。");
        }

        // 模拟撕膜模组流程
        private void ModuleTask()
        {
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(rand.Next(1000, 3000)); // 模拟撕膜完成 + 移动

                _logService.WriteLog($"[模组] 穴位{i} 撕膜完成，准备移动");
                Module1WorkingEvent.Set(); // 通知卷放卷收开始

                Thread.Sleep(rand.Next(1000, 2000)); // 模拟移动时间

                _logService.WriteLog($"[模组] 到位准备下压穴位{i}，等待卷放卷收完成");
                Module1WaitingEvent.WaitOne(); // 等待卷放卷收完成

                _logService.WriteLog($"[模组] 穴位{i} 下压完成");
            }
        }

        // 模拟卷放卷收流程
        private void RollTask()
        {
            while (true)
            {
                Module1WorkingEvent.WaitOne(); // 等模组通知开始卷放卷收
                _logService.WriteLog("[卷放卷收] 接收到开始信号，开始作业");

                Thread.Sleep(rand.Next(1500, 3000)); // 模拟动作执行

                _logService.WriteLog("[卷放卷收] 完成，通知模组可以下压");
                Module1WaitingEvent.Set(); // 通知模组继续
            }
        }
    }
}