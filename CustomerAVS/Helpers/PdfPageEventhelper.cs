using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CustomerAVS.Helpers
{
    public class PdfPageEventHelperCustom : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = 100;
            footerTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell(new Phrase("Página " + writer.PageNumber, FontFactory.GetFont(FontFactory.HELVETICA, 8)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER,
                PaddingTop = 10
            };

            footerTable.AddCell(cell);
            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
        }
    }
}
