using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using project.Class;
using project.Models;
using project.Repositories;


namespace project.ViewModels
{
    public class LoginViewModel : Screen
    {
        //Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository;

        public string Username 
        {
            get => _username;
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
            }
        }
        public SecureString Password 
        { 
            get => _password; 
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }   
        }
        public string ErrorMessage 
        { 
            get => _errorMessage;
            set 
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }   
        }
        public bool IsViewVisible 
        { 
            get => _isViewVisible;
            set 
            {
                _isViewVisible = value;
                NotifyOfPropertyChange(() => IsViewVisible);
            }   
        }
        //-> Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        public LoginViewModel()
        {
            //userRepository = new UserRepository();
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new RelayCommand(ExecuteRecoverPasswordCommand);
        }

        public bool CanExecuteLoginCommand()
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }

        public void ExecuteLoginCommand()
        {
            //var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
            //if (isValidUser)
            //{
            //    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                IsViewVisible = false;
            //}
            //else
            //{
                //ErrorMessage = "* Invalid username or password";
            //}
        }

        public void ExecuteRecoverPasswordCommand()
        {

        }
    }
}
