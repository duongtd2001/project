using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using project.Views;
using project.ViewModels;
using System.Windows.Media;
using System.Threading;
using System.Windows;
using project.Services;
using project.Models;
using project.Repositories;
using project.Messages;
using project.Helpers;
using System.Windows.Controls;
using System.Windows.Documents;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Diagnostics.Contracts;

namespace project.ViewModels
{
    public class AutoViewModel : Screen, IHandle<PlcDataMessage>, IHandle<SqlDataMessage>, IHandle<E_Stop>
    {
        #region MyRegion
        private Brush _aIsBall1;
        public Brush aIsBall1 { get => _aIsBall1; set { _aIsBall1 = value; NotifyOfPropertyChange(() => aIsBall1); } }

        private Brush _aIsBall2;
        public Brush aIsBall2 { get => _aIsBall2; set { _aIsBall2 = value; NotifyOfPropertyChange(() => aIsBall2); } }

        private Brush _aIsBall3;
        public Brush aIsBall3 { get => _aIsBall3; set { _aIsBall3 = value; NotifyOfPropertyChange(() => aIsBall3); } }

        private Brush _aIsBall4;
        public Brush aIsBall4 { get => _aIsBall4; set { _aIsBall4 = value; NotifyOfPropertyChange(() => aIsBall4); } }

        private Brush _aIsBall5;
        public Brush aIsBall5 { get => _aIsBall5; set { _aIsBall5 = value; NotifyOfPropertyChange(() => aIsBall5); } }

        private Brush _aIsBall6;
        public Brush aIsBall6 { get => _aIsBall6; set { _aIsBall6 = value; NotifyOfPropertyChange(() => aIsBall6); } }

        private Brush _aIsBall7;
        public Brush aIsBall7 { get => _aIsBall7; set { _aIsBall7 = value; NotifyOfPropertyChange(() => aIsBall7); } }

        private Brush _aIsBall8;
        public Brush aIsBall8 { get => _aIsBall8; set { _aIsBall8 = value; NotifyOfPropertyChange(() => aIsBall8); } }

        private Brush _aIsBall9;
        public Brush aIsBall9 { get => _aIsBall9; set { _aIsBall9 = value; NotifyOfPropertyChange(() => aIsBall9); } }

        private Brush _aIsBall10;
        public Brush aIsBall10 { get => _aIsBall10; set { _aIsBall10 = value; NotifyOfPropertyChange(() => aIsBall10); } }

        private Brush _aISAirSP;
        public Brush aISAirSP { get => _aISAirSP; set { _aISAirSP = value; NotifyOfPropertyChange(() => aISAirSP); } }

        private bool _EnaBall1;
        public bool EnaBall1 { get => _EnaBall1; set { _EnaBall1 = value; NotifyOfPropertyChange(() => EnaBall1); } }

        private bool _EnaBall2;
        public bool EnaBall2 { get => _EnaBall2; set { _EnaBall2 = value; NotifyOfPropertyChange(() => EnaBall2); } }

        private bool _EnaBall3;
        public bool EnaBall3 { get => _EnaBall3; set { _EnaBall3 = value; NotifyOfPropertyChange(() => EnaBall3); } }

        private bool _EnaBall4;
        public bool EnaBall4 { get => _EnaBall4; set { _EnaBall4 = value; NotifyOfPropertyChange(() => EnaBall4); } }

        private bool _EnaBall5;
        public bool EnaBall5 { get => _EnaBall5; set { _EnaBall5 = value; NotifyOfPropertyChange(() => EnaBall5); } }

        private bool _EnaBall6;
        public bool EnaBall6 { get => _EnaBall6; set { _EnaBall6 = value; NotifyOfPropertyChange(() => EnaBall6); } }

        private bool _EnaBall7;
        public bool EnaBall7 { get => _EnaBall7; set { _EnaBall7 = value; NotifyOfPropertyChange(() => EnaBall7); } }

        private bool _EnaBall8;
        public bool EnaBall8 { get => _EnaBall8; set { _EnaBall8 = value; NotifyOfPropertyChange(() => EnaBall8); } }

        private bool _EnaBall9;
        public bool EnaBall9 { get => _EnaBall9; set { _EnaBall9 = value; NotifyOfPropertyChange(() => EnaBall9); } }

        private bool _EnaBall10;
        public bool EnaBall10 { get => _EnaBall10; set { _EnaBall10 = value; NotifyOfPropertyChange(() => EnaBall10); } }


        private string _aResults;
        public string aResults { get => _aResults; set { _aResults = value; NotifyOfPropertyChange(() => aResults); } }

        private bool _aINSERTBALL_Enabled;
        public bool aINSERTBALL_Enabled { get => _aINSERTBALL_Enabled; set { _aINSERTBALL_Enabled = value; NotifyOfPropertyChange(() => aINSERTBALL_Enabled); } }

