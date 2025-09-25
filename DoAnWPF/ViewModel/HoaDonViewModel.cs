using DoAnWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DoAnWPF.ViewModel
{
    internal class HoaDonViewModel
    {
        private DoanWPFEntities db = new DoanWPFEntities();

        public List<HoaDon> LoadHoaDon()
        {
            return db.HoaDons
                     .Include("KhachHang")
                     .Include("ChiTietHoaDons.SanPham.DonViTinh")
                     .ToList();
        }

        public HoaDon FindHoaDon(int maHd)
        {
            return db.HoaDons
                     .Include("KhachHang")
                     .Include("ChiTietHoaDons.SanPham.DonViTinh")
                     .FirstOrDefault(h => h.HoaDonID == maHd);
        }

        public void XoaHoaDon(HoaDon hdXoa)
        {
            var hd = db.HoaDons.Include("ChiTietHoaDons")
                                .FirstOrDefault(h => h.HoaDonID == hdXoa.HoaDonID);
            if (hd != null)
            {
                db.ChiTietHoaDons.RemoveRange(hd.ChiTietHoaDons);
                db.HoaDons.Remove(hd);
                db.SaveChanges();
            }
        }


        public bool SuaHoaDon(int maHd, DateTime? ngayLap, string tenKhach)
        {
            var hd = db.HoaDons.Find(maHd);
            if (hd == null) return false;

            if (ngayLap.HasValue)
            {
                hd.NgayLap = ngayLap.Value;
            }

            if (!string.IsNullOrWhiteSpace(tenKhach))
            {
                var kh = db.KhachHangs.FirstOrDefault(k => k.TenKhach == tenKhach);
                if (kh != null)
                {
                    hd.KhachHangID = kh.KhachHangID;
                }
            }

            db.SaveChanges();
            return true;
        }

        public List<object> LoadChiTiet(int maHd)
        {
            var hd = db.HoaDons.Include("ChiTietHoaDons.SanPham.DonViTinh")
                               .FirstOrDefault(h => h.HoaDonID == maHd);
            if (hd == null) return new List<object>();

            return hd.ChiTietHoaDons.Select(ct => new
            {
                ct.SanPhamID,
                TenSanPham = ct.SanPham.TenSanPham,
                TenDonVi = ct.SanPham.DonViTinh.TenDonVi,
                ct.SoLuong,
                ct.DonGia,
                ct.ThanhTien
            }).ToList<object>();
        }

        public List<HoaDon> GetHoaDonByKhachHangId(int khachHangId)
        {
            return db.HoaDons
                     .Where(hd => hd.KhachHangID == khachHangId)
                     .ToList();
        }
    }
}
