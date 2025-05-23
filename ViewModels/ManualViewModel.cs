using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Expando;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using project.Services;
using project.Models;

namespace project.ViewModels
{
    public class ManualViewModel : Screen, IHandle<PlcDataMessage>
    {
        private string _AirTime;
        public string AirTime { get => _AirTime; set { _AirTime = value; NotifyOfPropertyChange(() => AirTime); } }

        private string _mAirTimeValue;
        public string mAirTimeValue { get => _mAirTimeValue; set { _mAirTimeValue = value; NotifyOfPropertyChange(() => mAirTimeValue); } }

        private Brush _mIsINSERTBALL;
        public Brush mIsINSERTBALL { get => _mIsINSERTBALL; set { _mIsINSERTBALL = value; NotifyOfPropertyChange(() => mIsINSERTBALL); } }

        private Brush _IsCylinder1;
        public Brush IsCylinder1 { get => _IsCylinder1; set { _IsCylinder1 = value; NotifyOfPropertyChange(() => IsCylinder1); } }

        private Brush _IsCylinder2;
        public Brush IsCylinder2 { get => _IsCylinder2; set { _IsCylinder2 = value; NotifyOfPropertyChange(() => IsCylinder2); } }

        private Brush _IsCylinder3;
        public Brush IsCylinder3 { get => _IsCylinder3; set { _IsCylinder3 = value; NotifyOfPropertyChange(() => IsCylinder3); } }

        private string _mPos1;
        public string mPos1 { get => _mPos1; set { _mPos1 = value; NotifyOfPropertyChange(() => mPos1); } }

        private string _mPos2;
        public string mPos2 { get => _mPos2; set { _mPos2 = value; NotifyOfPropertyChange(() => mPos2); } }

        private string _mPos3;
        public string mPos3 { get => _mPos3; set { _mPos3 = value; NotifyOfPropertyChange(() => mPos3); } }

        private string _mPos4;
        public string mPos4 { get => _mPos4; set { _mPos4 = value; NotifyOfPropertyChange(() => mPos4); } }

        private string _mPos5;
        public string mPos5 { get => _mPos5; set { _mPos5 = value; NotifyOfPropertyChange(() => mPos5); } }

        private string _mPos6;
        public string mPos6 { get => _mPos6; set { _mPos6 = value; NotifyOfPropertyChange(() => mPos6); } }

        private string _mPos7;
        public string mPos7 { get => _mPos7; set { _mPos7 = value; NotifyOfPropertyChange(() => mPos7); } }

        private string _mPos8;
        public string mPos8 { get => _mPos8; set { _mPos8 = value; NotifyOfPropertyChange(() => mPos8); } }

        private string _mPos9;
        public string mPos9 { get => _mPos9; set { _mPos9 = value; NotifyOfPropertyChange(() => mPos9); } }

        private string _mPos10;
        public string mPos10 { get => _mPos10; set { _mPos10 = value; NotifyOfPropertyChange(() => mPos10); } }

        private string _mPosServo;
        public string mPosServo { get => _mPosServo; set { _mPosServo = value; NotifyOfPropertyChange(() => mPosServo); } }

        private string _mPosServoSet;
        public string mPosServoSet { get => _mPosServoSet; set { _mPosServoSet = value; NotifyOfPropertyChange(() => mPosServoSet); } }

        private string _mSpeedServo;
        public string mSpeedServo { get => _mSpeedServo; set { _mSpeedServo = value; NotifyOfPropertyChange(() => mSpeedServo); } }

        private string _mSpeedServoSet;
        public string mSpeedServoSet { get => _mSpeedServoSet; set { _mSpeedServoSet = value; NotifyOfPropertyChange(() => mSpeedServoSet); } }

        private string _mPosResVal;
        public string mPosResVal { get => _mPosResVal; set { _mPosResVal = value; NotifyOfPropertyChange(() => mPosResVal); } }

        private string _mSpeedResVal;
        public string mSpeedResVal { get => _mSpeedResVal; set { _mSpeedResVal = value; NotifyOfPropertyChange(() => mSpeedResVal); } }

        private string _mResults;
        public string mResults { get => _mResults; set { _mResults = value; NotifyOfPropertyChange(() => mResults); } }

        private int _mExpandResult = 50;
        public int mExpandResult { get => _mExpandResult; set { _mExpandResult = value; NotifyOfPropertyChange(() => mExpandResult); } }

        private bool _mIsExpand;
        public bool mIsExpand { get => _mIsExpand; set { _mIsExpand = value; NotifyOfPropertyChange(() => mIsExpand); } }

