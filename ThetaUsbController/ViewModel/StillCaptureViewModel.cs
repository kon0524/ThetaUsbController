﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ThetaUsbController.Model;
using WpdMtpLib.DeviceProperty;

namespace ThetaUsbController.ViewModel
{
    public class StillCaptureViewModel : ViewModelBase
    {
        /// <summary>
        /// メインVM
        /// </summary>
        public MainViewModel mainVM { get; set; }

        /// <summary>
        /// 露出プログラム
        /// </summary>
        private int[] supportedExposureProgram = { 0x0001, 0x0002, 0x0004, 0x8003};
        public int ExposureProgram
        {
            get
            {
                return (int)Array.IndexOf(supportedExposureProgram, (int)theta.Program);
            }

            set
            {
                theta.Program = (ExposureProgramMode)supportedExposureProgram[value];
                NotifyPropertyChanged("ExposureProgram");
            }
        }


        /// <summary>
        /// 露出位置(0～12)
        /// </summary>
        private int evPos;
        public int EvPos
        {
            get { return evPos; }
            set
            {
                evPos = value;
                Ev = supportedEvValue[evPos];
            }
        }

        /// <summary>
        /// 露出値
        /// </summary>
        public int Ev
        {
            get { return theta.Ev; }
            set
            {
                theta.Ev = value;
                NotifyPropertyChanged("Ev");
            }
        }

        /// <summary>
        /// レリーズ
        /// </summary>
        public ICommand Release { get; set; }

        /// <summary>
        /// 露出のサポート値
        /// </summary>
        private int[] supportedEvValue = new int[] { -2000, -1700, -1300, -1000, -700, -300, 0, 300, 700, 1000, 1300, 1700, 2000};

        /// <summary>
        /// THETA
        /// </summary>
        private Theta theta;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StillCaptureViewModel()
        {
            theta = Theta.GetInstance();
            ExposureProgram = 1;
            EvPos = 6;
            Release = new DelegateCommand(releaseExecute, null);
        }

        /// <summary>
        /// レリーズ押下
        /// </summary>
        /// <param name="param"></param>
        private void releaseExecute(object param)
        {
            theta.Release();
        }
    }
}
