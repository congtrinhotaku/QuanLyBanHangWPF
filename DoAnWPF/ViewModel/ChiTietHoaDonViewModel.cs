using DoAnWPF.Model;
using System.Linq;
using System.Windows.Controls;

namespace DoAnWPF.ViewModel
{
    internal class ChiTietHoaDonViewModel
    {
        DoanWPFEntities db = new DoanWPFEntities();

        public void ThemChiTiet(ChiTietHoaDon ct)
        {
            db.ChiTietHoaDons.Add(ct);
            db.SaveChanges();
        }

        public void XoaChiTiet(ChiTietHoaDon ctXoa)
        {
            ChiTietHoaDon ct = db.ChiTietHoaDons.Find(ctXoa.CTHD_ID);
            if (ct != null)
            {
                db.ChiTietHoaDons.Remove(ct);
                db.SaveChanges();
            }
        }

        public void SuaChiTiet(ChiTietHoaDon ctCapNhat)
        {
            ChiTietHoaDon ct = db.ChiTietHoaDons.Find(ctCapNhat.CTHD_ID);
            if (ct != null)
            {
                ct.HoaDonID = ctCapNhat.HoaDonID;
                ct.SanPhamID = ctCapNhat.SanPhamID;
                ct.SoLuong = ctCapNhat.SoLuong;
                ct.DonGia = ctCapNhat.DonGia;
                ct.ThanhTien = ctCapNhat.ThanhTien;
                db.SaveChanges();
            }
        }

        public void LoadChiTiet(DataGrid dg)
        {
            dg.ItemsSource = db.ChiTietHoaDons.ToList();
        }

        public void LoadHoaDon(ComboBox cb)
        {
            cb.ItemsSource = db.HoaDons.ToList();
            cb.DisplayMemberPath = "HoaDonID"; // hoặc hiển thị Ngày lập
            cb.SelectedValuePath = "HoaDonID";
        }

        public void LoadSanPham(ComboBox cb)
        {
            cb.ItemsSource = db.SanPhams.ToList();
            cb.DisplayMemberPath = "TenSanPham";
            cb.SelectedValuePath = "SanPhamID";
        }

        public ChiTietHoaDon FindChiTiet(int maCt)
        {
            return db.ChiTietHoaDons.Find(maCt);
        }
    }
}
