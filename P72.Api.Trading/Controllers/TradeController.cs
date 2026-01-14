using P72.Api.Common;
using P72.Api.Trading.Helper;
using P72.Api.Trading.Models.Request;
using P72.Api.Trading.Models.Response;
using P72.Api.Trading.Orchestrator;
using P72.Api.Common.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using P72.API.Trading.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace P72.Api.Trading.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITradeDetails _tradeDetails;
        private readonly ValidationController _validationController;
        private readonly IConfigManager _configuration;
        private readonly HttpClient _client;

        public TradeController(ITradeDetails tradeDetails, IHttpContextAccessor httpContextAccessor, IConfigManager configuration, IHttpClientFactory httpClientFactory)
        {
            this._tradeDetails = tradeDetails;
            this._validationController = new ValidationController(_tradeDetails, httpContextAccessor);
            this._configuration = configuration;
            _client = httpClientFactory.CreateClient();
        }
        

        [HttpGet]
        [Route("GetTradeDetailsFromDB")]
        [SwaggerIgnore]
        public IActionResult GetTradeDetailsFromDB([FromQuery] GetTradeRequestModel model)
        {
            try
            {
                var result = _validationController.ValidateAccount(model.AccountID,true) as OkObjectResult;
                if (result?.Value != null)
                {
                    return new NotFoundObjectResult(result);
                }
                else
                {
                    var tradeDetails = _tradeDetails.GetTradeDetailsFromDB(model);

                    return new OkObjectResult(new GetTradeApiSuccessResponse((int)HttpStatusCode.Created, tradeDetails));

                }
            }
            catch (Exception exception)
            {
                throw new ArgumentException(exception.Message, exception);
            }
        }

        [HttpGet]
        [Route("GetTradeDetails")]
        [ServiceFilter(typeof(TradeRequestValidatorAttribute))]
        public IActionResult GetTradeDetails([FromQuery] GetTradeRequestModel model)
        {
            try
            {
                var result = _validationController.ValidateAccount(model.AccountID, true) as OkObjectResult;
                if (result?.Value == null)
                {
                    return new NotFoundObjectResult(result);
                }
                else
                {

                    var tradeDetails = _tradeDetails.GetTradeDetails(model);

                    return new OkObjectResult(new GetTradeApiSuccessResponse((int)HttpStatusCode.Created, tradeDetails));

                }
            }
            catch (Exception exception)
            {
                throw new ArgumentException(exception.Message, exception);
            }
        }

        [HttpGet]
        [Route("GetPositionDetails")]
        public IActionResult GetPositionDetails([FromQuery] GetTradeRequestModel model)
        {
            try
            {
                var result = _validationController.ValidateAccount(model.AccountID, true) as OkObjectResult;
                if (result?.Value == null)
                {
                    return new NotFoundObjectResult(result);
                }
                else
                {

                    var positionDetails = _tradeDetails.GetPositionDetails(model);

                    return new OkObjectResult(new GetPositionApiSuccessResponse((int)HttpStatusCode.Created, positionDetails));

                }
            }
            catch (Exception exception)
            {
                throw new ArgumentException(exception.Message, exception);
            }
        }

    }
}