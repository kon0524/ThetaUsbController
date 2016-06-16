using System.Windows.Input;
using ThetaUsbController.Model;

namespace ThetaUsbController.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
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
        }
    }
}
