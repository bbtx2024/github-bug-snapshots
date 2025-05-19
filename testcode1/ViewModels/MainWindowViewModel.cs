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

        public AutoResetEvent BufferWaitingEvent = new AutoResetEvent(false);



        public MainWindowViewModel()
        {
        }

        public void StartTasks()
        {
            var Thread1 = new Thread(BufferMission);
            var Thread2 = new Thread(Line1Mission);

            Thread1.Start();
            Thread2.Start();

            _logService.WriteLog("已启动 2 个线程。");
        }

        private void BufferMission()
        {
            while(true)
            {
                Thread.Sleep(2000);
                _logService.WriteLog("缓存位等待");
                BufferWaitingEvent.WaitOne();
                _logService.WriteLog("缓存位出料完成");
            }
        }

        private void Line1Mission()
        {

            Thread.Sleep(200);
            _logService.WriteLog("流线出料完成");
            BufferWaitingEvent.Set();

        }
    }
}
