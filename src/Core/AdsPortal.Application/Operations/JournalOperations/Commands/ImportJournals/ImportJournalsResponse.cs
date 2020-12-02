namespace AdsPortal.Application.Operations.JournalOperations.Commands.ImportJournals
{
    using AdsPortal.Application.OperationsAbstractions;

    public class ImportJournalsResponse : IOperationResult
    {
        public int JournalsCreated { get; }

        public ImportJournalsResponse(int journalsCount)
        {
            JournalsCreated = journalsCount;
        }
    }
}
