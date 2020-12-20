using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CrossCutting.PdfHelper.Tests
{
    public class When_merge_pdfs_files : IDisposable
    {
        private Stream _resource;
        private readonly string _testFileName = "test{0}.pdf";
        private readonly PdfCustomGenerator _pdfCustomGenerator;

        public When_merge_pdfs_files()
        {
            _pdfCustomGenerator = new PdfCustomGenerator();
        }

        [Fact]
        public async Task Given_two_valid_pdf_streams_should_merge_in_one_stream()
        {
            var fileName = string.Format(_testFileName, nameof(Given_two_valid_pdf_streams_should_merge_in_one_stream));
            var streams = new List<Stream>
            {
                GetStream(),
                GetStream()
            };

            var result = _pdfCustomGenerator.MergePdfFiles(streams);

            _resource = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            await result.CopyToAsync(_resource);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
            Assert.True(File.Exists(fileName));

            Dispose(fileName);
        }

        [Fact]
        public async Task Given_one_valid_pdf_stream_should_one_stream()
        {
            var fileName = string.Format(_testFileName, nameof(Given_one_valid_pdf_stream_should_one_stream));
            var streams = new List<Stream>
            {
                GetStream()
            };

            var result = _pdfCustomGenerator.MergePdfFiles(streams);

            _resource = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            await result.CopyToAsync(_resource);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
            Assert.True(File.Exists(fileName));

            Dispose(fileName);
        }

        [Fact]
        public async Task Given_ten_valid_pdf_streams_should_merge_in_one_stream()
        {
            var fileName = string.Format(_testFileName, nameof(Given_ten_valid_pdf_streams_should_merge_in_one_stream));

            var streams = new List<Stream>();
            Enumerable.Range(1, 10).ToList().ForEach(i => streams.Add(GetStream()));
             
            var result = _pdfCustomGenerator.MergePdfFiles(streams);

            _resource = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            await result.CopyToAsync(_resource);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
            Assert.True(File.Exists(fileName));

            Dispose(fileName);
        }

        [Fact]
        public void Given_empty_list_of_pdf_streams_should_return_null()
        {
            var streams = new List<Stream>();

            Assert.Throws<ArgumentException>(() => _pdfCustomGenerator.MergePdfFiles(streams));
        }


        private Stream GetStream()
        {
            var htmlTemplate = GetHtmlTextString();

            var tempStream = _pdfCustomGenerator.GeneratePdfFile(htmlTemplate);
            byte[] bytes = new byte[tempStream.Length];
            tempStream.Read(bytes, 0, (int)tempStream.Length);

            return new MemoryStream(bytes);
        }

        private string GetHtmlTextString()
        {
            var doctorInfo =
                $"<h1>Test Header</h1>" +
                $"<p><span>Test Test</span></p>";

            var html =
                $"<html><header><title>Pdf Header</title></header>" +
                $"<body>{doctorInfo}</body>" +
                $"<footer>Pdf Footer</footer>";

            return html;
        }

        public void Dispose()
        {
            if (_resource != null)
                _resource.Dispose();
        }
         
        public void Dispose(string fileName)
        {
            _resource.Dispose();
            File.Delete(fileName);
        }
    }
}
