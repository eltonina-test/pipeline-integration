using CrossCutting.Core.Pdf;

namespace CrossCutting.PdfHelper
{
    public class PdfPositionPoint : IPdfPositionPoint
    {
        public PdfPositionPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        public double X { get; set; }
        public double Y { get; set; }
    }
}