using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.views
{
    public partial class QLHoaDon : UserControl
    {
        DoanWPFEntities db = new DoanWPFEntities();
        private int selectedHoaDonId = -1;

        public QLHoaDon()
        {
            InitializeComponent();
            LoadHoaDon();
        }

        // Load danh sách hóa đơn
        private void LoadHoaDon(string keyword = "")
        {
            var query = db.HoaDons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(hd => hd.KhachHang.TenKhach.Contains(keyword));
            }

            var data = query
                .Select(hd => new
                {
                    hd.HoaDonID,
                    hd.NgayLap,
                    TenKhach = hd.KhachHang != null ? hd.KhachHang.TenKhach : "Khách lẻ",
                    hd.TongTien
                })
                .ToList();

            DG_HoaDon.ItemsSource = data;
        }

        // Khi chọn hóa đơn → load chi tiết
        private void DG_HoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DG_HoaDon.SelectedItem == null) return;

            dynamic row = DG_HoaDon.SelectedItem;
            selectedHoaDonId = row.HoaDonID;

            var hd = db.HoaDons.FirstOrDefault(x => x.HoaDonID == selectedHoaDonId);
            if (hd != null)
            {
                txtMaHD.Text = hd.HoaDonID.ToString();
                txtNgayLap.Text = hd.NgayLap.ToString("dd/MM/yyyy");
                txtKhachHang.Text = hd.KhachHang != null ? hd.KhachHang.TenKhach : "Khách lẻ";

                var chiTiet = hd.ChiTietHoaDons.Select(ct => new
                {
                    ct.SanPhamID,
                    TenSanPham = ct.SanPham.TenSanPham,
                    TenDonVi = ct.SanPham.DonViTinh.TenDonVi,
                    ct.SoLuong,
                    ct.DonGia,
                    ct.ThanhTien
                }).ToList();

                DG_ChiTiet.ItemsSource = chiTiet;
            }
        }

        // Tìm kiếm
        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            LoadHoaDon(txtTimKiem.Text.Trim());
        }

        // Xóa hóa đơn
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (selectedHoaDonId == -1)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var hd = db.HoaDons.FirstOrDefault(x => x.HoaDonID == selectedHoaDonId);
                if (hd != null)
                {
                    // Xóa chi tiết trước
                    db.ChiTietHoaDons.RemoveRange(hd.ChiTietHoaDons);
                    db.HoaDons.Remove(hd);
                    db.SaveChanges();

                    LoadHoaDon();
                    DG_ChiTiet.ItemsSource = null;
                    txtMaHD.Clear();
                    txtNgayLap.Clear();
                    txtKhachHang.Clear();

                    MessageBox.Show("Xóa hóa đơn thành công!");
                }
            }
        }

        // Sửa hóa đơn (chỉ demo sửa ngày lập và khách hàng)
        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (selectedHoaDonId == -1)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!");
                return;
            }

            var hd = db.HoaDons.FirstOrDefault(x => x.HoaDonID == selectedHoaDonId);
            if (hd != null)
            {
                try
                {
                    // giả sử chỉ cho phép sửa ngày lập và khách hàng
                    if (DateTime.TryParse(txtNgayLap.Text, out DateTime newDate))
                    {
                        hd.NgayLap = newDate;
                    }
                    if (!string.IsNullOrWhiteSpace(txtKhachHang.Text))
                    {
                        var kh = db.KhachHangs.FirstOrDefault(k => k.TenKhach == txtKhachHang.Text);
                        if (kh != null)
                        {
                            hd.KhachHangID = kh.KhachHangID;
                        }
                    }

                    db.SaveChanges();
                    LoadHoaDon();
                    MessageBox.Show("Sửa hóa đơn thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa: " + ex.Message);
                }
            }
        }
    }
}