        private SerialPortPLC plcCom;
        private bool _ISmInsertBall = false;
        private bool _IsSTRCylinder1 = false;
        private bool _IsSTRCylinder2 = false;
        private bool _IsSTRCylinder3 = false;

        private PulseConverter _convert;
        private readonly IEventAggregator _eventAggregator;

        public ManualViewModel(ref SerialPortPLC _plcCom, IEventAggregator eventAggregator, ref PulseConverter pulseConverter)
        {
            plcCom = _plcCom;
            _convert = pulseConverter;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            if (plcCom.CheckConnectPLC())
            {
                Thread tFor = new Thread(() =>
                {
                    AirTime = (plcCom.ReadDataFromPLC2("D256") / 10).ToString();
                    for (int i = 1; i < 11; i++)
                    {
                        string address = $"D{200 + i * 4}";
                        var proop = this.GetType().GetProperty($"mPos{i}");
                        if (proop != null)
                        {
                            proop.SetValue(this, _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2(address)).ToString("F2"));
                        }
                    }
                });
                tFor.IsBackground = true;
                tFor.Start();
            }  
        }

        public void mExpand_CKB()
        {
            mExpandResult = mIsExpand ? 200 : 50;
        }

        public Task HandleAsync(PlcDataMessage message, CancellationToken cancellationToken)
        {
            switch (message.IsConnectPLC)
            {
                case 0:
                    mResults += DateTime.Now.ToString("yyyy MM dd - HH mm ss") + "     PLC not connected!!!\n";
                    break;
                case 1:
                    mResults += DateTime.Now.ToString("yyyy MM dd - HH mm ss") + "     PLC connected!!!\n";
                    break;
                case 2:
                    mPosServo = message.IsPosition;
                    mSpeedServo = message.IsSpeed;
                    break;
            }
            return Task.CompletedTask;
        }

