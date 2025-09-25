using DoAnWPF.Model;
using DoAnWPF.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.views
{
    public partial class QLLH : UserControl
    {
        private LoaiSanPhamViewModel loaiSPVM = new LoaiSanPhamViewModel();
        private int selectedLoaiId = -1;

        public QLLH()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            dtgLoaiSP.ItemsSource = loaiSPVM.GetAllLoaiSanPham();
            ClearForm();
        }

        private void btnThemLoai_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoaiSP.Text))
            {
                MessageBox.Show("Tên loại sản phẩm không được bỏ trống!");
                return;
            }

            if (loaiSPVM.AddLoaiSanPham(txtLoaiSP.Text.Trim()))
            {
                MessageBox.Show("Thêm loại sản phẩm thành công!");
                LoadData();
            }
        }

        private void btnXoaLoai_Click(object sender, RoutedEventArgs e)
        {
            if (selectedLoaiId <= 0)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa loại sản phẩm này?", "Xác nhận",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (loaiSPVM.DeleteLoaiSanPham(selectedLoaiId))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                }
            }
        }

        private void btnSuaLoai_Click(object sender, RoutedEventArgs e)
        {
            if (selectedLoaiId <= 0)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm để sửa!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLoaiSP.Text))
            {
                MessageBox.Show("Tên loại sản phẩm không được bỏ trống!");
                return;
            }

            if (loaiSPVM.UpdateLoaiSanPham(selectedLoaiId, txtLoaiSP.Text.Trim()))
            {
                MessageBox.Show("Cập nhật loại sản phẩm thành công!");
                LoadData();
            }
        }

        private void dtgLoaiSP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgLoaiSP.SelectedItem is LoaiSanPham loai)
            {
                selectedLoaiId = loai.LoaiID;
                txtLoaiSP.Text = loai.TenLoai;
            }
        }

        private void ClearForm()
        {
            txtLoaiSP.Clear();
            selectedLoaiId = -1;
        }
    }
}
