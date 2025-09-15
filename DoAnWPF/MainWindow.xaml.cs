using DoAnWPF.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Views;

namespace DoAnWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new pos();

        }
        private void BtnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị UserControl pos
            MainContent.Content = new pos();
        }
        private void BtnSanPham_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị UserControl pos
            MainContent.Content = new SanPhamUC();
        }


    }
}