        private string _aPosServo;
        public string aPosServo { get => _aPosServo; set { _aPosServo = value; NotifyOfPropertyChange(() => aPosServo); } }

        private string _aSpeedServo;
        public string aSpeedServo { get => _aSpeedServo; set { _aSpeedServo = value; NotifyOfPropertyChange(() => aSpeedServo); } }

        private int _ExpandResult = 50;
        public int ExpandResult { get => _ExpandResult; set { _ExpandResult = value; NotifyOfPropertyChange(() => ExpandResult); } }

        private bool _IsExpand;
        public bool IsExpand { get => _IsExpand; set { _IsExpand = value; NotifyOfPropertyChange(() => IsExpand); } }

        private bool _LockInsertBall = true;
        public bool LockInsertBall { get => _LockInsertBall; set { _LockInsertBall = value; NotifyOfPropertyChange(() => LockInsertBall); } }

        private Brush _aIsINSERTBALL;
        public Brush aIsINSERTBALL { get => _aIsINSERTBALL; set { _aIsINSERTBALL = value; NotifyOfPropertyChange(() => aIsINSERTBALL); } }
        #endregion

        private FlowDocument _logDocument = new FlowDocument();

        public FlowDocument LogDocument
        {
            get => _logDocument;
            set
            {
                _logDocument = value;
                NotifyOfPropertyChange(() => LogDocument);
            }
        }

        private string SelectedAddress;
        private SerialPortPLC plcCom;
        private Thread plcRT;
        private Thread UILog;
        private readonly IEventAggregator _eventAggregator;
        private PulseConverter _convert;
        private UserRepository userRepository;
        private SaveDataTestExcel _saveDataExcel;

        public AutoViewModel(ref SerialPortPLC _plcCom, IEventAggregator eventAggregator, ref PulseConverter pulseConverter)
        {
            
            plcCom = _plcCom;
            _convert = pulseConverter;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
            userRepository = new UserRepository();
            if (UserSession.NumberOfLoginTimes == 1)
            {
                for (int i = 1; i <= 10; i++)
                {
                    var proop2 = this.GetType().GetProperty($"EnaBall{i}");
                    proop2.SetValue(this, false);
                }
            }
            else
            {
                plcRT = new Thread(() => { RestoreButtonStates(); });
                plcRT.IsBackground = true;
                plcRT.Start();
            }
            
            _saveDataExcel = new SaveDataTestExcel();
        }
        public Task HandleAsync(PlcDataMessage message, CancellationToken cancellationToken)
        {
            switch (message.IsConnectPLC)
            {
                case 0:
                    for (int i = 1; i <= 10; i++)
                    {
                        var proop2 = this.GetType().GetProperty($"EnaBall{i}");
                        proop2.SetValue(this, false);
                    }
                    aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     PLC DISCONNECTED\n";
                    break;
                case 1:
                    aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     PLC CONNECTED\n";
                    break;
                case 2:
                    aPosServo = message.IsPosition;
                    aSpeedServo = message.IsSpeed;
                    break;
            }
            return Task.CompletedTask;
        }
        public Task HandleAsync(SqlDataMessage message, CancellationToken cancellationToken)
        {
            switch (message.SQLConnect)
            {
                case 0:
                    aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SQL SERVER DISCONNECTED\n";
                    break;
                case 1:
                    aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SQL SERVER CONNECTED\n";
                    break;
            }
            return Task.CompletedTask;
        }
        public Task HandleAsync(E_Stop message, CancellationToken cancellationToken)
        {
            switch (message.E_StopPLC)
            {
                case 0:
                    for (int i = 1; i <= 10; i++)
                    {
                        var proop2 = this.GetType().GetProperty($"EnaBall{i}");
                        proop2.SetValue(this, false);
                    }
                    aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     PLC E-STOP\n";
                    break;
                case 1:
                    break;
                case 2:
                    
                    for (int i = 1; i <= 10; i++)
                    {
                        var proop2 = this.GetType().GetProperty($"EnaBall{i}");
                        proop2.SetValue(this, true);
                    }
                    aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     MACHINE READY\n";
                    break;
            }
            return Task.CompletedTask;
        }
        public void Expand_CKB()
        {
            ExpandResult = IsExpand ? 200 : 50;
        }

