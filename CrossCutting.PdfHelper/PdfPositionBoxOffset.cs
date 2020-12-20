using CrossCutting.Core.Pdf;

namespace CrossCutting.PdfHelper
{
    public class PdfPositionBoxOffset : IPdfPositionBoxOffset
    {
        public PdfPositionBoxEnum Position { get; set; }
        public double OffSetX { get; set; }
        public double OffSetY { get; set; }
    }
}