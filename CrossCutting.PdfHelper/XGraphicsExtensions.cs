using System;
using System.Collections.Generic;
using System.Text;
using CrossCutting.Core.Pdf;
using PdfSharpCore.Drawing;

namespace CrossCutting.PdfHelper
{
    public static class XGraphicsExtensions
    {
        public static void DrawContent(this XGraphics graphic, IPdfFooterContent footerContent)
        {
            switch (footerContent.ContentType)
            {
                case PdfFooterContentType.String:
                    (footerContent as PdfFooterTextContent)?.DrawFooter(graphic);
                    break;

                case PdfFooterContentType.Image:
                    (footerContent as PdfFooterImageContent)?.DrawFooter(graphic);
                    break;

                case PdfFooterContentType.Line:
                    (footerContent as PdfFooterGeometryContent)?.DrawFooter(graphic);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
