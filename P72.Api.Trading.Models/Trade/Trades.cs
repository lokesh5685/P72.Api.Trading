using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text;

namespace P72.Api.Trading.Models.Trade
{
    public class Trades
    {
        public long? TradeID { get; set; }
        public int? AccountID { get; set; }
        public int? InstrumentID { get; set; }
        public string? TradeType { get; set; }//CHECK IN ('Buy', 'Sell')),
        public decimal? Quantity { get; set; }//CHECK > 0
        public decimal? Price { get; set; }
        public DateTime? TradeDate { get; set; }
        public decimal? Commission { get; set; }

    }
}
