namespace AdsPortal.Application.Interfaces.Media
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CsvHelper.Configuration;

    public interface ICsvBuilderService
    {
        Task<List<T>> GetRecordsAsync<T>(TextReader reader, string delimiter = ";")
            where T : class;

        Task<List<T>> GetRecordsAsync<T, TMap>(TextReader reader, string delimiter = ";")
            where T : class
            where TMap : ClassMap<T>;

        byte[] BuildProductsFile<T>(IEnumerable<T> records, string delimiter = ";");
    }
}