using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Caliburn.Micro;
using System.Windows;

namespace CMTest2.Services
{
    [Export(typeof(LogService))]
    //[Export]
    public class LogService
    {

        private readonly Dispatcher _dispatcher;
        public ObservableCollection<string> Logs { get; private set; }

        public LogService()
        {
            Logs = new ObservableCollection<string>();
            _dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
        }

        public void WriteLog(string message)
        {
            _dispatcher.Invoke(() =>
            {
                Logs.Add($"[{DateTime.Now:HH:mm:ss:fff}] {message}");
            });
        }
    }
}
