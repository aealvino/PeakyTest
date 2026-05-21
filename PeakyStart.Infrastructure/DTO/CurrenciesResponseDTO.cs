namespace PeakyStart.Infrastructure.DTO
{
    public class CurrenciesResponseDTO
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public Dictionary<string, CurrencyDTO> Valute { get; set; }
    }
}
