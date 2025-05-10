using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using project.Views;
using project.ViewModels;
using System.Windows.Media;

namespace project.ViewModels
{
    public class AutoViewModel : Screen
    {
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

        private string _aResults;
        public string aResults { get => _aResults; set { _aResults = value; NotifyOfPropertyChange(() => aResults); } }

        private string SelectedAddress;

        public AutoViewModel() 
        { 

        }
        public void SelectPos1_Auto()
        {
            ResetButtonColors(); 
            aIsBall1 = Brushes.LightGreen; 
            NotifyOfPropertyChange(() => aIsBall1); 
            SelectedAddress = "M20";
        }
        public void SelectPos2_Auto()
        {
            ResetButtonColors(); 
            aIsBall2 = Brushes.LightGreen; 
            NotifyOfPropertyChange(() => aIsBall2); 
            SelectedAddress = "M30";
        }
        public void SelectPos3_Auto()
        {
            ResetButtonColors();
            aIsBall3 = Brushes.LightGreen;
            NotifyOfPropertyChange(() => aIsBall3);
            SelectedAddress = "M40";
        }
        public void SelectPos4_Auto()
        {
            ResetButtonColors();
            aIsBall4 = Brushes.LightGreen;
            NotifyOfPropertyChange(() => aIsBall4);
            SelectedAddress = "M50";
        }
        public void SelectPos5_Auto()
        {
            ResetButtonColors();
            aIsBall5 = Brushes.LightGreen;
            NotifyOfPropertyChange(() => aIsBall5);
            SelectedAddress = "M60";
        }
        public void SelectPos6_Auto()
        {
            ResetButtonColors();
            aIsBall6 = Brushes.LightGreen;
            NotifyOfPropertyChange(() => aIsBall6);
            SelectedAddress = "M70";
        }
        public void SelectPos7_Auto()
        {
            ResetButtonColors();
            aIsBall7 = Brushes.LightGreen;
            NotifyOfPropertyChange(() => aIsBall7);
            SelectedAddress = "M80";
        }
        public void SelectPos8_Auto()
        {
            ResetButtonColors();
            aIsBall8 = Brushes.LightGreen;
            NotifyOfPropertyChange(() => aIsBall8);
            SelectedAddress = "M90";
        }
        public void SelectPos9_Auto()
        {
            ResetButtonColors();
            aIsBall9 = Brushes.LightGreen;
            NotifyOfPropertyChange(() => aIsBall9);
            SelectedAddress = "M100";
        }
        public void SelectPos10_Auto()
        {
            ResetButtonColors();
            aIsBall10 = Brushes.LightGreen;
            NotifyOfPropertyChange(() => aIsBall10);
            SelectedAddress = "M110";
        }
        public void AirSPCommand()
        {

        }
        public void INSERTBALL_Auto()
        {

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
