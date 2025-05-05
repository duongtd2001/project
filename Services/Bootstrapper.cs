//using Caliburn.Micro;
//using project.ViewModels;
//using System.Windows;
//using System;

//namespace project.Services
//{
//    public class Bootstrapper : BootstrapperBase
//    {
//        private SimpleContainer _container;

//        public Bootstrapper() => Initialize();
//        protected override void OnStartup(object sender, StartupEventArgs e)
//        {
//            DisplayRootViewForAsync<LoginViewModel>();
//        }
//        protected override void Configure()
//        {
//            _container = new SimpleContainer();
//            _container.Instance(_container);

//            _container
//                .Singleton<IWindowManager, WindowManager>()
//                .Singleton<IEventAggregator, EventAggregator>();
//            _container.PerRequest<LoginViewModel>();
//            _container.Singleton<MainViewModel>();
//            /*GetType().Assembly.GetTypes()
//                .Where(type => type.IsClass)
//                .Where(type => type.Name.EndsWith("ViewModel"))
//                .ToList()
//                .ForEach(viewModelType => _container.RegisterPerRequest(
//                    viewModelType, viewModelType.ToString(), viewModelType
//                    ));*/
//        }
//        protected override void OnExit(object sender, EventArgs e)
//        {
//            base.OnExit(sender, e);
//        }
//    }
//}



using Caliburn.Micro;
using project.ViewModels;
using System.Windows;
using System;
using System.Linq;

namespace project.Services
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;
        private readonly IWindowManager _windowManager;
        public Bootstrapper() => Initialize();

        protected override void Configure()
        {
            _container = new SimpleContainer();
            _container.Instance(_container);

            // Đăng ký services
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();
            // Đăng ký ViewModels
             GetType().Assembly.GetTypes()
                 .Where(type => type.IsClass)
                 .Where(type => type.Name.EndsWith("ViewModel"))
                 .ToList()
                 .ForEach(viewModelType => _container.RegisterPerRequest(
                     viewModelType, viewModelType.ToString(), viewModelType
                     ));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewForAsync<LoginViewModel>();
        }

        protected override object GetInstance(Type service, string key)
            => _container.GetInstance(service, key);
    }
}
