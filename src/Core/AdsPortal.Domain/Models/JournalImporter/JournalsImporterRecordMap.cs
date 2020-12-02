namespace AdsPortal.Domain.Models.JournalsImporter
{
    using System.Collections.Generic;
    using System.Linq;
    using CsvHelper.Configuration;

    public class JournalsImporterRecordMap : ClassMap<JournalsImporterRecord>
    {
        public JournalsImporterRecordMap()
        {
            Map(x => x.No).Name("No");
            Map(x => x.Id).Name("Id");
            Map(x => x.Points).Name("Points");

            Map(x => x.Name).Ignore()
                            .ConvertUsing((row) => string.IsNullOrWhiteSpace(row["Name1"]) ? row["Name2"].Trim() : row["Name1"].Trim());

            Map(x => x.NameAlt).Ignore()
                               .ConvertUsing((row) => row["Name2"].Trim());

            Map(x => x.ISSN).Ignore()
                            .ConvertUsing((row) => string.IsNullOrWhiteSpace(row["ISSN1"]) ? row["ISSN2"].Trim() : row["ISSN1"].Trim());

            Map(x => x.EISSN).Ignore()
                             .ConvertUsing((row) => string.IsNullOrWhiteSpace(row["EISSN1"]) ? row["EISSN2"].Trim() : row["EISSN1"].Trim());

            base.Map(m => m.Disciplines).ConvertUsing((row) =>
            {
                string[] headers = row.Context.HeaderRecord;

                int firstColumn = row.Context.NamedIndexes["Points"].Single() + 1;
                int lastColumn = row.Context.HeaderRecord.Length - 1;

                List<string> disciplines = new List<string>();
                for (int i = firstColumn; i < lastColumn; ++i)
                {
                    string value = row[i].Trim();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        string currentHeader = headers[i];
                        disciplines.Add(currentHeader);
                    }
                }

                return disciplines;
            });
        }
    }
}
