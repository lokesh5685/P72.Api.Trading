using P72.Api.Trading.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;

namespace P72.API.Trading.Filters
{
    public class TradeRequestValidatorAttribute : ServiceFilterAttribute
    {
        public TradeRequestValidatorAttribute() : base(typeof(TradeValidatorFilter))
        {

        }
        public class TradeValidatorFilter : IAuthorizationFilter
        {
            private readonly ILogger<TradeValidatorFilter> _logger;

            public TradeValidatorFilter(ILogger<TradeValidatorFilter> logger)
            {
                this._logger = logger;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                _logger.LogInformation("--------Validating Request Body--------{context.HttpContext.Request}", context.HttpContext.Request);
                string? bodyStr = "";
                var req = context.HttpContext.Request;
                using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = reader.ReadToEnd();
                }
                req.Body.Position = 0;
                GetTradeRequestModel _request = JsonConvert.DeserializeObject<GetTradeRequestModel>(bodyStr)!;
                if (_request != null)
                {
                    _logger.LogInformation("--------Request Body validated successfully--------{context.HttpContext.Request}", context.HttpContext.Request);
                }
            }
        }
    }
}