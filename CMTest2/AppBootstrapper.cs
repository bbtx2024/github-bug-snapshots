using Caliburn.Micro;
using System.Windows;
using CMTest2.ViewModels;
using CMTest2.Services;
using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Linq;

namespace CMTest2
{
    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer _compositionContainer;
        private IWindowManager _windowManager;  //新增

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            MEFConfigure();
            base.Configure();
        }

        private void MEFConfigure()
        {
            // 创建组合目录
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            _compositionContainer = new CompositionContainer(catalog);

            var batch = new CompositionBatch();


            // 注册WindowManager到MEF（手动new一个）
            _windowManager = new WindowManager();
            batch.AddExportedValue<IWindowManager>(_windowManager);

            // 导出Bootstrapper本身
            batch.AddExportedValue(this);

            _compositionContainer.Compose(batch);

        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;

            var exports = _compositionContainer.GetExportedValues<object>(contract);

            if (exports != null)
            {
                var enumerator = exports.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    var instance = enumerator.Current;

                    // 关键：补齐Import
                    _compositionContainer.SatisfyImportsOnce(instance);

                    return instance;
                }
            }

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);

            DisplayRootViewFor<MainWindowViewModel>();
        }
    }
}