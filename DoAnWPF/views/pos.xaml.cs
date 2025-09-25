using DoAnWPF.Model;
using DoAnWPF.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.Views
{
    public partial class pos : UserControl
    {
        private PosViewModel vm = new PosViewModel();
        private KhachHangViewModel khVm = new KhachHangViewModel();
        public pos()
        {
            InitializeComponent();
            LoadCategories();
            LoadProducts();
            RefreshCart();
            resetKH();
        }

       
        private void LoadCategories()
        {
            CategoryList.ItemsSource = vm.LoadLoaiSanPham();
        }

        private void LoadProducts()
        {
            ProductsControl.ItemsSource = vm.LoadSanPham();
        }

        private void LoadProductsByCategory(int loaiId)
        {
            ProductsControl.ItemsSource = vm.LoadSanPhamByLoai(loaiId);
        }

        private void RefreshCart()
        {
            CartList.ItemsSource = null;
            CartList.ItemsSource = vm.Cart;
            TotalTextBlock.Text = $"Thành tiền: {vm.GetTotal():N0} đ";
        }

        private void AllCategory_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var loai = btn?.DataContext as LoaiSanPham;
            if (loai != null)
            {
                LoadProductsByCategory(loai.LoaiID);
            }
        }

        private void Product_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var sp = btn?.DataContext as SanPham;
            if (sp != null)
            {
                vm.AddToCart(sp);
                RefreshCart();
            }
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn?.DataContext as CartItem;
            if (item != null)
            {
                var sp = vm.LoadSanPham().FirstOrDefault(s => s.SanPhamID == item.SanPhamID);
                if (sp != null)
                {
                    vm.AddToCart(sp, 1);
                    RefreshCart();
                }
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn?.DataContext as CartItem;
            if (item != null)
            {
                vm.DecreaseFromCart(item.SanPhamID);
                RefreshCart();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var customer = CustomerName.Text.Trim();
            var phone = CustomerPhone.Text.Trim();
            KhachHang kh = null;

            if (string.IsNullOrEmpty(customer) || string.IsNullOrEmpty(phone))
            {
          
                var result = MessageBox.Show(
                    "Bạn có muốn thanh toán với khách hàng ẩn danh không?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.No)
                    return;

             
                kh = khVm.GetByPhone("0");
            }
            else
            {
             
                kh = khVm.GetByPhone(phone);
                if (kh == null)
                {
                    kh = new KhachHang
                    {
                        TenKhach = customer,
                        DienThoai = phone,
                        DiaChi = ""
                    };
                    khVm.ThemKhachHang(kh);
                }
            }

            vm.ThanhToan(kh.KhachHangID);
            RefreshCart();
            resetKH();
            MessageBox.Show("Thanh toán thành công!");
        }


        private void FindCustomer_Click(object sender, RoutedEventArgs e)
        {
            var phone = CustomerPhone.Text.Trim();
            var khVm = new KhachHangViewModel();
            var kh = khVm.GetByPhone(phone);

            if (kh != null)
            {
                CustomerName.Text = kh.TenKhach;
                MessageBox.Show("Đã tìm thấy khách hàng!");
            }
            else
            {
                MessageBox.Show("Không tìm thấy khách hàng, vui lòng nhập thông tin mới!");
            }
        }
        private void resetKH()
        {
            CustomerName.Text = null;
            CustomerPhone.Text = null;
        }
    }
}
