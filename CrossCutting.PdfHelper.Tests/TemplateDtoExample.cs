using System;
using System.Collections.Generic;

namespace CrossCutting.PdfHelper.Tests.TemplateExample
{
    public class TemplateDtoExample
    {
        public string DoctorName { get; set; }
        public DateTime EpisodeDate { get; set; }
        public PatientDto Patient { get; set; }
        public IReadOnlyList<SectionDto> Sections { get; set; }
        public IReadOnlyList<FileDto> Files { get; set; }
    }

    public class PatientDto
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DocumentNumber { get; set; }
        public string Address { get; set; }
        public LegalRepresentativeDto LegalRepresentative { get; set; }
    }

    public class LegalRepresentativeDto
    {
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string Profession { get; set; }
        public string Address { get; set; }
    }

    public class SectionDto
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class FileDto
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
    }
}