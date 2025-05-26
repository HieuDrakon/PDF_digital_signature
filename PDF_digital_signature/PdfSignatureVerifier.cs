using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.IO;

public class PdfSignatureVerifier
{
    public bool VerifyPdfSignature(string signedPdfPath)
    {
        if (!File.Exists(signedPdfPath))
        {
            Console.WriteLine("❌ Không tìm thấy file PDF đã ký.");
            return false;
        }

        using (FileStream documentStream = new FileStream(signedPdfPath, FileMode.Open, FileAccess.Read))
        {
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(documentStream);

            if (loadedDocument.Form == null || loadedDocument.Form.Fields.Count == 0)
            {
                Console.WriteLine("⚠️ Không tìm thấy trường chữ ký trong tài liệu.");
                return false;
            }

            foreach (PdfLoadedField field in loadedDocument.Form.Fields)
            {
                if (field is PdfLoadedSignatureField signatureField)
                {
                    Console.WriteLine($"🔍 Đang xác minh chữ ký: {signatureField.Name}");

                    PdfSignatureValidationResult validationResult = signatureField.ValidateSignature();

                    Console.WriteLine(validationResult.IsSignatureValid
                        ? "✅ Chữ ký hợp lệ!"
                        : "❌ Chữ ký không hợp lệ!");

                    Console.WriteLine($"📄 Toàn vẹn tài liệu: {(validationResult.IsDocumentModified ? "❌ Bị thay đổi" : "✅ Không bị thay đổi")}");
                    Console.WriteLine($"🔐 Trạng thái chứng thư: {validationResult.SignatureStatus}");
                    Console.WriteLine($"🔄 Thu hồi (OCSP): {validationResult.RevocationResult.OcspRevocationStatus}");
                    Console.WriteLine($"📌 Thuật toán băm: {validationResult.DigestAlgorithm}");

                    loadedDocument.Close(true);
                    return validationResult.IsSignatureValid && !validationResult.IsDocumentModified;
                }
            }

            loadedDocument.Close(true);
        }

        return false;
    }

}
