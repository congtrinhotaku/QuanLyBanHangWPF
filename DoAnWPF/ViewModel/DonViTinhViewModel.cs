using DoAnWPF.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace DoAnWPF.ViewModel
{
    internal class DonViTinhViewModel
    {
        private DoanWPFEntities db = new DoanWPFEntities();

        // Lấy danh sách tất cả đơn vị tính
        public List<DonViTinh> GetAll()
        {
            return db.DonViTinhs.ToList();
        }

        // Thêm đơn vị tính
        public void ThemDonVi(DonViTinh dv)
        {
            db.DonViTinhs.Add(dv);
            db.SaveChanges();
        }

        // Xóa đơn vị tính
        public void XoaDonVi(DonViTinh dvXoa)
        {
            DonViTinh dv = db.DonViTinhs.Find(dvXoa.DonViID);
            if (dv != null)
            {
                db.DonViTinhs.Remove(dv);
                db.SaveChanges();
            }
        }

        // Sửa đơn vị tính
        public void SuaDonVi(DonViTinh dvCapNhat)
        {
            DonViTinh dv = db.DonViTinhs.Find(dvCapNhat.DonViID);
            if (dv != null)
            {
                dv.TenDonVi = dvCapNhat.TenDonVi;
                db.SaveChanges();
            }
        }

        // Load vào DataGrid
        public void LoadDonVi(DataGrid dg)
        {
            dg.ItemsSource = db.DonViTinhs.ToList();
        }

        // Load vào ComboBox (dùng khi chọn đơn vị tính cho sản phẩm)
        public void LoadDonViToComboBox(ComboBox cb)
        {
            cb.ItemsSource = db.DonViTinhs.ToList();
            cb.DisplayMemberPath = "TenDonVi";
            cb.SelectedValuePath = "DonViID";
        }

        // Tìm đơn vị theo ID
        public DonViTinh FindDonVi(int id)
        {
            return db.DonViTinhs.Find(id);
        }
    }
}
