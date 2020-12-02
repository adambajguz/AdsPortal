namespace AdsPortal.Infrastructure.Media.CsvBuilder
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Media;
    using CsvHelper;
    using CsvHelper.Configuration;

    public class CsvBuilderService : ICsvBuilderService
    {
        public CsvBuilderService()
        {

        }

        public async Task<List<T>> GetRecordsAsync<T>(TextReader reader, string delimiter = ";")
            where T : class
        {
            CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
                AllowComments = true,
                Comment = '#',
                Escape = '\"',
                HasHeaderRecord = true,
                IgnoreBlankLines = true
            };

            using (CsvReader? csv = new CsvReader(reader, configuration))
            {
                List<T> records = await csv.GetRecordsAsync<T>().ToListAsync();

                return records;
            }
        }

        public async Task<List<T>> GetRecordsAsync<T, TMap>(TextReader reader, string delimiter = ";")
            where T : class
            where TMap : ClassMap<T>
        {
            CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
                AllowComments = true,
                Comment = '#',
                Escape = '\"',
                HasHeaderRecord = true,
                IgnoreBlankLines = true
            };
            configuration.RegisterClassMap<TMap>();

            using (CsvReader? csv = new CsvReader(reader, configuration))
            {
                List<T> records = await csv.GetRecordsAsync<T>().ToListAsync();

                return records;
            }
        }

        public byte[] BuildProductsFile<T>(IEnumerable<T> records, string delimiter = ";")
        {
            CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
                AllowComments = true,
                Comment = '#',
                Escape = '\"',
                HasHeaderRecord = true,
                IgnoreBlankLines = true
            };

            using MemoryStream memoryStream = new MemoryStream();
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            {
                using CsvWriter csvWriter = new CsvWriter(streamWriter, configuration);
                csvWriter.Configuration.RegisterClassMap(typeof(T));
                //csvWriter.Configuration.RegisterClassMap<T>();
                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }
    }
}
