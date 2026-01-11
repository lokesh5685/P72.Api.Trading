using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace P72.Api.Trading.Models.Trade
{
    public class Positions
    {
        public int? PositionID { get; set; }
        public int? AccountID { get; set; }
        public int? InstrumentID { get; set; }//UNIQUE AccountId and InstrumentID combined
        public decimal? Quantity { get; set; }
        public decimal? AverageCostBasis { get; set; }// Can store average cost for P/L calculation

        public DateTime? LastUpdated { get; set; }

    }
}
