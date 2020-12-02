namespace AdsPortal.Domain.Models.AuthorImporter
{
    using System;
    using System.Linq;
    using CsvHelper.Configuration;

    public class PublicationsImporterRecordMap : ClassMap<PublicationsImporterRecord>
    {
        public PublicationsImporterRecordMap()
        {
            Map(x => x.No).Name("No");

            Map(x => x.Authors).Name("Authors").ConvertUsing((row) =>
            {
                string rawValue = row.GetField<string>("Authors");

                return rawValue.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => x.Trim())
                               .ToArray();
            });

            Map(x => x.Title).Name("Title");
            Map(x => x.Year).Name("Year");
            Map(x => x.Journal).Name("Journal");
            Map(x => x.ListedAuthors).Name("ListedAuthors");
            Map(x => x.TotalAuthors).Name("TotalAuthors");
        }
    }
}
