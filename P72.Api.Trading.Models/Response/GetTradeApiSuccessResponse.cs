using System;
using System.Collections.Generic;
using System.Text;

namespace P72.Api.Trading.Models.Response
{
    public class GetTradeApiSuccessResponse
    {
        public int statusCode { get; set; }
        public string responseMessage { get; set; }
        public List<GetTradeResponseModel>? data { get; set; }

        public GetTradeApiSuccessResponse(int code, List<GetTradeResponseModel> data)
        {
            this.statusCode = code;
            this.data = data;
            this.responseMessage = "Get Trade Details Successful";
        }
    }
}
