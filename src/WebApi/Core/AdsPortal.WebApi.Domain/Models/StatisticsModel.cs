namespace AdsPortal.WebApi.Domain.Models
{
    public class StatisticsModel<T>
        where T : struct
    {
        public int Count { get; set; }

        public T Sum { get; set; }

        public T Min { get; set; }
        public double Average { get; set; }
        public T Max { get; set; }
    }
}
