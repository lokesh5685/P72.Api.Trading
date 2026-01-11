using System;
using System.Collections.Generic;
using System.Text;

namespace P72.Api.Trading.Models.Response
{
    public class TradeErrors
    {
        public string? id { get; set; }
        public string? code { get; set; }
        public string? title { get; set; }
        public string? detail { get; set; }
    }

    public class TradeErrorsList
    {
        public List<TradeErrors>? errors { get; set; }
    }
}
