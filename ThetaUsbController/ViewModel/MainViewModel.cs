using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ThetaUsbController.Model;

namespace ThetaUsbController.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// 接続状態
        /// </summary>
        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                NotifyPropertyChanged("IsConnected");
            }
        }

        /// <summary>
        /// 撮影モード
        /// </summary>
        private int mode;
        public int Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                NotifyPropertyChanged("Mode");
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel()
        {
            IsConnected = false;
            Mode = (int)ThetaMode.StillCaptureMode;
        }


    }
}
