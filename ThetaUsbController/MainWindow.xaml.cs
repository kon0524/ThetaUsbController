using System.Windows;
using ThetaUsbController.ViewModel;

namespace ThetaUsbController
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel mainVM = new MainViewModel();
            mainVM.scVM = this.StillCaptureControl.DataContext as StillCaptureViewModel;
            mainVM.mVM = this.MovieControl.DataContext as MovieViewModel;
            this.DataContext = mainVM;
        }
    }
}
