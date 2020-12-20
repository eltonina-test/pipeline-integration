using System.Collections.Generic;
using System.Text;
using CrossCutting.Core.Pdf;
using PdfSharpCore.Drawing;

namespace CrossCutting.PdfHelper
{
    public abstract class PdfFooterContent : IPdfFooterContent
    { 
        public void SetBox(double positionX, double positionY, double width, double height)
        {
            PositionBox = new PdfPositionBox(positionX, positionY, width, height);
        }

        public void SetBoxOffset(PdfPositionBoxEnum pdfPositionBox, double offsetX, double offsetY)
        {
            PositionBoxOffset = new PdfPositionBoxOffset
            {
                Position = pdfPositionBox,
                OffSetX = offsetX,
                OffSetY = offsetY,
            };
        }

        public void SetPosition(double newPositionX, double newPositionY)
        {
            Position = new PdfPositionPoint(newPositionX, newPositionY);
        }

        public void SetPositionWithBoxReference()
        {
            var newPositionX = PositionBox.PositionX;
            var newPositionY = PositionBox.PositionY;

            if (PositionBoxOffset != null)
            {
                switch (PositionBoxOffset.Position)
                {
                    case PdfPositionBoxEnum.TopLeft:
                        newPositionX = PositionBox.TopLeft.X + PositionBoxOffset.OffSetX;
                        newPositionY = PositionBox.TopLeft.Y + PositionBoxOffset.OffSetY;
                        break;

                    case PdfPositionBoxEnum.TopCenter:
                        newPositionX = PositionBox.TopCenter.X + PositionBoxOffset.OffSetX;
                        newPositionY = PositionBox.TopCenter.Y + PositionBoxOffset.OffSetY;
                        break;

                    case PdfPositionBoxEnum.TopRight:
                        newPositionX = PositionBox.TopRight.X + PositionBoxOffset.OffSetX;
                        newPositionY = PositionBox.TopRight.Y + PositionBoxOffset.OffSetY;
                        break;

                    case PdfPositionBoxEnum.BottomLeft:
                        newPositionX = PositionBox.BottomLeft.X + PositionBoxOffset.OffSetX;
                        newPositionY = PositionBox.BottomLeft.Y + PositionBoxOffset.OffSetY;
                        break;

                    case PdfPositionBoxEnum.BottomCenter:
                        newPositionX = PositionBox.BottomCenter.X + PositionBoxOffset.OffSetX;
                        newPositionY = PositionBox.BottomCenter.Y + PositionBoxOffset.OffSetY;
                        break;

                    case PdfPositionBoxEnum.BottomRight:
                        newPositionX = PositionBox.BottomRight.X + PositionBoxOffset.OffSetX;
                        newPositionY = PositionBox.BottomRight.Y + PositionBoxOffset.OffSetY;
                        break;

                    case PdfPositionBoxEnum.MiddleLeft:
                        newPositionX = PositionBox.MiddleLeft.X + PositionBoxOffset.OffSetX;
                        newPositionY = PositionBox.MiddleLeft.Y + PositionBoxOffset.OffSetY;
                        break;

                    case PdfPositionBoxEnum.MiddleCenter:
                        newPositionX = PositionBox.MiddleCenter.X + PositionBoxOffset.OffSetX;
                        newPositionY = PositionBox.MiddleCenter.Y + PositionBoxOffset.OffSetY;
                        break;

                    case PdfPositionBoxEnum.MiddleRight:
                        newPositionX = PositionBox.MiddleRight.X + PositionBoxOffset.OffSetX;
                        newPositionY = PositionBox.MiddleRight.Y + PositionBoxOffset.OffSetY;
                        break;

                    default:
                        break;
                }
            }

            SetPosition(newPositionX, newPositionY);
        }
         
        public PdfFooterContentType ContentType { get; set; }
        public object Content { get; set; }
        public PdfCustomColors Color { get; set; } 

        public bool UseBoxPosition { get; set; } = true;
        public double InflatePositionBoxWidth { get; set; }
        public double InflatePositionBoxHeight { get; set; }
        public IPdfPositionPoint Position { get; set; } 
        public IPdfPositionBox PositionBox { get; private set; }
        public IPdfPositionBoxOffset PositionBoxOffset { get; private set; }

        public abstract void DrawFooter(XGraphics gfx);
    }
}
