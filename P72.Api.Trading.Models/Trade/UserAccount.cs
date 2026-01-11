using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace P72.Api.Trading.Models.Trade
{
    public class UserAccount
    {
        public int? AccountID { get; set; }
        public int? UserID { get; set; }
        public string? AccountNumber { get; set; }
        public string? Currency { get; set; }

    }
}
