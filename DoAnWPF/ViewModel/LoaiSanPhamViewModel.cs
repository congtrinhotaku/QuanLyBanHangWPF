using DoAnWPF.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.ViewModel
{
    public class LoaiSanPhamViewModel
    {
        private DoanWPFEntities db = new DoanWPFEntities();

        public List<LoaiSanPham> LoaiSanPhams { get; set; }

        public LoaiSanPhamViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            LoaiSanPhams = db.LoaiSanPhams.ToList();
        }

        public bool AddLoaiSanPham(string tenLoai)
        {
            if (db.LoaiSanPhams.Any(x => x.TenLoai == tenLoai))
            {
                MessageBox.Show("Loại sản phẩm này đã tồn tại!");
                return false;
            }

            var loai = new LoaiSanPham { TenLoai = tenLoai };
            db.LoaiSanPhams.Add(loai);
            db.SaveChanges();
            LoadData();
            return true;
        }

        public bool UpdateLoaiSanPham(int loaiId, string tenLoai)
        {
            var loai = db.LoaiSanPhams.FirstOrDefault(x => x.LoaiID == loaiId);
            if (loai == null)
            {
                MessageBox.Show("Không tìm thấy loại sản phẩm cần sửa.");
                return false;
            }

            loai.TenLoai = tenLoai;
            db.SaveChanges();
            LoadData();
            return true;
        }

        public bool DeleteLoaiSanPham(int loaiId)
        {
            var loai = db.LoaiSanPhams.FirstOrDefault(x => x.LoaiID == loaiId);
            if (loai == null)
            {
                MessageBox.Show("Không tìm thấy loại sản phẩm cần xóa.");
                return false;
            }

            // Kiểm tra ràng buộc FK
            if (loai.SanPhams.Any())
            {
                MessageBox.Show("Không thể xóa vì loại này đang có sản phẩm!");
                return false;
            }

            db.LoaiSanPhams.Remove(loai);
            db.SaveChanges();
            LoadData();
            return true;
        }

        public LoaiSanPham FindById(int loaiId)
        {
            return db.LoaiSanPhams.FirstOrDefault(x => x.LoaiID == loaiId);
        }

        public List<LoaiSanPham> GetAllLoaiSanPham()
        {
            return db.LoaiSanPhams.ToList();
        }

        public void LoadForcb(ComboBox cb)
        {
            cb.ItemsSource = db.LoaiSanPhams.ToList();
            cb.DisplayMemberPath = "TenLoai";
            cb.SelectedValuePath = "LoaiID";
        }
    }
}
