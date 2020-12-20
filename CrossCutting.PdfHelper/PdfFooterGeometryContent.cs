using CrossCutting.Core.Pdf;
using PdfSharpCore.Drawing;

namespace CrossCutting.PdfHelper
{
    public class PdfFooterGeometryContent : PdfFooterContent, IPdfFooterGeometryContent
    {
        public override void DrawFooter(XGraphics gfx)
        {
            var color = Color.ToString().GetXPensFromColorName();

            // We can add other geometries and make strategy 
            // if (ContentType == PdfFooterContentType.Line)

            gfx.DrawLine(color, PositionBox.BottomLeft.X, PositionBox.BottomLeft.Y, PositionBox.BottomRight.X,
                PositionBox.BottomRight.Y);
        }
    }
}