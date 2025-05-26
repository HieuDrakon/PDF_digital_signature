using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Syncfusion.Drawing;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace PDF_Digital_Signature
{
    public class PdfSigner
    {
        public void SignPdfWithSHA256(string pdfPath, string certPath, string certPassword = "HieuTop1")
        {
            if (!File.Exists(pdfPath))
            {
                Console.WriteLine("❌ Không tìm thấy file PDF.");
                return;
            }

            if (!File.Exists(certPath) || string.IsNullOrEmpty(certPath))
            {
                Console.WriteLine("❌ Không tìm thấy chứng chỉ.");
                return;
            }

            // Load tài liệu
            PdfLoadedDocument document = new PdfLoadedDocument(pdfPath);
            string newfileName = Path.GetFileNameWithoutExtension(pdfPath) + "-đã ký.pdf";

            // Load chứng chỉ
            X509Certificate2 x509 = new X509Certificate2(certPath, certPassword);
            Console.WriteLine("=== Thông tin chứng chỉ số ===");
            Console.WriteLine($"Chủ sở hữu: {x509.Subject}");
            Console.WriteLine($"Hiệu lực: {x509.NotBefore} → {x509.NotAfter}");

            PdfCertificate certificate = new PdfCertificate(certPath, certPassword);

            // Tạo chữ ký
            PdfSignature signature = new PdfSignature(document, document.Pages[0], certificate, "HieuDrakon")
            {
                Bounds = new RectangleF(0, 0, 200, 50),
                ContactInfo = "1050080013@sv.hcmunre.edu.vn",
                LocationInfo = "Ho Chi Minh, Vietnam",
                Reason = "Ký xác nhận tài liệu",
                Certificated = true
            };

            signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA256;

            // Lưu file đã ký
            document.Save(newfileName);
            document.Close();

            // Tính hash SHA-256
            byte[] fileBytes = File.ReadAllBytes(newfileName);
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(fileBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            Console.WriteLine("🔐 SHA-256 của file đã ký:");
            Console.WriteLine(sb.ToString());
            Console.WriteLine($"✅ File đã ký được lưu tại: {newfileName}");
        }
    }
}
