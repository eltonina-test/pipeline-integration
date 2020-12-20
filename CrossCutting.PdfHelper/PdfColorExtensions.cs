using PdfSharpCore.Drawing;
using System.Reflection;
using CrossCutting.Core.Pdf;

namespace CrossCutting.PdfHelper
{
    public static class PdfColorExtensions
    {

        public static XSolidBrush GetXSolidBrushFromColor(this PdfCustomColors color)
        {
            return GetXSolidBrushFromColorName(color.ToString());
        }

        public static XPen GetXPensFromColor(this PdfCustomColors color)
        {
            return GetXPensFromColorName(color.ToString());
        }

        public static XSolidBrush GetXSolidBrushFromColorName(this string colorName)
        {
            var xBrushesType = typeof(XBrushes);
            
            var brushesProperty = xBrushesType.GetProperty(colorName, BindingFlags.Public | BindingFlags.Static);
            var xSolidBrushColor = brushesProperty?.GetValue(null, null);
         
            return xSolidBrushColor as XSolidBrush;
        }

        public static XPen GetXPensFromColorName(this string colorName)
        {
            var xPensType = typeof(XPens);

            var pensProperty = xPensType.GetProperty(colorName, BindingFlags.Public | BindingFlags.Static);
            var xPenColor = pensProperty?.GetValue(null, null);

            return xPenColor as XPen;
        }
    }
}
