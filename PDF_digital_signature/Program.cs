using System;
using PDF_Digital_Signature;

class Program
{
    static void Main(string[] args)
    {
        PdfSigner signer = new PdfSigner(); // Lớp PdfSigner đã có cả ký và xác nhận chữ ký

        while (true)
        {
            Console.WriteLine("\n===== MENU CHÍNH =====");
            Console.WriteLine("1. Tạo chứng chỉ số");
            Console.WriteLine("2. Ký file PDF với SHA-256");
            Console.WriteLine("3. Kiểm tra chữ ký số trong file PDF");
            Console.WriteLine("4. Thoát");
            Console.Write("Chọn chức năng: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Tạo chứng chỉ số
                    CertificateGenerator certGen = new CertificateGenerator();
                    certGen.CreateCertificate();
                    break;

                case "2":
                    // Ký file PDF
                    string pdfPath = @"Data/test.pdf";
                    string certPath = @"Data/HieuDrakonCert.pfx";
                    signer.SignPdfWithSHA256(pdfPath, certPath);
                    break;

                case "3":
                    // Kiểm tra chữ ký số
                    string signedPdfPath = @"test-đã ký.pdf"; // Mặc định là file đã ký
                    PdfSignatureVerifier verifier = new PdfSignatureVerifier();
                    bool isValid = verifier.VerifyPdfSignature(signedPdfPath);
                    if (isValid)
                    {
                        Console.WriteLine("✅ Xác nhận chữ ký thành công!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Xác nhận chữ ký thất bại!");
                    }
                    break;

                case "4":
                    Console.WriteLine("👋 Cảm ơn bạn đã sử dụng chương trình!");
                    return;

                default:
                    Console.WriteLine("❌ Lựa chọn không hợp lệ.");
                    break;
            }
        }
    }
}
