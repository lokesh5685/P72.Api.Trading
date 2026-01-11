using System;
using System.Collections.Generic;
using System.Text;

namespace P72.Api.Trading.Models.Response
{
    public class GetPositionApiSuccessResponse
    {
        public int statusCode { get; set; }
        public string responseMessage { get; set; }
        public List<GetPositionResponseModel>? data { get; set; }

        public GetPositionApiSuccessResponse(int code, List<GetPositionResponseModel> data)
        {
            this.statusCode = code;
            this.data = data;
            this.responseMessage = "Get Position Details Successful";
        }
    }
}
