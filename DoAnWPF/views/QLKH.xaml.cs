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

namespace DoAnWPF.views
{
    /// <summary>
    /// Interaction logic for QLKH.xaml
    /// </summary>
    public partial class QLKH : UserControl
    {
        private DoanWPFEntities _db = new DoanWPFEntities();
        private int selectedKhachHangId = -1; // lưu ID khi chọn trên DataGrid

        public QLKH()
        {
            InitializeComponent();
            LoadData();
        }

        // Load danh sách khách hàng + hóa đơn
        private void LoadData()
        {
            DataKhachHang.ItemsSource = _db.KhachHangs.ToList();
            dataHoaDon.ItemsSource = _db.HoaDons.ToList();
        }

        // Thêm khách hàng
        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            try
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

                _db.KhachHangs.Add(kh);
                _db.SaveChanges();

                MessageBox.Show("Thêm khách hàng thành công!");
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm khách hàng: " + ex.Message);
            }
        }

        // Xóa khách hàng
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (selectedKhachHangId <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa!");
                return;
            }

            try
            {
                var kh = _db.KhachHangs.FirstOrDefault(k => k.KhachHangID == selectedKhachHangId);
                if (kh != null)
                {
                    _db.KhachHangs.Remove(kh);
                    _db.SaveChanges();
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa khách hàng: " + ex.Message);
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

            try
            {
                var kh = _db.KhachHangs.FirstOrDefault(k => k.KhachHangID == selectedKhachHangId);
                if (kh != null)
                {
                    kh.TenKhach = txt_TenKhachHang.Text.Trim();
                    kh.DienThoai = txt_sdtkh.Text.Trim();

                    _db.SaveChanges();
                    MessageBox.Show("Cập nhật thành công!");
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa khách hàng: " + ex.Message);
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
                dataHoaDon.ItemsSource = _db.HoaDons
                                           .Where(hd => hd.KhachHangID == kh.KhachHangID)
                                           .ToList();
            }
        }

        // Hàm clear form
        private void ClearForm()
        {
            txt_TenKhachHang.Text = "";
            txt_sdtkh.Text = "";
            selectedKhachHangId = -1;
        }
    }
}
