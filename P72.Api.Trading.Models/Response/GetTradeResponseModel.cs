namespace P72.Api.Trading.Models.Response
{
    public class GetTradeResponseModel
    {
        public long? TradeID { get; set; }
        public int? AccountID { get; set; }
        public int? InstrumentID { get; set; }
        public string? TradeType { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTime? TradeDate { get; set; }
        public decimal? Commission { get; set; }
    }
}
