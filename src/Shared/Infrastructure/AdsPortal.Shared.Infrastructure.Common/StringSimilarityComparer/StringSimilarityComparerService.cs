namespace AdsPortal.Shared.Infrastructure.Common.StringSimilarityComparer
{
    using System;
    using AdsPortal.Shared.Common.Interfaces.StringSimilarity;
    using F23.StringSimilarity;
    using F23.StringSimilarity.Interfaces;

    public class StringSimilarityComparerService : IStringSimilarityComparerService
    {
        private JaroWinkler JaroWinklerComparer { get; } = new JaroWinkler();
        private NormalizedLevenshtein NormalizedLevenshteinComparer { get; } = new NormalizedLevenshtein();
        private Cosine CosineComparer { get; } = new Cosine();
        private Jaccard JaccardComparer { get; } = new Jaccard();
        private SorensenDice SorensenDiceComparer { get; } = new SorensenDice();

        public StringSimilarityComparerService()
        {

        }

        private INormalizedStringSimilarity GetSimilarityComparer(StringComparers type)
        {
            return type switch
            {
                StringComparers.JaroWinkler => JaroWinklerComparer,
                StringComparers.NormalizedLevenshtein => NormalizedLevenshteinComparer,
                StringComparers.Cosine => CosineComparer,
                StringComparers.Jaccard => JaccardComparer,
                StringComparers.SorensenDice => SorensenDiceComparer,
                _ => throw new ArgumentException("Invalid string similarity comparer type", nameof(type)),
            };
        }

        public bool AreSimilar(string s0, string s1, StringComparers type = StringComparers.JaroWinkler, double threshold = 0.75)
        {
            double similarity = GetSimilarityComparer(type).Similarity(s0, s1);
            return similarity >= threshold;
        }

        public bool AreNotSimilar(string s0, string s1, StringComparers type = StringComparers.JaroWinkler, double threshold = 0.75)
        {
            double similarity = GetSimilarityComparer(type).Similarity(s0, s1);
            return similarity < threshold;
        }

        public double GetSimilarity(string s0, string s1, StringComparers type = StringComparers.JaroWinkler)
        {
            return GetSimilarityComparer(type).Similarity(s0, s1);
        }
    }
}
