using iTextSharp.text;
using iTextSharp.text.pdf;

namespace UI.Helpers;

public static class PdfFileHelper
{
    public static int GetPageCount(string filePath)
    {
        using var reader = new PdfReader(filePath);
        return reader.NumberOfPages;
    }

    public static byte[] GetPageContent(string filePath, int pageNumber)
    {
        using var reader = new PdfReader(filePath);
        using var document = new Document(reader.GetPageSizeWithRotation(pageNumber));
        using var stream = new MemoryStream();
        var writer = PdfWriter.GetInstance(document, stream);
        document.Open();
        var importedPage = writer.GetImportedPage(reader, pageNumber);
        var contentByte = writer.DirectContent;
        contentByte.AddTemplate(importedPage, 0, 0);
        document.Close();
        return stream.ToArray();
    }
}