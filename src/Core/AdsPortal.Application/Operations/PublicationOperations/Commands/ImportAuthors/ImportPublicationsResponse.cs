namespace AdsPortal.Application.OperationsModels.Importer
{
    using AdsPortal.Application.OperationsAbstractions;

    public class ImportPublicationsResponse : IOperationResult
    {
        public ImporterModel Publications { get; }
        public ImporterModel AuthorsInPublications { get; }

        public ImportPublicationsResponse()
        {
            Publications = new ImporterModel();
            AuthorsInPublications = new ImporterModel();
        }

        public ImportPublicationsResponse(ImporterModel publications,
                                          ImporterModel authorsInPublications)
        {
            Publications = publications;
            AuthorsInPublications = authorsInPublications;
        }
    }
}
