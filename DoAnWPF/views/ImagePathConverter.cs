using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DoAnWPF.views
{
    public class ImagePathConverter : IValueConverter
    {
        // Convert relative DB path (ví dụ "uploads/abc.png") -> BitmapImage (absolute path)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string relativePath = value as string;
            if (string.IsNullOrEmpty(relativePath))
                return null;

            try
            {
                // Lấy thư mục gốc project (lên 2 cấp từ bin\Debug\...)
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var projectDir = Directory.GetParent(baseDir).Parent.Parent.FullName;

                // Chuẩn hóa đường dẫn (thay / bằng \ nếu cần)
                string normalized = relativePath.Replace('/', Path.DirectorySeparatorChar);

                string absolutePath = Path.Combine(projectDir, normalized);

                if (!File.Exists(absolutePath))
                    return null;

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(absolutePath, UriKind.Absolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                bmp.Freeze();
                return bmp;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
