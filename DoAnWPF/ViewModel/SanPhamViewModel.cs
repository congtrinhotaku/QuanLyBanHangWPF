using DoAnWPF.Model;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.ViewModel
{
    internal class SanPhamViewModel
    {
        DoanWPFEntities db = new DoanWPFEntities();

        public void ThemSanPham(SanPham sp)
        {
            db.SanPhams.Add(sp);
            db.SaveChanges();
        }

        public void XoaSanPham(SanPham spXoa)
        {
            var sp = db.SanPhams.Find(spXoa.SanPhamID);
            if (sp != null)
            {
                bool daCoHoaDon = db.ChiTietHoaDons.Any(ct => ct.SanPhamID == sp.SanPhamID);
                if (daCoHoaDon)
                {
                    MessageBox.Show("Không thể xóa sản phẩm đã có trong hóa đơn!");
                    return;
                }

                db.SanPhams.Remove(sp);
                db.SaveChanges();
            }
        }


        public void SuaSanPham(SanPham spCapNhat)
        {
            SanPham sp = db.SanPhams.Find(spCapNhat.SanPhamID);
            if (sp != null)
            {
                sp.TenSanPham = spCapNhat.TenSanPham;
                sp.LoaiID = spCapNhat.LoaiID;
                sp.DonViID = spCapNhat.DonViID;
                sp.GiaNhap = spCapNhat.GiaNhap;
                sp.GiaBan = spCapNhat.GiaBan;
                sp.SoLuongTon = spCapNhat.SoLuongTon;
                sp.HinhAnh = spCapNhat.HinhAnh;
                db.SaveChanges();
            }
        }

        public void LoadSanPham(DataGrid dg)
        {
            dg.ItemsSource = db.SanPhams.ToList();
        }

        public void LoadLoaiSanPham(ComboBox cb)
        {
            cb.ItemsSource = db.LoaiSanPhams.ToList();
            cb.DisplayMemberPath = "TenLoai";
            cb.SelectedValuePath = "LoaiID";
        }

        public void LoadDonViTinh(ComboBox cb)
        {
            cb.ItemsSource = db.DonViTinhs.ToList();
            cb.DisplayMemberPath = "TenDonVi";
            cb.SelectedValuePath = "DonViID";
        }

        public SanPham FindSanPham(int maSp)
        {
            return db.SanPhams.Find(maSp);
        }

        public List<SanPham> GetAllSanPham()
        {
            return db.SanPhams.ToList();
        }
    }
}
