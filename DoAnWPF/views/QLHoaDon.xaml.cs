using DoAnWPF.Model;
using DoAnWPF.ViewModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.views
{
    public partial class QLHoaDon : UserControl
    {
        private HoaDonViewModel hdVM = new HoaDonViewModel();
        private KhachHangViewModel khVM = new KhachHangViewModel();

        private HoaDon selectedHoaDon;

        public QLHoaDon()
        {
            InitializeComponent();
            LoadHoaDon();
        }

        // Load tất cả hóa đơn
        private void LoadHoaDon()
        {
            var data = hdVM.LoadHoaDon();

            DG_HoaDon.ItemsSource = data.Select(hd => new
            {
                hd.HoaDonID,
                hd.NgayLap,
                TenKhach = hd.KhachHang != null ? hd.KhachHang.TenKhach : "Khách lẻ",
                hd.TongTien
            }).ToList();
        }

        // Khi chọn hóa đơn
        private void DG_HoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DG_HoaDon.SelectedItem == null) return;

            dynamic row = DG_HoaDon.SelectedItem;
            int id = row.HoaDonID;

            selectedHoaDon = hdVM.FindHoaDon(id);

            if (selectedHoaDon != null)
            {
                txtMaHD.Text = selectedHoaDon.HoaDonID.ToString();
                txtNgayLap.Text = selectedHoaDon.NgayLap.ToString("dd/MM/yyyy");
                txtKhachHang.Text = selectedHoaDon.KhachHang != null ? selectedHoaDon.KhachHang.TenKhach : "Khách lẻ";

                DG_ChiTiet.ItemsSource = hdVM.LoadChiTiet(id);
            }
        }

        // Tìm theo mã hóa đơn (Find)
        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtTimKiem.Text.Trim(), out int maHD))
            {
                var hd = hdVM.FindHoaDon(maHD);
                if (hd != null)
                {
                    DG_HoaDon.ItemsSource = new[]
                    {
                        new {
                            hd.HoaDonID,
                            hd.NgayLap,
                            TenKhach = hd.KhachHang != null ? hd.KhachHang.TenKhach : "Khách lẻ",
                            hd.TongTien
                        }
                    }.ToList();
                    selectedHoaDon = hd;
                    DG_ChiTiet.ItemsSource = hdVM.LoadChiTiet(maHD);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hóa đơn!");
                    LoadHoaDon();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn hợp lệ!");
            }
        }

        // Xóa hóa đơn
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (selectedHoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?", "Xác nhận",
                                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                hdVM.XoaHoaDon(selectedHoaDon);
                LoadHoaDon();
                DG_ChiTiet.ItemsSource = null;
                txtMaHD.Clear();
                txtNgayLap.Clear();
                txtKhachHang.Clear();
                selectedHoaDon = null;

                MessageBox.Show("Xóa hóa đơn thành công!");
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (selectedHoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!");
                return;
            }

            DateTime? ngayCapNhat = null;
            if (DateTime.TryParse(txtNgayLap.Text, out DateTime tmpDate))
            {
                ngayCapNhat = tmpDate;
            }

            string tenKh = txtKhachHang.Text.Trim();

            bool ok = hdVM.SuaHoaDon(selectedHoaDon.HoaDonID, ngayCapNhat, tenKh);
            if (ok)
            {
                LoadHoaDon();
                MessageBox.Show("Sửa hóa đơn thành công!");
            }
            else
            {
                MessageBox.Show("Không tìm thấy hóa đơn để sửa!");
            }
        }
    }
}
