using P72.Api.Trading.Models;
//using P72.Api.Trading.Models.Entities;
using P72.Api.Trading.Models.Request;
//using P72.Api.Trading.Models.Request.NewPricingCase;
using P72.Api.Trading.Models.Response;
using P72.Api.Trading.Models.Trade;
//using P72.Api.Trading.Models.Response.NewPricingCase;
//using P72.Api.Trading.Models.Validate.NewPricingCase;

namespace P72.Api.Trading.DataAccess.Repository
{
    public interface ITradeRepository
    {
        public bool SaveLoggingRequestResponse(LoggingDataModel loggingDataModel);
        List<GetTradeResponseModel> GetTradeDetailsFromDB(GetTradeRequestModel model);
        List<GetTradeResponseModel> GetTradeDetails(GetTradeRequestModel model);
        List<GetPositionResponseModel> GetPositionDetails(GetTradeRequestModel model);
    }
}