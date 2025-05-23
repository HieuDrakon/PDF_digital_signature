using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Syncfusion.Drawing;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main(string[] args)
    {
        // Bước 1: Tải file PDF cần ký
        string pdfPath = @"Data/test.pdf";
        if (!File.Exists(pdfPath))
        {
            Console.WriteLine("Không tìm thấy file PDF.");
            return;
        }

        PdfLoadedDocument document = new PdfLoadedDocument(pdfPath);
        string newfileName = Path.GetFileNameWithoutExtension(pdfPath);

        // Bước 2: Tải chứng chỉ số .pfx
        string certPath = @"Data/HieuDrakonCert.pfx";
        if (!File.Exists(certPath))
        {
            Console.WriteLine("Không tìm thấy file chứng chỉ.");
            return;
        }

        string certPassword = "HieuTop1";

        // Hiển thị thông tin chứng chỉ
        X509Certificate2 x509 = new X509Certificate2(certPath, certPassword);
        Console.WriteLine("=== Thông tin chứng chỉ số ===");
        Console.WriteLine($"Chủ sở hữu (Subject): {x509.Subject}");
        Console.WriteLine($"Nhà phát hành (Issuer): {x509.Issuer}");
        Console.WriteLine($"Ngày bắt đầu hiệu lực: {x509.NotBefore}");
        Console.WriteLine($"Ngày hết hạn: {x509.NotAfter}");
        Console.WriteLine($"Số sê-ri: {x509.SerialNumber}");
        Console.WriteLine($"Thuật toán mã hóa khóa công khai: {x509.PublicKey.Oid.FriendlyName}");
        Console.WriteLine($"Độ dài khóa: {x509.PublicKey.Key.KeySize} bits");
        Console.WriteLine($"Địa chỉ email: {x509.GetNameInfo(X509NameType.EmailName, false)}");

        // Khởi tạo chứng chỉ cho Syncfusion
        PdfCertificate certificate = new PdfCertificate(certPath, certPassword);

        // Bước 3: Tạo chữ ký số trên trang đầu tiên
        PdfSignature signature = new PdfSignature(document, document.Pages[0], certificate, "HieuDrakon");

        // Bước 4: Cấu hình thuật toán băm SHA-256
        signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA256;

        // Bước 5 (tùy chọn): Hiển thị chữ ký
        signature.Bounds = new RectangleF(0, 0, 200, 50);
        signature.ContactInfo = "1050080013@sv.hcmunre.edu.vn";
        signature.LocationInfo = "Ho Chi Minh, Vietnam";
        signature.Reason = "Ký xác nhận tài liệu";
        signature.Certificated = true;

        // Bước 6: Lưu file đã ký
        newfileName = newfileName + "-đã ký.pdf";
        document.Save(newfileName);
        document.Close();

        Console.WriteLine("Đã ký file PDF thành công với SHA-256!");
    }
}
