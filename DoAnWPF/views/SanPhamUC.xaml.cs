using DoAnWPF.Model;
using DoAnWPF.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.views
{
    public partial class SanPhamUC : UserControl
    {
        private SanPhamViewModel spVM = new SanPhamViewModel();
        private string uploadedImagePath = null;
        private int selectedSanPhamId = -1;

        public SanPhamUC()
        {
            InitializeComponent();
            Loaded += SanPhamUC_Loaded;
        }

        private void SanPhamUC_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            spVM.LoadLoaiSanPham(cbLoaiSanPham);
            spVM.LoadDonViTinh(cbDonViTinh);
        }

        private void LoadData()
        {
            dgSanPham.ItemsSource = spVM.GetAllSanPham();
            ClearForm();
        }

        private void dgSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSanPham.SelectedItem is SanPham sp)
            {
                selectedSanPhamId = sp.SanPhamID;
                txtTenSanPham.Text = sp.TenSanPham;
                cbLoaiSanPham.SelectedValue = sp.LoaiID;
                cbDonViTinh.SelectedValue = sp.DonViID;
                txtSoLuongTon.Text = sp.SoLuongTon.ToString();
                txtGiaNhap.Text = sp.GiaNhap.ToString();
                txtGiaBan.Text = sp.GiaBan.ToString();
                txtHinhAnh.Text = sp.HinhAnh;
                uploadedImagePath = sp.HinhAnh;
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            var sp = new SanPham
            {
                TenSanPham = txtTenSanPham.Text.Trim(),
                LoaiID = (int)cbLoaiSanPham.SelectedValue,
                DonViID = (int)cbDonViTinh.SelectedValue,
                SoLuongTon = int.Parse(txtSoLuongTon.Text),
                GiaNhap = decimal.Parse(txtGiaNhap.Text),
                GiaBan = decimal.Parse(txtGiaBan.Text),
                HinhAnh = uploadedImagePath
            };

            spVM.ThemSanPham(sp);
            MessageBox.Show("Thêm sản phẩm thành công!");
            LoadData();
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSanPhamId == -1)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!");
                return;
            }

            if (!ValidateInput()) return;

            var sp = new SanPham
            {
                SanPhamID = selectedSanPhamId,
                TenSanPham = txtTenSanPham.Text.Trim(),
                LoaiID = (int)cbLoaiSanPham.SelectedValue,
                DonViID = (int)cbDonViTinh.SelectedValue,
                SoLuongTon = int.Parse(txtSoLuongTon.Text),
                GiaNhap = decimal.Parse(txtGiaNhap.Text),
                GiaBan = decimal.Parse(txtGiaBan.Text),
                HinhAnh = uploadedImagePath
            };

            spVM.SuaSanPham(sp);
            MessageBox.Show("Cập nhật sản phẩm thành công!");
            LoadData();
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSanPhamId == -1)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!");
                return;
            }

            var sp = spVM.FindSanPham(selectedSanPhamId);
            if (sp != null && MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận",
                                               MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                spVM.XoaSanPham(sp);
                MessageBox.Show("Xóa sản phẩm thành công!");
                LoadData();
            }
        }

        private void BtnUploadAnh_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif";

            if (openFile.ShowDialog() == true)
            {
                string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string uploadsFolder = Path.Combine(projectDir, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(openFile.FileName);
                string destPath = Path.Combine(uploadsFolder, fileName);

                File.Copy(openFile.FileName, destPath, true);

                uploadedImagePath = "uploads/" + fileName;
                txtHinhAnh.Text = uploadedImagePath;

                MessageBox.Show("Upload ảnh thành công!");
            }
        }

        private void ClearForm()
        {
            txtTenSanPham.Clear();
            cbLoaiSanPham.SelectedIndex = -1;
            cbDonViTinh.SelectedIndex = -1;
            txtSoLuongTon.Clear();
            txtGiaNhap.Clear();
            txtGiaBan.Clear();
            txtHinhAnh.Clear();
            uploadedImagePath = null;
            selectedSanPhamId = -1;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenSanPham.Text))
            {
                MessageBox.Show("Tên sản phẩm không được bỏ trống!");
                return false;
            }

            if (cbLoaiSanPham.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm!");
                return false;
            }

            if (cbDonViTinh.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính!");
                return false;
            }

            if (!int.TryParse(txtSoLuongTon.Text, out _) ||
                !decimal.TryParse(txtGiaNhap.Text, out _) ||
                !decimal.TryParse(txtGiaBan.Text, out _))
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ cho số lượng, giá nhập và giá bán!");
                return false;
            }

            return true;
        }
    }
}
