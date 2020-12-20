using System;
using Xunit;

namespace CrossCutting.PdfHelper.Tests
{
    public class When_generate_pdf_from_html_string
    {
        private readonly PdfCustomGenerator _pdfCustomGenerator;

        private const string LOGO = "<img width=24 src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAACXBIWXMAABYlAAAWJQFJUiTwAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAR1SURBVHgB1VlLUiNHEH3V3QjvbJ/AzQ3EmoGQTgAsbQ0e6QTIJ0A+wcAJJGKMWRqfoAU4wlvdAM0N2I7orprMan2Q+lfVqolg3oIQUikrPy9/LeA7h4BLPNx0oWRYekb4Exz9dgdHCOASQnyA8Frlh+SY/jgzwINTqJ+qzwiDM+Zwa4BCtXICb9gAgbDyjDI4YwF3BkRDc8/anK2AywiExicbjR/hCO4MCAJzr0q5B0dwZ4CySE7lv0UKidD8bPwGDVDKIgIihCO4TGILr3rOIrAaJaJhCL/RQjIbo92bwhae94vxWR/2Veh/0u9lt6m7fTK7Ix2f+e2VAf7OCTWijwgawH83UwrzhAaze/hijIPOpPoGHhEUjKBUWPo594kgaNLgd0wiSWnZREzyhVroyhG8XDdAeMdLBXS3pEuEOIGk/x//Imu9CWRyT6Qb4/BsnKOVGS2UeoYnprlK+zvduR6t+dmFcutn9ZnUADH/ckief4IVKDICZBTukXwZI9h5Kh/U1Jgc8y+Sl9Ei/LlKmyKe/cxy0ggw961BFyq0yIi+pl3uEfK2ENcUtREO3q9oyA7b2T0ninatqPcabDhFITWA5/g6QoqgFVdXSOLLpbcZj59adPNFajzft8U+NaeRqEefAiwUjzcU1x5vXJCPunAJolFQjz65II+/DDYUp2rywzmQ9K1GDVMQjYKt6aMwhZC9TGViughvmO7IblfvJYhGAV3wmV5NjZaRLMjrs6zX/V3med9lWq1BU5UqIOT9yjX8RAG4MNuqNNdPM15Puf5P2nwcI430Nb1a60PZ2HLoleim1CoQlHhttH+drr3/8Dd1cjl0urQvyjDkXX7zLCNndEvVKabnPN6HV1GZIPZOM8o/3hJl5ADOkNP0CmCWXUwvgWMqW72MQLfKM6//KPJ2HrYrDw+3Q6JNF9tCz0f4E+/eX1p+c4t9wJXyaf/Yq6M8o54BLpTnYgDZxmGnr2nJFawG7CnkgvNKXZPXV4pz6eWN7mW2X5W0m7B7uOtCeaEoSed0iW6aCFS0HDO8oE9/B7CAeQT0JOlFqIvN5rdQfr1vPFOl27dZac1ygMPMc01d6Obn76+U5wk4ozyDhr9dq3vMDEjHgxB1sNm5U85HxR2bdgUdbTNUG8C8rzvb5I0d6V4Qln6Po234ALjcAF3aaiZtnvL6JyiDpYYNTBO6EuUG6FDXACdsguzM5NG0awrPOzfpDcUGpN4KUQc8FrQ3niVFw6alPKOEzjeALbfx1hpk/kzT7k30Z1aoTuh8A3hPruN95v3h2aDwc/2ZvIINKhI634Cjzggx9tN5xRCa95S0VTg86+uHYsZyyxO6OAeYw0edPeOwa95vJG0R4i+nFs65gowLJ1WzUYK3M19GhfsyK6ONtUClTIqoT8vNwe+jMjFmnZg9u4yGyk6LJtTJk+lTqc2Txysljx4VyjPsx2m9K8uP9OpkLmGEd50e6iJdV1+Xyyu9I3xz8MUPN0/aoG3x+GmgZVnMQG7g8AfrurK+Al9M6rOhPHL7AAAAAElFTkSuQmCC'>";

        public When_generate_pdf_from_html_string()
        {
            _pdfCustomGenerator = new PdfCustomGenerator();
        }

        [Fact]
        public void Given_valid_html_string_should_create_pdf_stream()
        {
            var htmlTemplate = GetHtmlTextString(); 
            var tempStream = _pdfCustomGenerator.GeneratePdfFile(htmlTemplate); 

            Assert.NotNull(tempStream);
            Assert.True(tempStream.Length > 0);
        }

        [Fact]
        public void Given_invalid_html_string_should_not_create_pdf()
        {
            var htmlTemplate = "<invalid string>";

            Assert.Throws<InvalidOperationException>(() => _pdfCustomGenerator.GeneratePdfFile(htmlTemplate));
        }


        public string GetHtmlTextString()
        {
            var doctorInfo =
                LOGO +
                $"<h1>Test Header</h1>" +
                $"<p><span>Test Test</span></p>";

            var html =
                $"<html><header><title>Pdf Header</title></header>" +
                $"<body>{doctorInfo}</body>" +
                $"<footer>Pdf Footer</footer>";

            return html;
        }
    }
}