        public void mSetSpeed()
        {
            try
            {
                if (plcCom.CheckConnectPLC())
                {
                    Thread t = new Thread(() =>
                    {
                        if (Convert.ToDouble(mSpeedServoSet) >= 1 && Convert.ToDouble(mSpeedServoSet) <= 80)
                        {
                            plcCom.WriteDataToPLC2("D244", Convert.ToInt32(_convert.ConvertSpeedMMperSecToPulse(Convert.ToDouble(mSpeedServoSet))));
                            plcCom.WriteDataToPLC("M120", true);
                            Thread.Sleep(100);
                            plcCom.WriteDataToPLC("M120", false);
                        }
                        else
                        {
                            MessageBox.Show("Input value out of range!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    });
                    t.IsBackground = true;
                    t.Start();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter value!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void mHome()
        {
            if (plcCom.CheckConnectPLC())
            {
                if (!plcCom.ReadDataFromPLC("M3"))
                {
                    Thread t = new Thread(() =>
                    {
                        plcCom.WriteDataToPLC("M6", true);
                        Thread.Sleep(100);
                    });
                    t.IsBackground = true;
                    t.Start();
                }
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mPosPos()
        {
            try
            {
                if (plcCom.CheckConnectPLC())
                {
                    Thread t = new Thread(() =>
                    {
                        double mResValue = Convert.ToDouble(mPosResVal);
                        plcCom.WriteDataToPLC2("D100", Convert.ToInt32(_convert.ConvertMMtoPulse((Convert.ToDouble(mPosServo) + mResValue))));
                        Thread.Sleep(200);
                        plcCom.WriteDataToPLC("M320", true);
                    });
                    t.IsBackground = true;
                    t.Start();
                }
                else
                {
                    MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter value!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        public void mNegPos()
        {
            try
            {
                if (plcCom.CheckConnectPLC())
                {
                    Thread t = new Thread(() =>
                    {
                        double mResValue = Convert.ToDouble(mPosResVal);
                        plcCom.WriteDataToPLC2("D100", Convert.ToInt32(_convert.ConvertMMtoPulse((Convert.ToDouble(mPosServo) - mResValue))));
                        Thread.Sleep(200);
                        plcCom.WriteDataToPLC("M320", true);
                    });
                    t.IsBackground = true;
                    t.Start();
                }
                else
                {
                    MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter value!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void mMotion()
        {
            try
            {
                if (plcCom.CheckConnectPLC())
                {
                    Thread t = new Thread(() =>
                    {
                        plcCom.WriteDataToPLC2("D100", Convert.ToInt32(_convert.ConvertMMtoPulse((Convert.ToDouble(mPosServoSet)))));
                        Thread.Sleep(100);
                        plcCom.WriteDataToPLC("M320", true);
                    });
                    t.IsBackground = true;
                    t.Start();
                }
                else
                {
                    MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter value!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void mMotionPos8()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M90", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M90", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos8()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M270", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M270", false);
                    mPos8 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D232")).ToString("F2");
                    MessageBox.Show("Set position 8 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mMotionPos7()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M80", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M80", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos7()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M260", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M260", false);
                    mPos7 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D228")).ToString("F2");
                    MessageBox.Show("Set position 7 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mMotionPos6()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M70", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M70", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos6()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M250", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M250", false);
                    mPos6 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D224")).ToString("F2");
                    MessageBox.Show("Set position 6 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mMotionPos5()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M60", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M60", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos5()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M240", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M240", false);
                    mPos5 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D220")).ToString("F2");
                    MessageBox.Show("Set position 5 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mMotionPos4()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M50", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M50", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos4()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M230", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M230", false);
                    mPos4 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D216")).ToString("F2");
                    MessageBox.Show("Set position 4 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mMotionPos3()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M40", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M40", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos3()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M220", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M220", false);
                    mPos3 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D212")).ToString("F2");
                    MessageBox.Show("Set position 3 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mMotionPos2()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M30", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M30", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos2()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M210", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M210", false);
                    mPos2 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D208")).ToString("F2");
                    MessageBox.Show("Set position 2 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mMotionPos1()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M20", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M20", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos1()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M200", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M200", false);
                    mPos1 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D204")).ToString("F2");
                    MessageBox.Show("Set position 1 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mMotionPos9()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M100", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M100", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos9()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M280", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M280", false);
                    mPos9 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D236")).ToString("F2");
                    MessageBox.Show("Set position 9 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mMotionPos10()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M110", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M110", false);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mSetPos10()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    plcCom.WriteDataToPLC("M290", true);
                    Thread.Sleep(100);
                    plcCom.WriteDataToPLC("M290", false);
                    mPos10 = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D240")).ToString("F2");
                    MessageBox.Show("Set position 10 success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mCylinder1()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    if (!_IsSTRCylinder1)
                    {
                        plcCom.WriteDataToPLC("M356", true);
                        IsCylinder1 = Brushes.LightGreen;
                    }
                    else
                    {
                        plcCom.WriteDataToPLC("M356", false);
                        IsCylinder1 = Brushes.Transparent;
                    }
                    _IsSTRCylinder1 = !_IsSTRCylinder1;
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mCylinder2()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    if (!_IsSTRCylinder2)
                    {
                        plcCom.WriteDataToPLC("M424", true);
                        IsCylinder2 = Brushes.LightGreen;
                    }
                    else
                    {
                        plcCom.WriteDataToPLC("M424", false);
                        IsCylinder2 = Brushes.Transparent;
                    }
                    _IsSTRCylinder2 = !_IsSTRCylinder2;
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void mCylinder3()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    if (!_IsSTRCylinder3)
                    {
                        plcCom.WriteDataToPLC("M358", true);
                        IsCylinder3 = Brushes.LightGreen;
                    }
                    else
                    {
                        plcCom.WriteDataToPLC("M358", false);
                        IsCylinder3 = Brushes.Transparent;
                    }
                    _IsSTRCylinder3 = !_IsSTRCylinder3;
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void mSetAirTime()
        {
            try
            {
                if (plcCom.CheckConnectPLC())
                {
                    Thread t = new Thread(() =>
                    {
                        plcCom.WriteDataToPLC2("D260", (Convert.ToInt32(mAirTimeValue) * 10));
                        Thread.Sleep(100);
                        plcCom.WriteDataToPLC("M368", true);
                        plcCom.WriteDataToPLC("M368", false);

                        AirTime = (plcCom.ReadDataFromPLC2("D256") / 10).ToString();
                        MessageBox.Show("Set Air Blow Time success!!!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                    });
                    t.IsBackground = true;
                    t.Start();
                }
                else
                {
                    MessageBox.Show("PLC not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter value!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void mINSERT_BALL()
        {
            if (plcCom.CheckConnectPLC())
            {
                Thread t = new Thread(() =>
                {
                    if (!_ISmInsertBall)
                    {
                        plcCom.WriteDataToPLC("M348", true);
                        mIsINSERTBALL = Brushes.LightGreen;
                        _ISmInsertBall = true;
                    }
                    else
                    {
                        plcCom.WriteDataToPLC("M348", false);
                        mIsINSERTBALL = Brushes.Transparent;
                        _ISmInsertBall = false;
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
    }
}
