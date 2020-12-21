using System.Collections.Generic;
using System.Text;
using CrossCutting.Core.Pdf;
using PdfSharpCore.Drawing;

namespace CrossCutting.PdfHelper
{
    public class PdfFooterTextContent : PdfFooterContent, IPdfFooterTextContent
    {
        public string FontFamilyName { get; set; } = string.Empty;
        public double SizeEm { get; set; }
        public FontStyle FontStyle { get; set; }
        public PdfFooterTextAlignment TextAlignment { get; set; }
        public PdfFooterLineAlignment LineAlignment { get; set; }

        public override void DrawFooter(XGraphics gfx)
        {
            var format = new XStringFormat
            {
                Alignment = (XStringAlignment)TextAlignment,
                LineAlignment = (XLineAlignment)LineAlignment
            };

            var color = Color.GetXSolidBrushFromColor();
            var font = new XFont(FontFamilyName, SizeEm, (XFontStyle) FontStyle);

            // we need to refactor this - can be using the handlebars 
            var textContent = (string) Content;
            textContent = textContent.Replace("Page [{0}] of [{1}]", $"Page {PageNumber} of {TotalPages}");
            textContent = textContent.Replace("[{0}] • Visit History Record", $"{PatientName} • Visit History Record");

            if (UseBoxPosition)
            {
                var box = new XRect(PositionBox.PositionX, PositionBox.PositionY, PositionBox.Width, PositionBox.Height); 
                gfx.DrawString(textContent, font, color, box, format);
            }
            else
            {
                SetPositionWithBoxReference();
                gfx.DrawString(textContent, font, color, Position.X, Position.Y);
            }
        }

        // These properties need to be more generic, maybe Dictionary filled in origin and read here
        public string PatientName { get; set; }
        public double TotalPages { get; set; }
        public double PageNumber { get; set; }
    }
}
