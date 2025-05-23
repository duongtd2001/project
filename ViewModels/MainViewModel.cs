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
using project.Messages;
using System.Xml.Linq;
using System.Drawing;
using System.Data.SqlClient;
using project.Helpers;

namespace project.ViewModels
{
    public class MainViewModel : Conductor<object>, IViewAware, IHandle<AutoDoneEvent>
    {
        //Fields
        private string _currentUserAccount;
        private string _CurrentUserAccountIMG;
        private string _caption;
        private IconChar _icon;
        private Visibility _IsManualCommand = Visibility.Collapsed;

        private UserRepository userRepository;
        // properties
        public string CurrentUserAccountIMG { get => _CurrentUserAccountIMG; set { _CurrentUserAccountIMG = value; NotifyOfPropertyChange(() => CurrentUserAccountIMG); } }

        public string CurrentUserAccount { get => _currentUserAccount; set { _currentUserAccount = value; NotifyOfPropertyChange(() => CurrentUserAccount); } }

        public string Caption { get => _caption; set { _caption = value; NotifyOfPropertyChange(() => Caption); } }

        public IconChar Icon { get => _icon; set { _icon = value; NotifyOfPropertyChange(() => Icon); } }

        public Visibility IsManualCommand { get => _IsManualCommand; set { _IsManualCommand = value; NotifyOfPropertyChange(() => IsManualCommand); } }
        
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
        //private CustomerViewModel customerViewModel;
        //private HomeViewModel homeViewModel;
        private ManualViewModel manualViewModel;
        //private ReportViewModel reportViewModel;
        //private SettingViewModel settingViewModel;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private SerialPortPLC plcCom;
        private Thread _plcThread;
        private Thread _sqlThread;
        private bool _isRunning = false;
        public bool _isStatusPLC = false;
        //private ReadExcelData _readExcelData;
        private PulseConverter _convert;

        public MainViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            plcCom = new SerialPortPLC();
            plcCom.ConnectPLC();
            if (UserSession.NumberOfLoginTimes == 1)
            {
                if (plcCom.CheckConnectPLC())
                {
                    plcCom.WriteDataToPLC("M330", true);
                    plcCom.WriteDataToPLC("M0", true);
                }
            }
            _convert = new PulseConverter(5000, 5);
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            autoViewModel = new AutoViewModel(ref plcCom, _eventAggregator, ref _convert);
            //customerViewModel = new CustomerViewModel();
            //homeViewModel = new HomeViewModel();
            manualViewModel = new ManualViewModel(ref plcCom, _eventAggregator, ref _convert);
            //reportViewModel = new ReportViewModel();
            //settingViewModel = new SettingViewModel();
            //_readExcelData = new ReadExcelData();
            userRepository = new UserRepository();
            ShowAutoCommand();
            LoadCurrentUserData();
            StartPlcThread();
            SQLConn();
            _windowManager = windowManager;
        }

        //public void ShowHomeViewCommand()
        //{
        //    ActivateItemAsync(homeViewModel);
        //    Caption = "Dashboard";
        //    Icon = IconChar.Home;
        //}
        //public void ShowCustomerViewCommand()
        //{
        //    ActivateItemAsync(customerViewModel);
        //    Caption = "Customer";
        //    Icon = IconChar.UserGroup;
        //}
        //public void ShowReportCommand()
        //{
        //    ActivateItemAsync(reportViewModel);
        //    Caption = "Report";
        //    Icon = IconChar.PieChart;
        //}
        //public void ShowSettingCommand()
        //{
        //    ActivateItemAsync(settingViewModel);
        //    Caption = "Setting";
        //    Icon = IconChar.Tools;
        //}
        public void ShowAutoCommand()
        {
            ActivateItemAsync(autoViewModel);
            Caption = "Auto";
            Icon = IconChar.Play;
            if(plcCom.CheckConnectPLC())
            {
                plcCom.WriteDataToPLC("M492", true);
                plcCom.WriteDataToPLC("M492", false);
            }
        }
        public void ShowManualCommand()
        {
            ActivateItemAsync(manualViewModel);
            Caption = "Manual";
            Icon = IconChar.Hand;
            if (plcCom.CheckConnectPLC())
            {
                plcCom.WriteDataToPLC("M482", true);
                plcCom.WriteDataToPLC("M482", false);
            }
        }
        public void bnClose()
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                if (_view is MainView window)
                {
                    window.CloseWithFade();
                    plcCom.DisconnectPLC();
                }
            }
            else
                return;

        }
        private string _access = "";
        private void LoadCurrentUserData()
        {
            CurrentUserAccount = UserSession.CurrentUser;
            _access = UserSession.CurrentAccess;
            if(_access.Equals("PE"))
            {
                IsManualCommand = Visibility.Visible;
            }
        }
        private void StartPlcThread()
        {
            _plcThread = new Thread(() =>
            {
                bool previousStatus = false;

                while (!_isRunning)
                {
                    bool currentStatus = plcCom.CheckConnectPLC();

                    if (!currentStatus)
                    {
                        _eventAggregator.PublishOnUIThreadAsync(new PlcDataMessage { IsConnectPLC = 0 });
                        plcCom.ConnectPLC();
                        if (UserSession.NumberOfLoginTimes == 1)
                        {
                            if (plcCom.CheckConnectPLC())
                            {
                                plcCom.WriteDataToPLC("M330", true);
                                plcCom.WriteDataToPLC("M0", true);
                            }
                        }
                        previousStatus = false;
                        Thread.Sleep(2500);
                    }
                    else
                    {
                        if (!previousStatus)
                        {
                            _eventAggregator.PublishOnUIThreadAsync(new PlcDataMessage { IsConnectPLC = 1 });
                            previousStatus = true;
                        }
                        else
                        {
                            _eventAggregator.PublishOnUIThreadAsync(new PlcDataMessage
                            {
                                IsConnectPLC = 2,
                                IsPosition = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D248")).ToString("F2"),
                                IsSpeed = _convert.ConvertSpeedPulseToMMperSec(plcCom.ReadDataFromPLC2("D252")).ToString("F2")
                            });
                            Thread.Sleep(100);
                        }
                    }
                }
            });
            _plcThread.IsBackground = true;
            _plcThread.Start();
        }

        private void SQLConn()
        {
            _sqlThread = new Thread(() =>
            {
                bool previousStatusSQL = false;

                while (!_isRunning)
                {
                    bool _statusSQLServer = userRepository.StatusConnectSQL();

                    if (!_statusSQLServer)
                    {
                        _eventAggregator.PublishOnUIThreadAsync(new SqlDataMessage { SQLConnect = 0 });
                        previousStatusSQL = false;
                        Thread.Sleep(2500);
                    }
                    else
                    {
                        if (!previousStatusSQL)
                        {
                            _eventAggregator.PublishOnUIThreadAsync(new SqlDataMessage { SQLConnect = 1 });
                            previousStatusSQL = true;
                        }
                    }
                }
            });
            _sqlThread.IsBackground = true;
            _sqlThread.Start();
        }
        public Task HandleAsync(AutoDoneEvent message, CancellationToken cancellationToken)
        {
            if (message.Message == 1)
            {
                var loginVM = IoC.Get<LoginViewModel>();
                _windowManager.ShowWindowAsync(loginVM);
                if (_view is MainView window)
                {
                    window.CloseWithFade();
                    _isRunning = true;
                    _sqlThread.Abort();
                    _plcThread.Abort();
                    plcCom.DisconnectPLC();
                }
            }
            return Task.CompletedTask;
        }
    }
}
