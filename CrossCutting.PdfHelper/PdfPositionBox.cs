using System;
using CrossCutting.Core.Pdf;

namespace CrossCutting.PdfHelper
{
    public class PdfPositionBox : IPdfPositionBox
    {
        public double PositionX { get; }
        public double PositionY { get; }
        public double Width { get; }
        public double Height { get; }

        public double Left => PositionX;
        public double Right => PositionX + Width;

        public double Top => PositionY;
        public double Bottom => (PositionY + Height);

        public double Center => Right / 2;
        public double Middle => Bottom / 2;

        public IPdfPositionPoint TopLeft => new PdfPositionPoint(Left, Top);
        public IPdfPositionPoint TopCenter => new PdfPositionPoint(Center, Top);
        public IPdfPositionPoint TopRight => new PdfPositionPoint(Right, Top);
        public IPdfPositionPoint BottomLeft => new PdfPositionPoint(Left, Bottom);
        public IPdfPositionPoint BottomCenter => new PdfPositionPoint(Center, Bottom);
        public IPdfPositionPoint BottomRight => new PdfPositionPoint(Right, Bottom);
        public IPdfPositionPoint MiddleLeft => new PdfPositionPoint(Left, Middle);
        public IPdfPositionPoint MiddleCenter => new PdfPositionPoint(Center, Middle);
        public IPdfPositionPoint MiddleRight => new PdfPositionPoint(Right, Middle);


        public PdfPositionBox(double positionX, double positionY, double width, double height)
        {
            PositionX = positionX;
            PositionY = positionY;
            Width = width;
            Height = height;
        }
    }
}