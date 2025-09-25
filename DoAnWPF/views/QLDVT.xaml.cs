using DoAnWPF.Model;
using DoAnWPF.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DoAnWPF.views
{
    public partial class QLDVT : UserControl
    {
        private DonViTinhViewModel dvtVM = new DonViTinhViewModel();
        private int selectedId = -1;

        public QLDVT()
        {
            InitializeComponent();
            LoadData();
            AddEventHandlers();
        }

        private void LoadData()
        {
            dvtVM.LoadDonVi(dtgDVT);
        }

        private void AddEventHandlers()
        {
            btnThemDVT.Click += BtnThemDVT_Click;
            btnXoaDVT.Click += BtnXoaDVT_Click;
            btnSuaDVT.Click += BtnSuaDVT_Click;
            dtgDVT.SelectionChanged += DtgDVT_SelectionChanged;
        }

        private void BtnThemDVT_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDonViTinh.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đơn vị tính!");
                return;
            }

            DonViTinh dv = new DonViTinh
            {
                TenDonVi = txtDonViTinh.Text.Trim()
            };

            dvtVM.ThemDonVi(dv);
            LoadData();
            txtDonViTinh.Clear();
        }

        private void BtnXoaDVT_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Vui lòng chọn một đơn vị tính để xóa!");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                dvtVM.XoaDonVi(new DonViTinh { DonViID = selectedId });
                LoadData();
                txtDonViTinh.Clear();
                selectedId = -1;
            }
        }

        private void BtnSuaDVT_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Vui lòng chọn một đơn vị tính để sửa!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDonViTinh.Text))
            {
                MessageBox.Show("Tên đơn vị tính không được để trống!");
                return;
            }

            DonViTinh dvUpdate = new DonViTinh
            {
                DonViID = selectedId,
                TenDonVi = txtDonViTinh.Text.Trim()
            };

            dvtVM.SuaDonVi(dvUpdate);
            LoadData();
            txtDonViTinh.Clear();
            selectedId = -1;
        }

        private void DtgDVT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgDVT.SelectedItem is DonViTinh dv)
            {
                selectedId = dv.DonViID;
                txtDonViTinh.Text = dv.TenDonVi;
            }
        }
        private void dtgDVT_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (dtgDVT.SelectedItem is DonViTinh dv)
            {
                selectedId = dv.DonViID;               // Lưu lại ID đơn vị tính
                txtDonViTinh.Text = dv.TenDonVi;       // Hiển thị vào TextBox
            }
        }
    }
}
