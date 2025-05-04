using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using project.Models;
using project.Repositories;
using project.ViewModels;
using project.Views;

namespace project.Services
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;//  = new SimpleContainer();
        public Bootstrapper()
        {
            _container = new SimpleContainer();
            Initialize();
            //_container = new SimpleContainer();
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewForAsync<LoginViewModel_origin>();
            
        }
        protected override void Configure()
        {
            _container.Instance(_container);

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .PerRequest<IUserRepository, UserRepository>();
            _container.PerRequest<LoginViewModel_origin>();
            _container.Singleton<MainViewModel>();
            /*GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType
                    ));*/
        }
        protected override void OnExit(object sender, EventArgs e)
        {
            base.OnExit(sender, e);
        }
    }
}
