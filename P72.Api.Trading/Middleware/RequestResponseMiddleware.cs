using P72.Api.Trading.DataAccess.Repository;
using P72.Api.Trading.Models;
using Microsoft.IO;
using System.Diagnostics.CodeAnalysis;

namespace P72.Api.Trading.Middleware
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestResponseMiddleware> logger;
        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
        private readonly ITradeRepository _tradeRepository;


        public RequestResponseMiddleware(RequestDelegate next, ILogger<RequestResponseMiddleware> logger,
            ITradeRepository tradeRepository)
        {
            this.next = next;
            this.logger = logger;
            recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _tradeRepository = tradeRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogResponse(context);
        }

        private void CreateLoggingObject(string request, string response, string endpoint,
           string uniqueRequestId, string loggedInUserName)
        {
            LoggingDataModel loggingDataModelData = new LoggingDataModel()
            {
                UniqueRequestId = uniqueRequestId,
                CreatedBy = loggedInUserName,
                CreatedDate = DateTime.Now,
                MachineName = Environment.MachineName,
                Endpoint = endpoint,
                RequestData = request,
                ResponseData = response
            };
            _tradeRepository.SaveLoggingRequestResponse(loggingDataModelData);
        }

        [ExcludeFromCodeCoverage]
        private async Task LogResponse(HttpContext context)
        {
            string requestLogMessage = string.Empty;
            string responseLogMessage = string.Empty;
            context.Request.EnableBuffering();            
            using (var requestStream = recyclableMemoryStreamManager.GetStream())
            {
                await context.Request.Body.CopyToAsync(requestStream);
                requestLogMessage = "\n ===========================================================================================" +
                                      $"\n Http Request Information:{Environment.NewLine}" +
                                      $"\n Schema:{context.Request.Scheme} " +
                                      $"\n Host: {context.Request.Host} " +
                                      $"\n Path: {context.Request.Path} " +
                                      $"\n QueryString: {context.Request.QueryString} " +
                                      $"\n Request Body: {ReadStreamInChunks(requestStream)} \n";
                logger.LogInformation(requestLogMessage);
                logger.LogInformation("Request Method :" + context.Request.Method + ",QueryString HasValue :" + context.Request.QueryString.HasValue);
            }

            context.Request.Body.Position = 0;

            var originalBodyStream = context.Response.Body;
            using (var responseBody = recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = responseBody;
                await next(context);
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                responseLogMessage = "\n\n " +
                                       $"\n Http Response Information:{Environment.NewLine}" +
                                       $"\n Schema:{context.Request.Scheme} " +
                                       $"\n Host: {context.Request.Host} " +
                                       $"\n Path: {context.Request.Path} " +
                                       $"\n QueryString: {context.Request.QueryString} " +
                                       $"\n Response Body: {text} \n===========================================================================================\n ";

                logger.LogInformation(responseLogMessage);
                if (context.Request.Path.ToString().Contains("api/Trade", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    if (context.Request.Method == "POST" || context.Request.QueryString.HasValue)
                    {
                        CreateLoggingObject(requestLogMessage, responseLogMessage, context.Request.Path, "", "");
                    }
                }
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using (var textWriter = new StringWriter())
            {
                using (var reader = new StreamReader(stream))
                {
                    var readChunk = new char[readChunkBufferLength];
                    int readChunkLength;
                    do
                    {
                        readChunkLength = reader.ReadBlock(readChunk,
                                                           0,
                                                           readChunkBufferLength);
                        textWriter.Write(readChunk, 0, readChunkLength);
                    } while (readChunkLength > 0);
                    return textWriter.ToString();
                }
            }
        }
    }
}
