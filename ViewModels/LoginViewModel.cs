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
using System.IO;


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
        private string _FullName;

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
                    UserSession.CurrentPO = PO;
                    LoginCommand();
                }
                else
                {
                    ErrorMessage = "* PO information is incorrect";
                }
            } 
        }
        public string FullName
        {
            get => _FullName;
            set
            {
                _FullName = value;
                NotifyOfPropertyChange(() => FullName);
            }
        }
        public string Username 
        {
            get => _username;
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
                try
                {
                    UserModel userModel = _readExcelData.FindProductByID(Username);
                    if (userModel != null)
                    {
                        UserSession.CurrentUser = userModel.Name;
                        UserSession.CurrentAccess = userModel.Access;
                        UserSession.CurrentID = userModel.ID;
                        FullName = userModel.Name;
                        IsViewVisible = false;
                        IsAuthenticated = true;
                        ErrorMessage = "";
                        IsIDFocused = false;
                        IsPOFocused = true;

                    }
                    else
                    {
                        FullName = "";
                        ErrorMessage = "* Invalid username or password";
                    }
                }
                catch (Exception)
                {
                    ErrorMessage = "* Employee data file not found";
                }
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

        private bool _IsIDFocused = true;
        public bool IsIDFocused { get => _IsIDFocused; set {  _IsIDFocused = value; NotifyOfPropertyChange(() => IsIDFocused); } }

        private bool _IsPOFocused;
        public bool IsPOFocused { get => _IsPOFocused; set { _IsPOFocused = value; NotifyOfPropertyChange(() => IsPOFocused); } }

        private readonly IWindowManager _windowManager;
        private ReadExcelData _readExcelData;
        private string basePathConfig;

        public LoginViewModel(IWindowManager windowManager)
        {
            LoadDataConfig();

            _windowManager = windowManager;
            _readExcelData = new ReadExcelData();
            userRepository = new UserRepository();
        }

        public void LoginCommand()
        {
            try
            {
                UserModel userModel = _readExcelData.FindProductByID(Username);
                //int _numberlogin = 0;
                if (userModel != null)
                {
                    UserSession.CurrentUser = userModel.Name;
                    UserSession.CurrentAccess = userModel.Access;
                    UserSession.NumberOfLoginTimes++;
                    if(UserSession.NumberOfLoginTimes > 1)
                    {
                        UserSession.NumberOfLoginTimes = 2;
                    }    
                    IsViewVisible = false;
                    IsAuthenticated = true;
                    var mainVM = IoC.Get<MainViewModel>();
                    _windowManager.ShowWindowAsync(mainVM);
                    if (_view is LoginView window)
                    {
                        window.CloseWithFade();
                    }
                }
                else
                {
                    ErrorMessage = "* Invalid username or password";
                }
            }
            catch { }
        }

        public void bnClose()
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                if (_view is LoginView window)
                {
                    window.ShutdownWithFade();
                }
            }
            else
                return;
        }

        public void bnMinimize()
        {
            if (_view is LoginView window)
            {
                window.MinimizeWithFade();
            }
        }

        private void LoadDataConfig()
        {
            // Path File
            basePathConfig = Path.Combine(AppContext.BaseDirectory, "config.ini");

            // Data PLC Siemen
            List<string> listPLC = IniFile.ReadSectionRawValue(basePathConfig, "PLCSiemen");
            DataConfigModel.CPUTypes = listPLC[0];
            DataConfigModel.IP_PLC = listPLC[1];
            DataConfigModel._Rack = listPLC[2];
            DataConfigModel._Slot = listPLC[3];

            // Data PLC FX Serial
            List<string> listPLCFx = IniFile.ReadSectionRawValue(basePathConfig, "PLCFXSerial");
            DataConfigModel._Port = listPLCFx[0];
            DataConfigModel._BaudRate = listPLCFx[1];
            DataConfigModel._Parity = listPLCFx[2];
            DataConfigModel._DataBits = listPLCFx[3];
            DataConfigModel._StopBits = listPLCFx[4];

            // Data Employees
            List<string> listEmp = IniFile.ReadSectionRawValue(basePathConfig, "Employees");
            DataConfigModel.PathEmployees = listEmp[0];
            DataConfigModel.FileNameEmp = listEmp[1];

            // Data SQL Server
            List<string> listSQL = IniFile.ReadSectionRaw(basePathConfig, "SQLServer");
            DataConfigModel.DataSource = listSQL[0];
            DataConfigModel.InitialCatalog = listSQL[1];
            DataConfigModel.PersistSecurityInfo = listSQL[2];
            DataConfigModel.UserID = listSQL[3];
            DataConfigModel.Password = listSQL[4];
            DataConfigModel.SaveSQL = listSQL[5];

            // Data Save Excel
            List<string> listSaveData = IniFile.ReadSectionRawValue(basePathConfig, "SaveDataExcel");
            DataConfigModel.PathSaveData = listSaveData[0];
            DataConfigModel.FileSaveData = listSaveData[1];
            DataConfigModel.SaveExcel = listSaveData[2];
        }
    }
}
