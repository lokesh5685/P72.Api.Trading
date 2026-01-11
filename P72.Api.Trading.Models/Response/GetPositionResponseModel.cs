namespace P72.Api.Trading.Models.Response
{
    public class GetPositionResponseModel
    {
        public int? PositionID { get; set; }
        public int? AccountID { get; set; }
        public int? InstrumentID { get; set; }//UNIQUE AccountId and InstrumentID combined
        public decimal? Quantity { get; set; }
        public decimal? AverageCostBasis { get; set; }// Can store average cost for P/L calculation

        public DateTime? LastUpdated { get; set; }
    }
}
