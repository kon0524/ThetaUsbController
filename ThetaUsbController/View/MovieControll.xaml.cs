using System.Windows.Controls;
using ThetaUsbController.ViewModel;

namespace ThetaUsbController.View
{
    /// <summary>
    /// MovieControll.xaml の相互作用ロジック
    /// </summary>
    public partial class MovieControll : UserControl
    {
        public MovieControll()
        {
            InitializeComponent();
            this.DataContext = new MovieViewModel();
        }
    }
}