        private void RestoreButtonStates()
        {
            for (int i = 1; i <= 10; i++)
            {
                var proop = this.GetType().GetProperty($"aIsBall{i}");
                var proop2 = this.GetType().GetProperty($"EnaBall{i}");
                if(i == UserSession.CurrentPos)
                {
                    proop2.SetValue(this, false);
                }
                else
                {
                    proop.SetValue(this, Brushes.Transparent);
                    proop2.SetValue(this, true);
                }
                
            }
        }
        private void UIManipulation()
        {
            bool UILogBool = true;
            UILog = new Thread(() =>
            {
                while (UILogBool)
                {
                    for (int i = 2; i <= 11; i++)
                    {
                        if (plcCom.ReadDataFromPLC($"M{i}4"))
                        {
                            for (int j = 2; j <= 11 ; j++)
                            {
                                var proop = this.GetType().GetProperty($"aIsBall{j - 1}");
                                var proop2 = this.GetType().GetProperty($"EnaBall{j - 1}");
                                if (j != i)
                                {
                                    proop.SetValue(this, Brushes.Transparent);
                                    proop2.SetValue(this, true);
                                }
                            }
                            aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     MOTION POSITION {i - 1} DONE\n";
                            UILogBool = false;
                            break;
                        }
                    }
                }
            });
            UILog.IsBackground = true;
            UILog.Start();
        }

