using System;
using System.Windows.Input;
using ThetaUsbController.Model;
using WpdMtpLib.DeviceProperty;

namespace ThetaUsbController.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// タイトル
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return theta.IsConnected ? "接続中" : "未接続";
            }
        }

        /// <summary>
        /// 撮影モード
        /// </summary>
        private ushort[] supportedCaptureMode = { 0x0001, 0x8002 }; // 0x0003はインターバルだがまだ未対応
        public ushort Mode
        {
            get 
            {
                StillCaptureMode mode = theta.CaptureMode;
                if (mode == StillCaptureMode.Interval)
                {
                    mode = StillCaptureMode.Single;
                }
                return (ushort)Array.IndexOf(supportedCaptureMode, (ushort)mode);
            }
            set
            {
                theta.CaptureMode = (StillCaptureMode)supportedCaptureMode[value];
                NotifyPropertyChanged("Mode");
            }
        }

        /// <summary>
        /// 静止画画面のVM
        /// </summary>
        public StillCaptureViewModel scVM { get; set; }

        /// <summary>
        /// 動画画面のVM
        /// </summary>
        public MovieViewModel mVM { get; set; }

        /// <summary>
        /// 接続・切断コマンド
        /// </summary>
        public ICommand Connect { get; set; }

        /// <summary>
        /// レリーズ
        /// </summary>
        public ICommand Release { get; set; }

        /// <summary>
        /// THETA
        /// </summary>
        private Theta theta;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel()
        {
            theta = Theta.GetInstance();
            Mode = (int)ThetaMode.StillCaptureMode;

            Connect = new DelegateCommand(connectExecute, null);
            Release = new DelegateCommand(releaseExecute, null);
        }

        /// <summary>
        /// 接続・切断
        /// </summary>
        /// <param name="param"></param>
        private void connectExecute(object param)
        {
            if (theta.IsConnected)
            {   // 切断する
                theta.Disconnect();
            }
            else
            {   // 接続する
                theta.Connect();
            }
            NotifyPropertyChanged("WindowTitle");
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
