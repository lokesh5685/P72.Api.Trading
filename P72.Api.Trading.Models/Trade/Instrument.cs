using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace P72.Api.Trading.Models.Trade
{
    public class Instrument
    {
        public int? InstrumentID { get; set; }
        public string? Symbol { get; set; }
        public string? CompanyName { get; set; }
        public string? Exchange { get; set; }
        public string? InstrumentType { get; set; }

    }
}
