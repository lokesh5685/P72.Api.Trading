using P72.Api.Common;
using P72.Api.Trading.DataAccess.Repository;
using P72.Api.Trading.Models.Request;
using P72.Api.Trading.Models.Response;
using P72.Api.Common.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Constant = P72.Api.Common.Constant;

namespace P72.Api.Trading.Orchestrator
{
    public class TradeDetails : ITradeDetails
    {
        private readonly IConfigManager? _configuration;
        private readonly ITradeRepository _tradeRepository;
        private readonly ILogger<TradeDetails> _logger;
        private string _userRoleForNewPricingAPI = string.Empty;

        public TradeDetails(ITradeRepository tradeRepository, ILogger<TradeDetails> logger, IConfigManager? configuration)
        {
            _tradeRepository = tradeRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public List<GetTradeResponseModel> GetTradeDetailsFromDB(GetTradeRequestModel model)
        {
            var watch = new Stopwatch();
            watch.Start();

            var result = _tradeRepository.GetTradeDetailsFromDB(model);
            watch.Stop();
            var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
            string logMessage = $"\n\n GetTradeDetails function took Time (Millisec):{responseTimeForCompleteRequest}";
            _logger.LogInformation(logMessage, model);
            return result;
        }

        public List<GetTradeResponseModel> GetTradeDetails(GetTradeRequestModel model)
        {
            var watch = new Stopwatch();
            watch.Start();

            var result = _tradeRepository.GetTradeDetails(model);
            watch.Stop();
            var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
            string logMessage = $"\n\n GetTradeDetails function took Time (Millisec):{responseTimeForCompleteRequest}";
            _logger.LogInformation(logMessage, model);
            return result;
        }

        public List<GetPositionResponseModel> GetPositionDetails(GetTradeRequestModel model)
        {
            var watch = new Stopwatch();
            watch.Start();

            var result = _tradeRepository.GetPositionDetails(model);
            watch.Stop();
            var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
            string logMessage = $"\n\n GetPositionDetails function took Time (Millisec):{responseTimeForCompleteRequest}";
            _logger.LogInformation(logMessage, model);
            return result;
        }

    }
}