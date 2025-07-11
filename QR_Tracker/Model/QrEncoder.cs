using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;
using System.Drawing;
using System.IO;

namespace QR_Tracker.Model
{
    public class QrEncoder
    {
        public static string SaveQrCode(string content, string fullPath)
        {
            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(fullPath)) // IsNullOrWhiteSpace문자열 검사
                return null;

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            var options = new EncodingOptions
            {
                Width = 200,
                Height = 200,
                Margin = 2
            };
            options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");  

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options
            };

            using (var bitmap = writer.Write(content))
            {
                bitmap.Save(fullPath, ImageFormat.Png);
            }

            return fullPath;
        }

    }
}