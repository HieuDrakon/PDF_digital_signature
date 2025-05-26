using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PDF_Digital_Signature
{
    public class CertificateGenerator
    {
        public string CreateCertificate(string filePath = "Data/HieuDrakonCert.pfx", string password = "HieuTop1")
        {
            using RSA rsa = RSA.Create(2048);

            var request = new CertificateRequest(
                "CN=Nguyễn Hoàng Trung Hiếu - 1050080013,OU=10_ĐH_CNPM1, C=VN",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));

            var certificate = request.CreateSelfSigned(
                DateTimeOffset.UtcNow.AddDays(-1),
                DateTimeOffset.UtcNow.AddYears(5));

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            File.WriteAllBytes(filePath, certificate.Export(X509ContentType.Pfx, password));

            Console.WriteLine("Đã tạo chứng chỉ số: " + filePath);
            return filePath;
        }
    }
}
