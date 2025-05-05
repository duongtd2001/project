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

namespace project.ViewModels
{
     public class MainViewModel : Conductor<object>, IViewAware
    {
        //Fields
        private UserAccountModel _currentUserAccount;
        private UserControl _currentChildView;
        private string _caption;
        private IconChar _icon;

        private UserRepository userRepository;
        // properties
        public UserAccountModel CurrentUserAccount { get => _currentUserAccount; set { _currentUserAccount = value; NotifyOfPropertyChange(() => CurrentUserAccount); } }
       
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

        public MainViewModel()
        {
           // ActivateItemAsync(new CustomerViewModel());
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
            CurrentChildView = new HomeView();

        }

        public void ShowHomeViewCommand()
        {
            if (_view is MainView window)
            {
                CurrentChildView = new HomeView();
                Caption = "Dashboard";
                Icon = IconChar.Home;
            }
        }
        public void ShowCustomerViewCommand()
        {

            CurrentChildView = new CustomerView();
            Caption = "Dashboard";
            Icon = IconChar.UserGroup;
        }

        public void bnClose()
        {
            if (_view is MainView window)
            {
                window.CloseWithFade();
            }
        }
        public void bnMinimize()
        {
            if (_view is MainView window)
            {
                window.MinimizeWithFade();
            }
        }
        private void LoadCurrentUserData()
        {
            // Excel
            var excelReader = new ReadExcelData();
            var user = excelReader.FindProductByID(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Name;
                CurrentUserAccount.DisplayName = user.Name;
                CurrentUserAccount.ProfilePicture = @"C:\Users\duong\Documents\isme.jpg";
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logger in";
            }
        }
     }
}
