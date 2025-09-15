using DoAnWPF;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2.Views
{
    public partial class pos : UserControl
    {
        private DoanWPFEntities _db = new DoanWPFEntities();
        public ObservableCollection<CartItem> Cart { get; set; } = new ObservableCollection<CartItem>();

        public pos()
        {
            InitializeComponent();

            // Load categories
            CategoryList.ItemsSource = _db.LoaiSanPhams.ToList();

            // Load products
            ProductsControl.ItemsSource = _db.SanPhams.ToList();

            // Bind cart
            CartList.ItemsSource = Cart;
        }

        // Khi click vào category
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is LoaiSanPham loai)
            {
                ProductsControl.ItemsSource = _db.SanPhams
                                                .Where(sp => sp.LoaiID == loai.LoaiID)
                                                .ToList();
            }
        }
        private void AllCategory_Click(object sender, RoutedEventArgs e)
        {
            ProductsControl.ItemsSource = _db.SanPhams.ToList();
        }


        // Khi click vào product
        private void Product_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is SanPham sp)
            {
                var existing = Cart.FirstOrDefault(c => c.SanPhamID == sp.SanPhamID);
                if (existing != null)
                {
                    existing.Quantity++;
                    existing.UpdateTotal();
                }
                else
                {
                    var newItem = new CartItem
                    {
                        SanPhamID = sp.SanPhamID,
                        Name = sp.TenSanPham,
                        Price = sp.GiaBan,
                        Quantity = 1
                    };
                    newItem.UpdateTotal();
                    Cart.Add(newItem);
                }
                UpdateTotalText();
                CartList.Items.Refresh();
            }
        }

        // Tăng số lượng
        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CartItem item)
            {
                item.Quantity++;
                item.UpdateTotal();
                UpdateTotalText();
                CartList.Items.Refresh();
            }
        }

        // Giảm số lượng
        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CartItem item)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    item.UpdateTotal();
                }
                else
                {
                    Cart.Remove(item);
                }
                UpdateTotalText();
                CartList.Items.Refresh();
            }
        }

        // Update tổng tiền
        private void UpdateTotalText()
        {
            decimal total = Cart.Sum(c => c.Total);
            TotalTextBlock.Text = $"Thành tiền: {total:N0} đ";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // ✅ Lấy thông tin khách hàng từ giao diện
                string soDienThoai = CustomerPhone.Text.Trim();
                if (string.IsNullOrEmpty(soDienThoai)) soDienThoai = "0";

                string tenKhach = CustomerName.Text.Trim();
                if (string.IsNullOrEmpty(tenKhach)) tenKhach = "Anonymous";

                // ✅ Tìm khách hàng trong DB theo số điện thoại
                var khach = _db.KhachHangs.FirstOrDefault(k => k.DienThoai == soDienThoai);
                if (khach == null)
                {
                    khach = new KhachHang
                    {
                        TenKhach = tenKhach,
                        DienThoai = soDienThoai
                    };
                    _db.KhachHangs.Add(khach);
                    _db.SaveChanges();
                }

                // ✅ Tạo hóa đơn mới
                var hoaDon = new HoaDon
                {
                    KhachHangID = khach.KhachHangID,
                    NgayLap = DateTime.Now,
                    NguoiLap = "Chủ cửa hàng",
                    TongTien = Cart.Sum(c => c.Total)
                };
                _db.HoaDons.Add(hoaDon);
                _db.SaveChanges();

                // ✅ Lưu chi tiết hóa đơn
                foreach (var item in Cart)
                {
                    var cthd = new ChiTietHoaDon
                    {
                        HoaDonID = hoaDon.HoaDonID,
                        SanPhamID = item.SanPhamID,
                        SoLuong = item.Quantity,
                        DonGia = item.Price
                    };
                    _db.ChiTietHoaDons.Add(cthd);

                    // Giảm số lượng tồn trong bảng sản phẩm
                    var sp = _db.SanPhams.FirstOrDefault(s => s.SanPhamID == item.SanPhamID);
                    if (sp != null)
                    {
                        sp.SoLuongTon -= item.Quantity;
                    }
                }
                _db.SaveChanges();

                // ✅ Thông báo thành công
                MessageBox.Show($"Thanh toán thành công!\nMã hóa đơn: {hoaDon.HoaDonID}\nTổng tiền: {hoaDon.TongTien:N0} đ",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // ✅ Reset giỏ hàng
                Cart.Clear();
                UpdateTotalText();
                CartList.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        // ✅ Tìm khách hàng theo số điện thoại
        private void FindCustomer_Click(object sender, RoutedEventArgs e)
        {
            string phone = CustomerPhone.Text.Trim();
            if (!string.IsNullOrEmpty(phone))
            {
                var khach = _db.KhachHangs.FirstOrDefault(k => k.DienThoai == phone);
                if (khach != null)
                {
                    CustomerName.Text = khach.TenKhach; // điền tên vào textbox
                    MessageBox.Show("Đã tìm thấy khách hàng: " + khach.TenKhach, "Thông báo");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng với số điện thoại này!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập số điện thoại để tìm.", "Cảnh báo");
            }
        }

    }


    // Model giỏ hàng
    public class CartItem
    {
        public int SanPhamID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public void UpdateTotal()
        {
            Total = Price * Quantity;
        }
    }
}
