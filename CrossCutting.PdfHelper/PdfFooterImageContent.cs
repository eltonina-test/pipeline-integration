using System.IO;
using CrossCutting.Core.Pdf;
using PdfSharpCore.Drawing;

namespace CrossCutting.PdfHelper
{
    public class PdfFooterImageContent : PdfFooterContent, IPdfFooterImageContent
    {
        public double ImageWidth { get; set; }
        public double ImageHeight { get; set; }
        public double PixelWidth { get; private set; }
        public double PixelHeight { get; private set; }
        public double OffsetWidth { get; private set; }
        public double OffsetHeight { get; private set; } 
        
        public void SetPixelsImageOffset(double pixelWidthOffset = 1, double pixelHeightOffset = 1)
        {
            OffsetWidth = pixelWidthOffset;
            OffsetHeight = pixelHeightOffset;
        }

        public void SetPixelsImage(double pixelWidth, double pixelHeight)
        {
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;
        }

        public void TransformImage(double imageResolution)
        {
            if (imageResolution == 0) return;

            ImageWidth = PixelWidth * OffsetWidth / imageResolution;
            ImageHeight = PixelHeight * OffsetHeight / imageResolution;
        }

        public override void DrawFooter(XGraphics gfx)
        {
            var bytes = (byte[]) Content;
            var ms = new MemoryStream(bytes);

            var image = XImage.FromStream(() => ms);

            SetPixelsImage(image.PixelWidth, image.PixelHeight);
            TransformImage(image.HorizontalResolution);

            if (!UseBoxPosition)
            {
                SetPositionWithBoxReference();
            }
            else
            {
                SetPosition(PositionBox.PositionX, PositionBox.PositionY);
            }

            gfx.DrawImage(image, Position.X, Position.Y, ImageWidth, ImageHeight);
        }
    }
}