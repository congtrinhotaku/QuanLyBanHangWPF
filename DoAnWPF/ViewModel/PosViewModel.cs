using DoAnWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoAnWPF.ViewModel
{
    internal class PosViewModel
    {
        private DoanWPFEntities db = new DoanWPFEntities();

        public List<CartItem> Cart { get; set; } = new List<CartItem>();

        public List<SanPham> LoadSanPham()
        {
            return db.SanPhams.ToList();
        }

     
        public List<SanPham> LoadSanPhamByLoai(int loaiId)
        {
            return db.SanPhams.Where(sp => sp.LoaiID == loaiId).ToList();
        }

        public List<LoaiSanPham> LoadLoaiSanPham()
        {
            return db.LoaiSanPhams.ToList();
        }

     
        public void AddToCart(SanPham sp, int soLuong = 1)
        {
            var item = Cart.FirstOrDefault(c => c.SanPhamID == sp.SanPhamID);
            if (item == null)
            {
                Cart.Add(new CartItem
                {
                    SanPhamID = sp.SanPhamID,
                    Name = sp.TenSanPham,
                    Price = sp.GiaBan,
                    Quantity = soLuong
                });
            }
            else
            {
                item.Quantity += soLuong;
            }
        }

       
        public void DecreaseFromCart(int sanPhamId)
        {
            var item = Cart.FirstOrDefault(c => c.SanPhamID == sanPhamId);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    Cart.Remove(item);
                }
            }
        }

     
        public decimal GetTotal()
        {
            return Cart.Sum(c => c.Total);
        }

      
        public void ThanhToan(int khachHangId)
        {
            if (Cart.Count == 0) return;

            HoaDon hd = new HoaDon
            {
                KhachHangID = khachHangId,
                NgayLap = DateTime.Now,
                TongTien = GetTotal(),
                NguoiLap = "chủ cửa hàng"
            };

            db.HoaDons.Add(hd);
            db.SaveChanges();

            foreach (var item in Cart)
            {
                ChiTietHoaDon ct = new ChiTietHoaDon
                {
                    HoaDonID = hd.HoaDonID,
                    SanPhamID = item.SanPhamID,
                    SoLuong = item.Quantity,
                    DonGia = item.Price,
                    ThanhTien = item.Total
                };
                db.ChiTietHoaDons.Add(ct);
            }

            db.SaveChanges();
            Cart.Clear(); 
        }
    }


    internal class CartItem
    {
        public int SanPhamID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    }
}
