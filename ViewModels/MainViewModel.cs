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
using project.Class;
using project.Views;

namespace project.ViewModels
{
     public class MainViewModel : Conductor<object>
     {
        //Fields
        private UserAccountModel _currentUserAccount;
        private UserControl _currentChildView;
        private string _caption;
        private IconChar _icon;


        private IUserRepository userRepository;
        // properties
        public UserAccountModel CurrentUserAccount { get => _currentUserAccount; set { _currentUserAccount = value; NotifyOfPropertyChange(() => CurrentUserAccount); } }
       
        public UserControl CurrentChildView { get => _currentChildView; set { _currentChildView = value; NotifyOfPropertyChange(()=>  CurrentChildView); } }    
       
        public string Caption { get => _caption; set { _caption = value; NotifyOfPropertyChange(()=> Caption); } } 

        public IconChar Icon { get => _icon; set { _icon = value; NotifyOfPropertyChange(()=> Icon); } }

        // Commands
        public ICommand ShowHomeViewCommand {  get; }
        public ICommand ShowCustomerViewCommand {  get; }

        public MainViewModel(UserRepository _user)
        {
            userRepository = _user;
            CurrentUserAccount = new UserAccountModel();
            ShowHomeViewCommand = new RelayCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new RelayCommand(ExecuteShowCustomerViewCommand);

            LoadCurrentUserData();
            CurrentChildView = new HomeView();
        }

        private void ExecuteShowCustomerViewCommand()
        {
            CurrentChildView = new CustomerView();
            Caption = "Customers";
            Icon = IconChar.UserGroup;
        }

        private void ExecuteShowHomeViewCommand()
        {
            CurrentChildView = new HomeView();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        private void LoadCurrentUserData()
        {
            // SQL

            //var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            //if(user != null)
            //{
            //    CurrentUserAccount.Username = user.Name;
            //    CurrentUserAccount.DisplayName = $"{user.Name} {user.Name}";
            //    CurrentUserAccount.ProfilePicture = null;
            //}
            //else
            //{
            //    CurrentUserAccount.DisplayName = "Invalid user, not logger in";
            //}

            // Excel
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user.Name != null)
            {
                CurrentUserAccount.Username = user.Name; 
                CurrentUserAccount.DisplayName = user.Name;
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";
            }
        }
     }
}
