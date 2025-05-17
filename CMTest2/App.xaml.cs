using System.Windows;

namespace CMTest2
{
    public partial class App : Application
    {
        private readonly AppBootstrapper _bootstrapper;

        public App()
        {
            _bootstrapper = new AppBootstrapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}