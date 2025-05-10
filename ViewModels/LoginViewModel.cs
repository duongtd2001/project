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
using System.Configuration;


namespace project.ViewModels
{
    public class LoginViewModel : Screen, IViewAware
    {
        //Fields
        private string _username;
        private string _po;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;
        public bool IsAuthenticated { get; set; }

        private UserRepository userRepository;
        public string PO 
        { 
            get => _po; 
            set 
            { 
                _po = value; 
                NotifyOfPropertyChange(() => PO);
                if (!string.IsNullOrWhiteSpace(_po) && _po.Length == 12)
                {
                    LoginCommand();
                }
            } 
        }
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
        private ReadExcelData _readExcelData;

        public LoginViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            _readExcelData = new ReadExcelData();
            userRepository = new UserRepository();
        }
        
        public void LoginCommand()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_po) && _po.Length == 12)
                {
                    UserModel userModel = _readExcelData.FindProductByID(Username);
                    if (userModel != null)
                    {
                        UserSession.CurrentUser = userModel.Name;
                        UserSession.CurrentPO = PO;
                        IsViewVisible = false;
                        IsAuthenticated = true;
                        var mainVM = IoC.Get<MainViewModel>();
                        _windowManager.ShowWindowAsync(mainVM);
                        bnClose();
                    }
                    else
                    {
                        ErrorMessage = "* Invalid username or password";
                    }
                }
                else
                {
                    ErrorMessage = "* PO information is incorrect";
                }
            }
            catch { }
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

        //private string SecureStringToString(SecureString secureString)
        //{
        //    IntPtr unmanagedString = IntPtr.Zero;
        //    try
        //    {
        //        unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
        //        return Marshal.PtrToStringUni(unmanagedString);
        //    }
        //    finally
        //    {
        //        Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        //    }
        //}
        //public void ExecuteRecoverPasswordCommand()
        //{

        //}
    }
}
