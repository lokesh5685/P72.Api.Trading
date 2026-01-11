using System;
using System.Collections.Generic;
using System.Text;

namespace P72.Api.Trading.Models.Response
{
    public class TradApiErrorResponse
    {
        public int? statusCode { get; set; }
        public string? responseMessage { get; set; }
        public List<TradeErrors>? errors { get; set; }
        public TradApiErrorResponse(int code, string msg)
        {
            this.statusCode = code;
            this.responseMessage = msg;
        }
        public TradApiErrorResponse(int code, List<TradeErrors>? errors)
        {
            this.statusCode = code;
            this.errors = errors;
            this.responseMessage = "";
        }
    }
    }
