using System;
using PDF_Digital_Signature;

class Program
{
    static void Main(string[] args)
    {
        CertificateGenerator certGen = new CertificateGenerator();
        PdfSigner signer = new PdfSigner();

        while (true)
        {
            Console.WriteLine("\n===== MENU CHÍNH =====");
            Console.WriteLine("1. Tạo chứng chỉ số");
            Console.WriteLine("2. Ký file PDF với SHA-256");
            Console.WriteLine("3. Thoát");
            Console.Write("Chọn chức năng: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    certGen.CreateCertificate();
                    break;
                case "2":
                    string pdfPath = @"Data/test.pdf";
                    string certPath = @"Data/HieuDrakonCert.pfx";
                    signer.SignPdfWithSHA256(pdfPath, certPath);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("❌ Lựa chọn không hợp lệ.");
                    break;
            }
        }
    }
}
