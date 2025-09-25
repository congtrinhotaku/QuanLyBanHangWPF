using DoAnWPF.Model;
using DoAnWPF.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.views
{
    public partial class QLKH : UserControl
    {
        private KhachHangViewModel khVM = new KhachHangViewModel();
        private HoaDonViewModel hdVM = new HoaDonViewModel();
        private int selectedKhachHangId = -1; // lưu ID khi chọn trên DataGrid

        public QLKH()
        {
            InitializeComponent();
            LoadData();
        }

        // Load danh sách khách hàng + tất cả hóa đơn
        private void LoadData()
        {
            DataKhachHang.ItemsSource = khVM.GetAllKhachHang();
            dataHoaDon.ItemsSource = hdVM.LoadHoaDon();
            ClearForm();
        }

        // Thêm khách hàng
        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_TenKhachHang.Text))
            {
                MessageBox.Show("Tên khách hàng không được bỏ trống!");
                return;
            }

            var kh = new KhachHang
            {
                TenKhach = txt_TenKhachHang.Text.Trim(),
                DienThoai = txt_sdtkh.Text.Trim()
            };

            khVM.ThemKhachHang(kh);
            MessageBox.Show("Thêm khách hàng thành công!");
            LoadData();
        }

        // Xóa khách hàng
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (selectedKhachHangId <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa!");
                return;
            }

            var kh = khVM.FindKhachHang(selectedKhachHangId);
            if (kh != null)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận",
                                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    khVM.XoaKhachHang(kh);
                    MessageBox.Show("Xóa khách hàng thành công!");
                    LoadData();
                }
            }
        }

        // Sửa khách hàng
        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (selectedKhachHangId <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để sửa!");
                return;
            }

            var kh = khVM.FindKhachHang(selectedKhachHangId);
            if (kh != null)
            {
                kh.TenKhach = txt_TenKhachHang.Text.Trim();
                kh.DienThoai = txt_sdtkh.Text.Trim();

                khVM.SuaKhachHang(kh);
                MessageBox.Show("Cập nhật khách hàng thành công!");
                LoadData();
            }
        }

        // Khi chọn dòng trong DataGrid khách hàng
        private void DataKhachHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataKhachHang.SelectedItem is KhachHang kh)
            {
                selectedKhachHangId = kh.KhachHangID;
                txt_TenKhachHang.Text = kh.TenKhach;
                txt_sdtkh.Text = kh.DienThoai;

                // Load hóa đơn theo khách hàng
                dataHoaDon.ItemsSource = hdVM.GetHoaDonByKhachHangId(kh.KhachHangID);
            }
        }

        // Hàm clear form
        private void ClearForm()
        {
            txt_TenKhachHang.Clear();
            txt_sdtkh.Clear();
            selectedKhachHangId = -1;
        }
    }
}
