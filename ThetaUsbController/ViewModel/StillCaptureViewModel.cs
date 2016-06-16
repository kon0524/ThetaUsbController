using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ThetaUsbController.Model;

namespace ThetaUsbController.ViewModel
{
    public class StillCaptureViewModel : ViewModelBase
    {
        /// <summary>
        /// メインVM
        /// </summary>
        public MainViewModel mainVM { get; set; }

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
        private int ev;
        public int Ev
        {
            get { return ev; }
            set
            {
                ev = value;
                Theta.Ev = ev;
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
        /// コンストラクタ
        /// </summary>
        public StillCaptureViewModel()
        {
            EvPos = 6;
            Release = new DelegateCommand(releaseExecute, null);
        }

        /// <summary>
        /// レリーズ押下
        /// </summary>
        /// <param name="param"></param>
        private void releaseExecute(object param)
        {
            Theta.Release();
        }
    }
}