        public void SelectPos1_Auto()
        {
            if (plcCom.CheckConnectPLC())
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall1 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall1);
                SelectedAddress = "M20";
                UserSession.SavePos = "-2";
                UserSession.CurrentPos = 1;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 1\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SelectPos2_Auto()
        {
            if ((plcCom.CheckConnectPLC()))
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall2 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall2);
                SelectedAddress = "M30";
                UserSession.SavePos = "-1.5";
                UserSession.CurrentPos = 2;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 2\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SelectPos3_Auto()
        {
            if ((plcCom.CheckConnectPLC()))
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall3 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall3);
                SelectedAddress = "M40";
                UserSession.SavePos = "-1";
                UserSession.CurrentPos = 3;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 3\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SelectPos4_Auto()
        {
            if ((plcCom.CheckConnectPLC()))
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall4 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall4);
                SelectedAddress = "M50";
                UserSession.SavePos = "-0.5";
                UserSession.CurrentPos = 4;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 4\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SelectPos5_Auto()
        {
            if ((plcCom.CheckConnectPLC()))
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall5 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall5);
                SelectedAddress = "M60";
                UserSession.SavePos = "0";
                UserSession.CurrentPos = 5;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 5\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SelectPos6_Auto()
        {
            if ((plcCom.CheckConnectPLC()))
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall6 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall6);
                SelectedAddress = "M70";
                UserSession.SavePos = "+0.5";
                UserSession.CurrentPos = 6;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 6\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SelectPos7_Auto()
        {
            if ((plcCom.CheckConnectPLC()))
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall7 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall7);
                SelectedAddress = "M80";
                UserSession.SavePos = "+1";
                UserSession.CurrentPos = 7;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 7\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SelectPos8_Auto()
        {
            if ((plcCom.CheckConnectPLC()))
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall8 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall8);
                SelectedAddress = "M90";
                UserSession.SavePos = "+1.5";
                UserSession.CurrentPos = 8;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 8\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SelectPos9_Auto()
        {
            if ((plcCom.CheckConnectPLC()))
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall9 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall9);
                SelectedAddress = "M100";
                UserSession.SavePos = "+2";
                UserSession.CurrentPos = 9;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 9\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SelectPos10_Auto()
        {
            if ((plcCom.CheckConnectPLC()))
            {
                ResetButtonColors();
                ResetEna();
                //aIsBall10 = Brushes.LightGreen;
                NotifyOfPropertyChange(() => aIsBall10);
                SelectedAddress = "M110";
                UserSession.SavePos = "+2.5";
                UserSession.CurrentPos = 10;
                Thread t = new Thread(() =>
                {
                    if (!string.IsNullOrEmpty(SelectedAddress))
                    {
                        plcCom.WriteDataToPLC(SelectedAddress, true);
                        plcCom.WriteDataToPLC(SelectedAddress, false);
                        aINSERTBALL_Enabled = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SELECT MOVE POSITION 10\n";
                        UIManipulation();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool _ISAirSP = false;
        public void AirSPCommand()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    if (!_ISAirSP)
                    {
                        plcCom.WriteDataToPLC("M376", true);
                        aISAirSP = Brushes.LightGreen;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     BLOW AIR\n";
                    }
                    else
                    {
                        if(!checkInsert)
                        {
                            plcCom.WriteDataToPLC("M376", false);
                            aISAirSP = Brushes.Transparent;
                            aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     STOP BLOWING AIR\n";
                        }
                        else
                        {
                            plcCom.WriteDataToPLC("M376", false);
                            aISAirSP = Brushes.Transparent;
                            aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     STOP BLOWING AIR\n";
                            _aInsertBoolDone = true;
                            _Question();
                        }
                    }
                    _ISAirSP = !_ISAirSP;
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool _InsertBall = false;
        private bool checkInsert = false;
        string _endtime = "";
        string _starttime = "";
        public void INSERTBALL_Auto()
        {
            if (plcCom.CheckConnectPLC())
            {
                if (UserSession.CurrentPos != 0)
                {
                    _starttime = DateTime.Now.ToString("HH:mm:ss");
                    Thread t = new Thread(() =>
                    {
                        if (!_InsertBall)
                        {
                            plcCom.WriteDataToPLC("M362", true);
                            checkInsert = true;
                            aIsINSERTBALL = Brushes.LightGreen;
                            aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     INSERT BALL\n";
                        }
                        aINSERTBALL_Enabled = false;
                    });
                    t.IsBackground = true;
                    t.Start();
                    _aInsertBool = true;
                    _aInsertDone = new Thread(() =>
                    {
                        aCheckInsertDone();
                    });
                    _aInsertDone.IsBackground = true;
                    //_aInsertDone.SetApartmentState(ApartmentState.STA);
                    _aInsertDone.Start();
                }
                else
                {
                    MessageBox.Show("Please select ball type!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        Thread _aInsertDone;
        private bool _aInsertBool = false;
        private bool _aInsertBoolDone = false;
        static ManualResetEvent saveDataSQLEvent = new ManualResetEvent(false);
        private void aCheckInsertDone()
        {
            while (_aInsertBool)
            {
                if (plcCom.ReadDataFromPLC("M366"))
                {
                    aINSERTBALL_Enabled = true;
                    _aInsertBool = false;
                    _aInsertBoolDone = true;
                    //_endtime = DateTime.Now.ToString("T");
                    _Question();
                    break;
                }
            }
        }
        private void _Question()
        {
            if(_aInsertBoolDone)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     IS THE BALL ALL IN?\n";
                    if (MessageBox.Show("IS THE BALL ALL IN?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _endtime = DateTime.Now.ToString("HH:mm:ss");
                        _aInsertBoolDone = false;
                        LockInsertBall = true;
                        Thread saveDataSQL = new Thread(() =>
                        {
                            /*UserModel _userModel = new UserModel
                            {
                                Machine = "LX15",
                                ID = UserSession.CurrentID,
                                Name = UserSession.CurrentUser,

                                //ProducName = "Product Test",
                                Date = DateTime.Now.ToString(),
                            };
                            userRepository.Add(_userModel);*/

                            _saveDataExcel.SaveData("LX15", UserSession.CurrentUser, UserSession.CurrentID, UserSession.CurrentAccess,
                                UserSession.CurrentPO, UserSession.SavePos, DateTime.Now.ToString("MM-dd-yyyy"), _starttime, _endtime);
                        });
                        saveDataSQL.IsBackground = true;
                        saveDataSQL.Start();
                        aIsINSERTBALL = Brushes.Transparent;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     YES!\n";
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     INSERT BALL DONE\n";
                        MessageBox.Show("PO complete!", "Infomation", MessageBoxButton.OK, MessageBoxImage.Information);
                        _eventAggregator.PublishOnUIThreadAsync(new AutoDoneEvent(1));
                    }
                    else
                    {
                        LockInsertBall = false;
                        _aInsertBoolDone = false;
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     NO!\n";
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     RETURN!\n";
                        return;
                    }
                });
            }    
        }

        private void ResetEna()
        {
            if (plcCom.CheckConnectPLC())
            {
                for (int i = 2; i <= 11; i++)
                {
                    var proop = this.GetType().GetProperty($"aIsBall{i - 1}");
                    var proop2 = this.GetType().GetProperty($"EnaBall{i - 1}");
                   
                    proop.SetValue(this, Brushes.Transparent);
                    proop2.SetValue(this, false);
                }
            }
        }

        private void ResetButtonColors()
        {
            aIsBall1 = Brushes.Transparent;
            aIsBall2 = Brushes.Transparent;
            aIsBall3 = Brushes.Transparent;
            aIsBall4 = Brushes.Transparent;
            aIsBall5 = Brushes.Transparent;
            aIsBall6 = Brushes.Transparent;
            aIsBall7 = Brushes.Transparent;
            aIsBall8 = Brushes.Transparent;
            aIsBall9 = Brushes.Transparent;
            aIsBall10 = Brushes.Transparent;

            NotifyOfPropertyChange(nameof(aIsBall1));
            NotifyOfPropertyChange(nameof(aIsBall2));
            NotifyOfPropertyChange(nameof(aIsBall3));
            NotifyOfPropertyChange(nameof(aIsBall4));
            NotifyOfPropertyChange(nameof(aIsBall5));
            NotifyOfPropertyChange(nameof(aIsBall6));
            NotifyOfPropertyChange(nameof(aIsBall7));
            NotifyOfPropertyChange(nameof(aIsBall8));
            NotifyOfPropertyChange(nameof(aIsBall9));
            NotifyOfPropertyChange(nameof(aIsBall10));
        }
    }
}
