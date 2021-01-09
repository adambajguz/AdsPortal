namespace AdsPortal.Shared.Common.Interfaces.StringSimilarity
{
    public interface IStringSimilarityComparerService
    {
        bool AreSimilar(string s0, string s1, StringComparers type = StringComparers.JaroWinkler, double threshold = 0.6);
        bool AreNotSimilar(string s0, string s1, StringComparers type = StringComparers.JaroWinkler, double threshold = 0.6);
        double GetSimilarity(string s0, string s1, StringComparers type = StringComparers.JaroWinkler);
    }
}