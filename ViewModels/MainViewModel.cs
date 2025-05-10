using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using project.Models;
using project.Repositories;
using Caliburn.Micro;
using System.Windows.Controls;
using project.Services;
using project.Views;
using System.Xml.Linq;
using System.Drawing;
using System.Windows.Media.Animation;

namespace project.ViewModels
{
     public class MainViewModel : Conductor<object>, IViewAware
    {
        //Fields
        private string _currentUserAccount;
        private UserControl _currentChildView;
        private string _caption;
        private IconChar _icon;

        private UserRepository userRepository;
        // properties
        public string CurrentUserAccount { get => _currentUserAccount; set { _currentUserAccount = value; NotifyOfPropertyChange(() => CurrentUserAccount); } }
       
        public UserControl CurrentChildView { get => _currentChildView; set { _currentChildView = value; NotifyOfPropertyChange(()=>  CurrentChildView); } }    
       
        public string Caption { get => _caption; set { _caption = value; NotifyOfPropertyChange(()=> Caption); } } 

        public IconChar Icon { get => _icon; set { _icon = value; NotifyOfPropertyChange(()=> Icon); } }

        private object _view;

        public void AttachView(object view, object context = null)
        {
            _view = view;
        }

        public object GetView()
        {
            return _view;
        }

        private AutoViewModel autoViewModel;
        private CustomerViewModel customerViewModel;
        private HomeViewModel homeViewModel;
        private ManualViewModel manualViewModel;
        private ReportViewModel reportViewModel;
        private SettingViewModel settingViewModel;

        public MainViewModel()
        {
            autoViewModel = new AutoViewModel();
            customerViewModel = new CustomerViewModel();
            homeViewModel = new HomeViewModel();
            manualViewModel = new ManualViewModel();
            reportViewModel = new ReportViewModel();
            settingViewModel = new SettingViewModel();

            userRepository = new UserRepository();
            LoadCurrentUserData();
            CurrentChildView = new AutoView();
        }

        public void ShowHomeViewCommand()
        {
            ActivateItemAsync(homeViewModel);
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }
        public void ShowCustomerViewCommand()
        {
            ActivateItemAsync(customerViewModel);
            Caption = "Customer";
            Icon = IconChar.UserGroup;
        }
        public void ShowReportCommand()
        {
            ActivateItemAsync(reportViewModel);
            Caption = "Report";
            Icon = IconChar.PieChart;
        }
        public void ShowSettingCommand()
        {
            ActivateItemAsync(settingViewModel);
            Caption = "Setting";
            Icon = IconChar.Tools;
        }
        public void ShowAutoCommand()
        {
            ActivateItemAsync(autoViewModel);
            Caption = "Auto";
            Icon = IconChar.Play;
        }
        public void ShowManualCommand()
        {
            ActivateItemAsync(manualViewModel);
            Caption = "Manual";
            Icon = IconChar.Hand;
        }
        public void bnClose()
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                if (_view is MainView window)
                {
                    window.CloseWithFade();
                }
            }
            else
                return;
            
        }
        private void LoadCurrentUserData()
        {
            if (UserSession.CurrentUser != null)
            {
                CurrentUserAccount = UserSession.CurrentUser;
            }
            else
            {
                CurrentUserAccount = "Invalid user, not logger in";
            }
        }
    }
}
