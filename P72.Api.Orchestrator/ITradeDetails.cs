using P72.Api.Trading.Models.Request;
using P72.Api.Trading.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace P72.Api.Trading.Orchestrator
{
    public interface ITradeDetails
    {
        List<GetTradeResponseModel> GetTradeDetailsFromDB(GetTradeRequestModel model);
        List<GetTradeResponseModel> GetTradeDetails(GetTradeRequestModel model);
        List<GetPositionResponseModel> GetPositionDetails(GetTradeRequestModel model);


    }
}