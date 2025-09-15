using DoAnWPF;            // chứa DbContext và entity SanPham
using DoAnWPF.views;      // namespace view
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.views
{
    public partial class SanPhamUC : UserControl
    {
        DoanWPFEntities db = new DoanWPFEntities();

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
        }

        private void DgSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // có thể viết thêm để load thông tin sp khi chọn
        }

        private void DgSanPham_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sp = new DoAnWPF.SanPham()
                {
                    TenSanPham = txtTenSanPham.Text,
                    LoaiID = (int)cbLoaiSanPham.SelectedValue,
                    DonViID = (int)cbDonViTinh.SelectedValue,
                    SoLuongTon = int.Parse(txtSoLuongTon.Text),
                    GiaNhap = decimal.Parse(txtGiaNhap.Text),
                    GiaBan = decimal.Parse(txtGiaBan.Text),
                    //HinhAnh = txtHinhAnh.Text
                };

                db.SanPhams.Add(sp);
                db.SaveChanges();

                loadData();
                MessageBox.Show("Thêm sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

    }
}
