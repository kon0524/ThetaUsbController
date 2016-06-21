using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int[] supportedExposureProgram = { 0x0001, 0x0002, 0x0004, 0x8003 };
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
                NotifyPropertyChanged("SelectedIsoIndex");
                NotifyPropertyChanged("EnableIso");
                NotifyPropertyChanged("SelectedShutterSpeedIndex");
                NotifyPropertyChanged("EnableShutterSpeed");
            }
        }

        /// <summary>
        /// ISO感度
        /// </summary>
        private short[] supportedIso = { 100, 125, 160, 200, 250, 320, 400, 500, 640, 800, 1000, 1250, 1600, -1 };
        public bool EnableIso
        {
            get
            {
                ExposureProgramMode program = theta.Program;
                return (program == ExposureProgramMode.IsoPriorityProgram) || (program == ExposureProgramMode.ManualProgram);
            }
        }
        public int SelectedIsoIndex
        {
            get
            {
                return Array.IndexOf(supportedIso, theta.Iso);
            }
            set
            {
                theta.Iso = supportedIso[value];
            }
        }
        
        /// <summary>
        /// シャッター速度
        /// </summary>
        private ulong[] supportedShutterSpeed = 
        {
            0x0000190000000001,0x0000138800000001,0x00000FA000000001,0x00000C8000000001,0x000009C400000001,
            0x000007D000000001,0x0000064000000001,0x000004E200000001,0x000003E800000001,0x0000032000000001,
            0x0000028000000001,0x000001F400000001,0x0000019000000001,0x0000014000000001,0x000000FA00000001,
            0x000000C800000001,0x000000A000000001,0x0000007D00000001,0x0000006400000001,0x0000005000000001,
            0x0000003C00000001,0x0000003200000001,0x0000002800000001,0x0000001E00000001,0x0000001900000001,
            0x0000001400000001,0x0000000F00000001,0x0000000D00000001,0x0000000A00000001,0x0000000800000001,
            0x0000000600000001,0x0000000500000001,0x0000000400000001,0x0000000300000001,0x000000190000000A,
            0x0000000200000001,0x000000100000000A,0x0000000D0000000A,0x0000000100000001,0x0000000A0000000D,
            0x000000100000000A,0x0000000100000002,0x0000000A00000019,0x0000000A00000020,0x0000000100000004,
            0x0000000100000005,0x0000000100000006,0x0000000100000008,0x000000010000000A,0x000000010000000D,
            0x000000010000000F,0x0000000100000014,0x0000000100000019,0x000000010000001E,0x000000010000003C,
            0x0000000000000000
        };
        public bool EnableShutterSpeed
        {
            get
            {
                ExposureProgramMode program = theta.Program;
                return (program == ExposureProgramMode.ShutterPriorityProgram) || (program == ExposureProgramMode.ManualProgram);
            }
        }
        public int SelectedShutterSpeedIndex
        {
            get
            {
                return Array.IndexOf(supportedShutterSpeed, theta.Shutter);
            }
            set
            {
                theta.Shutter = supportedShutterSpeed[value];
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
        /// ホワイトバランス
        /// </summary>
        private ushort[] supportedWhiteBalance = { 0x0002, 0x0004, 0x8001, 0x8002, 0x0006, 0x8020, 0x8003, 0x8004, 0x8005, 0x8006, };
        public ushort Wb
        {
            get
            {
                return (ushort)Array.IndexOf(supportedWhiteBalance, (ushort)theta.Wb);
            }

            set
            {
                theta.Wb = (WhiteBalance)supportedWhiteBalance[value];
            }
        }

        /// <summary>
        /// 露出のサポート値
        /// </summary>
        private int[] supportedEvValue = new int[] { -2000, -1700, -1300, -1000, -700, -300, 0, 300, 700, 1000, 1300, 1700, 2000 };

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
        }
    }
}
