using DoAnWPF.views;
using DoAnWPF.Views;
using System.Windows;

namespace DoAnWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new pos(); // Mặc định load POS
        }

        private void BtnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new pos();
        }

        private void BtnSanPham_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SanPhamUC();
        }

        private void BtnHoaDon_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new QLHoaDon();
        }

        private void BtnLoaiSanPham_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new QLLH(); // load Loại sản phẩm
        }

        private void BtnDonViDo_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new QLDVT(); // load Đơn vị đo
        }
    }
}
