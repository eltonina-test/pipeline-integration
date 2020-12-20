using CrossCutting.Core.Pdf;
using CrossCutting.PdfHelper.HtmlRenderer.PdfSharp;
using Microsoft.AspNetCore.StaticFiles;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace CrossCutting.PdfHelper
{
    public class PdfCustomGenerator : IMergePdf, IGeneratePdf
    {
        private const string MimePdf = "application/pdf";

        public Stream GeneratePdfFile(string html)
        {
            var config = new PdfConfig()
            {
                MarginBottom = 70,
                MarginLeft = 20,
                MarginRight = 20,
                MarginTop = 70,
            };

            return GeneratePdfFile(html, config);
        }

        public Stream GeneratePdfFile(string html, PdfConfig pdfConfig)
        {
            var ms = new MemoryStream();

            var config = new PdfGenerateConfig()
            {
                MarginBottom = (int) pdfConfig.MarginBottom,
                MarginLeft = (int) pdfConfig.MarginLeft,
                MarginRight = (int) pdfConfig.MarginRight,
                MarginTop = (int) pdfConfig.MarginTop,
                PageSize = PageSize.A4,
                PageOrientation = PageOrientation.Portrait
            };

            var pdf = PdfGenerator.GeneratePdf(html, config);

            pdf.Info.Title = pdfConfig.PdfDocumentInfo?.Title;
            pdf.Info.Subject = pdfConfig.PdfDocumentInfo?.Subject;
            pdf.Info.CreationDate = DateTime.UtcNow;
            pdf.Info.Elements.TryAdd("/DoctorName", new PdfString(pdfConfig.PdfDocumentInfo?.DoctorName));
            pdf.Info.Elements.TryAdd("/PatientName", new PdfString(pdfConfig.PdfDocumentInfo?.PatientName));

            pdf.Save(ms, false);

            if (pdfConfig.Footer == null) return ms;

            var footerGenerator = new PdfCustomFooterGenerator(ms, pdfConfig.Footer)
            {
                PatientName = pdfConfig.PdfDocumentInfo?.PatientName
            };

            return footerGenerator.GetStreamWithFooter();

        }

        public Stream MergePdfFiles(List<Stream> files, PdfFooterConfiguration footerConfiguration = null)
        {
            if (files == null || files.Count <= 0) throw new ArgumentException(nameof(files));

            IList<PdfDocument> inputDocuments = new List<PdfDocument>();
            files.ForEach(file => inputDocuments.Add(PdfReader.Open(file, PdfDocumentOpenMode.Import)));

            var documentResult = new PdfDocument();
            var memoryStream = new MemoryStream();
             
            foreach (var document in inputDocuments)
            {
                for (var pageIndex = 0; pageIndex < document.PageCount; pageIndex++)
                {
                    documentResult.AddPage(document.Pages[pageIndex]);
                }  
                SetInfo(document, documentResult, "/DoctorName", "/PatientName");
            }

            documentResult.Save(memoryStream, false);

            if (footerConfiguration == null) return memoryStream;

            var footerGenerator = new PdfCustomFooterGenerator(memoryStream, footerConfiguration);
            return footerGenerator.GetStreamWithFooter();
        }

        private static void SetInfo(PdfDocument srcDocument, PdfDocument trgDocument, params string[] elementNames)
        {
            foreach (var elementName in elementNames)
            {
                var element = srcDocument.Info.Elements.GetString(elementName);
                if (string.IsNullOrEmpty(element)) continue;
                trgDocument.Info.Elements.TryAdd(elementName, new PdfString(element));
            }
        }

        public bool IsPdfFile(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType == MimePdf;
        }
    }
}
