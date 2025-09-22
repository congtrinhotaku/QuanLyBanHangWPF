using DoAnWPF;            // chứa DbContext và entity SanPham
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.views
{
    public partial class SanPhamUC : UserControl
    {
        DoanWPFEntities db = new DoanWPFEntities();
        private string uploadedImagePath = null;
        private int selectedSanPhamId = -1;

        public SanPhamUC()
        {
            InitializeComponent();
        }

        private void SanPhamUC_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();

            cbLoaiSanPham.ItemsSource = db.LoaiSanPhams.ToList();
            cbLoaiSanPham.DisplayMemberPath = "TenLoai";
            cbLoaiSanPham.SelectedValuePath = "LoaiID";

            cbDonViTinh.ItemsSource = db.DonViTinhs.ToList();
            cbDonViTinh.DisplayMemberPath = "TenDonVi";
            cbDonViTinh.SelectedValuePath = "DonViID";
        }

        public void loadData()
        {
            dgSanPham.ItemsSource = db.SanPhams.ToList();
            dgSanPham.SelectedIndex = -1;
            ClearForm();
        }

        private void DgSanPham_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void DgSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            try
            {
                var sp = new SanPham()
                {
                    TenSanPham = txtTenSanPham.Text,
                    LoaiID = (int)cbLoaiSanPham.SelectedValue,
                    DonViID = (int)cbDonViTinh.SelectedValue,
                    SoLuongTon = int.Parse(txtSoLuongTon.Text),
                    GiaNhap = decimal.Parse(txtGiaNhap.Text),
                    GiaBan = decimal.Parse(txtGiaBan.Text),
                    HinhAnh = uploadedImagePath,
                };

                db.SanPhams.Add(sp);
                db.SaveChanges();

                loadData();
                MessageBox.Show("Thêm sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message);
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedSanPhamId == -1)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!");
                    return;
                }

                var sp = db.SanPhams.FirstOrDefault(x => x.SanPhamID == selectedSanPhamId);
                if (sp != null)
                {
                    sp.TenSanPham = txtTenSanPham.Text;
                    sp.LoaiID = (int)cbLoaiSanPham.SelectedValue;
                    sp.DonViID = (int)cbDonViTinh.SelectedValue;
                    sp.SoLuongTon = int.Parse(txtSoLuongTon.Text);
                    sp.GiaNhap = decimal.Parse(txtGiaNhap.Text);
                    sp.GiaBan = decimal.Parse(txtGiaBan.Text);
                    sp.HinhAnh = uploadedImagePath;

                    db.SaveChanges();
                    loadData();
                    MessageBox.Show("Cập nhật sản phẩm thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message);
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedSanPhamId == -1)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!");
                    return;
                }

                var sp = db.SanPhams.FirstOrDefault(x => x.SanPhamID == selectedSanPhamId);
                if (sp != null)
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        db.SanPhams.Remove(sp);
                        db.SaveChanges();
                        loadData();
                        MessageBox.Show("Xóa sản phẩm thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message);
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

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

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
    }
}
