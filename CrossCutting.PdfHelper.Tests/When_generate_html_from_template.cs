using AutoFixture;
using CrossCutting.Core.Pdf;
using CrossCutting.Core.Templating;
using CrossCutting.PdfHelper.Tests.TemplateExample;
using CrossCutting.Templating.Handlebars;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CrossCutting.PdfHelper.Tests
{
    public class When_generate_html_from_template
    {
        private Stream _resource = null;
        private readonly PdfCustomGenerator _pdfCustomGenerator;
        private readonly PdfConfig _pdfMargin;

        public When_generate_html_from_template()
        {
            _pdfCustomGenerator = new PdfCustomGenerator();

            _pdfMargin = new PdfConfig()
            {
                MarginTop = 40,
                MarginBottom = 60,
                MarginLeft = 20,
                MarginRight = 20, 
                PdfDocumentInfo = new PdfDocumentInfo()
                {
                    Title = "DocPlanner document",
                    Subject = "A DocPlanner document",
                    Author = "DocPlanner",
                    DoctorName = "Doctor Dolittle",
                    PatientName = "Donald the Duck"
                }
            };
        }

        [Fact]
        public void Given_list_of_example_should_create_list_of_streams_files()
        {
            var template = new HandlebarsTemplateBase();
            var fileString = File.ReadAllText(@"TemplateExample.html");
            var rawTemplate = template.GetTemplate(fileString);

            var fixture = new Fixture();
            var data = fixture.CreateMany<TemplateDtoExample>();
            var streams = GetListStream(data.ToList(), rawTemplate).ToList();

            Assert.NotNull(streams);
            Assert.True(streams.Count() == data.Count());
        }

        [Fact]
        public async Task Given_list_of_examples_should_merge_all_streams_in_one_file()
        {
            var fixture = new Fixture();
            var fileName = "testFileSummed.pdf";

            var template = new HandlebarsTemplateBase();
            var fileString = File.ReadAllText(@"TemplateExample.html");
            var rawTemplate = template.GetTemplate(fileString);

            var data = fixture.CreateMany<TemplateDtoExample>();
            var streams = GetListStream(data.ToList(), rawTemplate).ToList();
            var result = _pdfCustomGenerator.MergePdfFiles(streams);

            _resource = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            await result.CopyToAsync(_resource);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
            Assert.True(File.Exists(fileName));

            Dispose(fileName);
        }


        [Fact]
        public async Task Given_list_of_examples_should_merge_all_streams_in_one_file_handlebar_format()
        {
            var fixture = new Fixture();
            var fileName = "testFileSummed.pdf";

            var template = new HandlebarsTemplateBase();
            var fileString = File.ReadAllText(@"TemplateExampl2.html");
            var rawTemplate = template.GetTemplate(fileString);

            var data = fixture.CreateMany<TemplateDtoExample>();
            var footer = GetDocPlannerFooter();

            var streams = GetListStream(data.ToList(), rawTemplate).ToList();
            var result = _pdfCustomGenerator.MergePdfFiles(streams, footer);

            _resource = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            await result.CopyToAsync(_resource);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
            Assert.True(File.Exists(fileName));

            Dispose(fileName);
        }

        private IEnumerable<Stream> GetListStream(IReadOnlyList<TemplateDtoExample> data, IRawTemplate template, PdfFooterConfiguration footer = null)
        {
            return data
                .Select(html => template.Apply(html))
                .Select(htmlResult => GetStream(htmlResult, footer));
        }

        private PdfFooterConfiguration GetDocPlannerFooter()
        {
            IList<IPdfFooterContent> footerContents = new List<IPdfFooterContent>();
            footerContents.Add(PdfFooterContentFactory.LineSeparator);
            footerContents.Add(PdfFooterContentFactory.PatientNameContent);
            footerContents.Add(PdfFooterContentFactory.PageNumberContent);

            var poweredBy = PdfFooterContentFactory.PoweredByDoctoriaContent;
            poweredBy.SetBoxOffset(PdfPositionBoxEnum.BottomRight, -150, -2);
            footerContents.Add(poweredBy);

            var logo = PdfFooterContentFactory.LogoContent;
            logo.SetBoxOffset(PdfPositionBoxEnum.BottomRight, -20, -15);
            logo.SetPixelsImageOffset(30, 30);
            footerContents.Add(logo);

            return new PdfFooterConfiguration(footerContents);
        }

        private Stream GetStream(string html, PdfFooterConfiguration footer)
        {
            _pdfMargin.Footer = footer;

            var tempStream = _pdfCustomGenerator.GeneratePdfFile(html, _pdfMargin);
            byte[] bytes = new byte[tempStream.Length];
            tempStream.Read(bytes, 0, (int) tempStream.Length);

            return new MemoryStream(bytes);
        }

        private void Dispose(string fileName)
        {
            _resource.Close();
            if (File.Exists(fileName)) File.Delete(fileName);

            Dispose();
        }

        private void Dispose()
        {
        }
    }
}
