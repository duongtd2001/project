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
using project.Services;
using project.Models;
using project.Repositories;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;
using project.Views;


namespace project.ViewModels
{
    public class LoginViewModel : Screen, IViewAware
    {
        //Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;
        public bool IsAuthenticated { get; set; }

        private UserRepository userRepository;

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

        private object _view;

        public void AttachView(object view, object context = null)
        {
            _view = view;
        }

        public object GetView()
        {
            return _view;
        }

        private readonly IWindowManager _windowManager;
        private readonly MainViewModel _mainViewModel;

        public LoginViewModel(IWindowManager windowManager, MainViewModel mainViewModel)
        {
            _windowManager = windowManager;
            _mainViewModel = mainViewModel;
            userRepository = new UserRepository();
        }
        public event System.Action LoginSuccessful;
        public async Task LoginCommand()
        {
            try
            {
                string password = SecureStringToString(Password);
                var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Username, password));
                if (isValidUser)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                    IsViewVisible = false;
                    IsAuthenticated = true;
                    var mainVM = IoC.Get<MainViewModel>();
                    await _windowManager.ShowWindowAsync(mainVM);
                    bnClose();
                    //await TryCloseAsync();
                }
                else
                {
                    ErrorMessage = "* Invalid username or password";
                }
            }
            finally
            {
                Password?.Dispose();
            }
        }
        public void bnClose()
        {
            if (_view is LoginView window)
            {
                window.CloseWithFade();
            }
        }

        public void bnMinimize()
        {
            if (_view is LoginView window)
            {
                window.MinimizeWithFade();
            }
        }

        private string SecureStringToString(SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        public void ExecuteRecoverPasswordCommand()
        {

        }
    }
    public class AuthenticationMessage
    {
        public bool IsAuthenticated { get; }
        public AuthenticationMessage(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }
    }
}
