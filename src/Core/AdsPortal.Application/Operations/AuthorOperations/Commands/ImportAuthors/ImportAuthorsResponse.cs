namespace AdsPortal.Application.OperationsModels.Importer
{
    using AdsPortal.Application.OperationsAbstractions;

    public class ImportAuthorsResponse : IOperationResult
    {
        public ImporterModel Authors { get; }
        public ImporterModel Degrees { get; }
        public ImporterModel Departments { get; }

        public ImportAuthorsResponse()
        {
            Authors = new ImporterModel();
            Degrees = new ImporterModel();
            Departments = new ImporterModel();
        }

        public ImportAuthorsResponse(ImporterModel authors,
                                     ImporterModel degrees,
                                     ImporterModel departments)
        {
            Authors = authors;
            Degrees = degrees;
            Departments = departments;
        }
    }
}
