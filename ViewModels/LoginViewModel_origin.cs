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


namespace project.ViewModels
{
    public class LoginViewModel_origin : Screen
    {
        //Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;
        public bool IsAuthenticated { get; set; }

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
        private readonly IWindowManager _windowManager;
        private readonly IUserRepository _userRepository;
        private readonly IEventAggregator _eventAggregator;
        //public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }
        private readonly SimpleContainer _container;

        public LoginViewModel_origin(
           IWindowManager windowManager,
           IUserRepository userRepository,
           IEventAggregator eventAggregator)
            {
            _windowManager = windowManager;
            _userRepository = userRepository;
            _eventAggregator = eventAggregator;
        }

        //public bool CanExecuteLoginCommand()
        //{
        //    bool validData;
        //    if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
        //        Password == null || Password.Length < 3)
        //        validData = false;
        //    else
        //        validData = true;
        //    return validData;
        //}

        public async Task LoginCommand()
        {
            #region cmd
            //var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
            //if (isValidUser)
            //{
            //    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
            //    IsViewVisible = false;
            //}
            //else
            //{
            //    ErrorMessage = "* Invalid username or password";
            //}
            // new
            /*string password = ConvertToUnsecureString(Password);
            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Username, password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                IsViewVisible = false;
                IsAuthenticated = true;
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }*/


            /*protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    var mainView = new MainWindow();
                    mainView.Show();
                    loginView.Close();
                }
            };
        }*/
            #endregion

            string password = ConvertToUnsecureString(Password);
            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Username, password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                IsAuthenticated = true;

                var mainViewModel = _container.GetInstance<MainViewModel>(); // Lấy MainViewModel từ container
                await _windowManager.ShowWindowAsync(mainViewModel);  // Hiển thị cửa sổ chính
                await TryCloseAsync();
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }
        private string SecureStringToString(SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                // Giải mã SecureString thành chuỗi plain-text
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                // Xóa dữ liệu nhạy cảm khỏi bộ nhớ
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        public void ExecuteRecoverPasswordCommand()
        {

        }
        private string ConvertToUnsecureString(SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secureString);
                return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(unmanagedString);
            }
            finally
            {
                if (unmanagedString != IntPtr.Zero)
                {
                    System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(unmanagedString);
                }
            }
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
