using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using P72.Api.Trading.Models.Request;
using P72.Api.Trading.Models.Response;
using P72.Api.Trading.Orchestrator;
using System.Net;

namespace P72.Api.Trading.Controllers
{
    [Route("api/trade")]
    [ApiController]
    public class ValidationController : Controller
    {
        private readonly ITradeDetails? _tradeDetails;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidationController(ITradeDetails tradeDetails, IHttpContextAccessor httpContextAccessor)
        {
            this._tradeDetails = tradeDetails;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("validate")]
        public IActionResult ValidateAccount([FromQuery] int? accountID, [FromQuery] bool IsInternalRquest = false)
        {
            var result = "OK";//Actual implementation to validate user account
            if (IsInternalRquest)
            {
                return new OkObjectResult(result);
            }
            else if (result == null)
            {
                return new NotFoundObjectResult((int)HttpStatusCode.NotFound);
            }
            else
            {
                return new OkObjectResult(result);
            }


        }

    }
}