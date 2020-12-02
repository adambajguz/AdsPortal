namespace AdsPortal.Domain.Models.AuthorImporter
{
    using CsvHelper.Configuration;

    public class AuthorsImporterRecordMap : ClassMap<AuthorsImporterRecord>
    {
        public AuthorsImporterRecordMap()
        {
            Map(x => x.No).Name("No");
            Map(x => x.Name).Name("Name");
            Map(x => x.Surname).Name("Surname");
            Map(x => x.ORCID).Name("ORCID");
            Map(x => x.Department).Name("Department");
            Map(x => x.Degree).Name("Degree");
        }
    }
}
