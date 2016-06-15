using System.Windows.Controls;
using ThetaUsbController.ViewModel;

namespace ThetaUsbController.View
{
    /// <summary>
    /// StillCaptureControll.xaml の相互作用ロジック
    /// </summary>
    public partial class StillCaptureControll : UserControl
    {
        public StillCaptureControll()
        {
            InitializeComponent();
            this.DataContext = new StillCaptureViewModel();
        }
    }
}
