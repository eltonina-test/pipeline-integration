using CrossCutting.Core.Pdf;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CrossCutting.PdfHelper
{
    public class PdfCustomFooterGenerator
    {
        private readonly Stream _pdfDocument;
        private readonly PdfFooterConfiguration _pdfFooterConfiguration;
 
        public string PatientName { get; set; }

        public PdfCustomFooterGenerator(Stream pdfDocument, PdfFooterConfiguration pdfFooterConfiguration)
        {
            _pdfDocument = pdfDocument;
            _pdfFooterConfiguration = pdfFooterConfiguration;
        }

        public Stream GetStreamWithFooter()
        {
            if (_pdfDocument == null) return null;

            var inputDocument = PdfReader.Open(_pdfDocument, PdfDocumentOpenMode.Import);

            var outputDocument = new PdfDocument();

            for (var pageIndex = 0; pageIndex < inputDocument.PageCount; pageIndex++)
            {
                var newPage = outputDocument.AddPage(inputDocument.Pages[pageIndex]);

                var gfx = XGraphics.FromPdfPage(newPage);
                var patientName = inputDocument.Info.Elements.GetString("/PatientName");
                // TODO change the name of the property PdfFooterTextContent ->  PdfFooterContents
                foreach (var content in _pdfFooterConfiguration.PdfFooterTextContent)
                { 
                    var box = newPage.MediaBox.ToXRect();
                    box.Inflate(content.InflatePositionBoxWidth, content.InflatePositionBoxHeight);
                  
                    content.SetBox(box.X, box.Y, box.Width, box.Height);

                    // TODO This will be refactored in next iteration
                    if (content.ContentType == PdfFooterContentType.String)
                    {
                        ((PdfFooterTextContent) content).PageNumber = pageIndex + 1;
                        ((PdfFooterTextContent) content).TotalPages = inputDocument.PageCount;
                        ((PdfFooterTextContent) content).PatientName = patientName;
                    }

                    gfx.DrawContent(content);
                }
            }

            var memoryStream = new MemoryStream();
            outputDocument.Save(memoryStream, false);

            return memoryStream;
        }

    }
}
