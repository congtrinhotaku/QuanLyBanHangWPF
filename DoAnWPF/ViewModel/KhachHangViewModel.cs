using DoAnWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.ViewModel
{
    internal class KhachHangViewModel
    {
        DoanWPFEntities db = new DoanWPFEntities();

        public void ThemKhachHang(KhachHang kh)
        {
            var existing = db.KhachHangs.FirstOrDefault(x => x.DienThoai == kh.DienThoai);
            if (existing != null)
            {
                return;
            }

            db.KhachHangs.Add(kh);
            db.SaveChanges();
        }


        public void XoaKhachHang(KhachHang khXoa)
        {
            var kh = db.KhachHangs.Find(khXoa.KhachHangID);
            if (kh != null)
            {
                bool coHoaDon = db.HoaDons.Any(h => h.KhachHangID == kh.KhachHangID);
                if (coHoaDon)
                {
                    MessageBox.Show("Không thể xóa khách hàng đã có hóa đơn!");
                    return;
                }

                db.KhachHangs.Remove(kh);
                db.SaveChanges();
            }
        }


        public void SuaKhachHang(KhachHang khCapNhat)
        {
            KhachHang kh = db.KhachHangs.Find(khCapNhat.KhachHangID);
            if (kh != null)
            {
                kh.TenKhach = khCapNhat.TenKhach;
                kh.DienThoai = khCapNhat.DienThoai;
                kh.DiaChi = khCapNhat.DiaChi;
                db.SaveChanges();
            }
        }

        public void LoadKhachHang(DataGrid dg)
        {
            dg.ItemsSource = db.KhachHangs.ToList();
        }

        public KhachHang FindKhachHang(int maKh)
        {
            return db.KhachHangs.Find(maKh);
        }
        public KhachHang GetByPhone(string sdt)
        {
            return db.KhachHangs.FirstOrDefault(k => k.DienThoai == sdt);
        }

        public List<KhachHang> GetAllKhachHang()
        {
            return db.KhachHangs.ToList();
        }
    }
}
