using P72.Api.Common;
using P72.Api.Trading.Models;
using P72.Api.Trading.Models.Request;
using P72.Api.Trading.Models.Response;
using P72.Api.Common.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection.Metadata;
using P72.Api.Trading.Models.Trade;
using System.Collections.Immutable;

namespace P72.Api.Trading.DataAccess.Repository
{
    public class TradeRepository : ITradeRepository
    {
        private readonly IConfigManager _configuration;
        public TradeRepository(IConfigManager configuration)
        {
            this._configuration = configuration;
        }

        protected virtual IDbConnection GetConnection()
        {
            return new SqlConnection(this._configuration.P72Connection);
        }

        private static void OpenConnectionString(IDbConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        private IDbCommand AssignCommonParaeters(IDbCommand sqlCommand, string procName, string? accountID)
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = procName;
            sqlCommand.CommandTimeout = 1000;
           
            var brIdParameter = sqlCommand.CreateParameter();
            brIdParameter.ParameterName = "@AccountID";
            brIdParameter.Value = accountID;
            sqlCommand.Parameters.Add(brIdParameter);
            
            return sqlCommand;
        }
        

        public bool SaveLoggingRequestResponse(LoggingDataModel loggingDataModel)
        {
            //Skipping as currently not saving to DB
            var IssaveLoggingRequestResponse = false;
            if (IssaveLoggingRequestResponse)
            {
                using (var sqlConnection = GetConnection())
                {
                    OpenConnectionString(sqlConnection);
                    var loggingDataTable = TranslateToLoggingTableEntity(loggingDataModel);

                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {

                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "InsertTradingRequestResponseLog";
                        sqlCommand.CommandTimeout = 1000;
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Parameters.Add(new SqlParameter("@ClopRequestResponseLogTableData", loggingDataTable));
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            
            return true;
        }
        private static DataTable TranslateToLoggingTableEntity(LoggingDataModel loggingDataModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("RequestData", typeof(string)),
                new DataColumn("ResponseData", typeof(string)),
                new DataColumn("MachineName", typeof(string)),
                new DataColumn("CreatedBy", typeof(string)),
                new DataColumn("CreatedDate", typeof(DateTime)),
                new DataColumn("Endpoint", typeof(string)),
                new DataColumn("UniqueRequestId", typeof(string))
            });
            dataTable.Rows.Add(
                loggingDataModel.RequestData,
                loggingDataModel.ResponseData,
                loggingDataModel.MachineName,
                loggingDataModel.CreatedBy,
                loggingDataModel.CreatedDate,
                loggingDataModel.Endpoint,
                loggingDataModel.UniqueRequestId
                );
            return dataTable;
        }
                
        public List<GetTradeResponseModel> GetTradeDetailsFromDB(GetTradeRequestModel model)
        {
            List<GetTradeResponseModel> responsemodel = new();
            using (var sqlConnection = GetConnection())
            {
                OpenConnectionString(sqlConnection);
                using (var _sqlCommand = sqlConnection.CreateCommand())
                {
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandText = "GetTradeDetails";
                    _sqlCommand.CommandTimeout = 1000;

                    var PCIDParameter = _sqlCommand.CreateParameter();
                    PCIDParameter.ParameterName = "@AccountID";
                    PCIDParameter.Value = model.AccountID;
                    _sqlCommand.Parameters.Add(PCIDParameter);

                    IDataReader rdr = _sqlCommand.ExecuteReader();
                    while (rdr.Read())
                    {
                        GetTradeResponseModel trade = new();
                        trade.TradeID = Convert.ToInt64(rdr["TradeID"]);
                        trade.AccountID = Convert.ToInt32(rdr["AccountID"]);
                        trade.InstrumentID = Convert.ToInt32(rdr["InstrumentID"]);
                        trade.TradeType = GetDbValueDefaultStringNull(rdr, "TradeType");
                        trade.Quantity = GetDbValueDefaultDecimalNull(rdr, "Quantity",2);
                        trade.Price = GetDbValueDefaultDecimalNull(rdr, "Price", 2);
                        trade.TradeDate = Convert.ToDateTime(rdr["TradeDate"]);
                        trade.Commission = GetDbValueDefaultDecimalNull(rdr, "Commission", 2);                   
                     
                        responsemodel.Add(trade);
                    }
                }
                return responsemodel;
            }
        }

        public List<GetTradeResponseModel> GetTradeDetails(GetTradeRequestModel model)
        {
            List<GetTradeResponseModel> trades = GetTradeData().Where(p => p.AccountID == model.AccountID).ToList();

            return trades;
           
        }

        public List<GetPositionResponseModel> GetPositionDetails(GetTradeRequestModel model)
        {
            var trades = GetTradeData().Where(p => p.AccountID == model.AccountID).ToList();
            var tradesByInstrument = from trade in trades group trade by  trade.InstrumentID
                                     into tradeInstrumentGroup
                                     select new GetPositionResponseModel {
                                         InstrumentID = tradeInstrumentGroup.Key,
                                         Quantity = tradeInstrumentGroup.Sum(p => p.Quantity),
                                         AverageCostBasis = tradeInstrumentGroup.Sum(s=> s.Price * s.Quantity)/ tradeInstrumentGroup.Sum(p => p.Quantity),
                                         AccountID = model.AccountID,
                                         PositionID = 1
                                     };

            return tradesByInstrument.ToList();

        }

        private string? GetDbValueDefaultStringNull(IDataReader dataReader, string columnName)
        {
            return dataReader[columnName] == DBNull.Value ? null : Convert.ToString(dataReader[columnName]);
        }

        private decimal? GetDbValueDefaultDecimalNull(IDataReader dataReader, string columnName, int decimals)
        {
            return dataReader[columnName] == DBNull.Value ? null : Math.Round(Convert.ToDecimal(dataReader[columnName]), decimals);
        }

        private string? GetDbValueStringValue(IDataReader dataReader, string columnName)
        {
            if (dataReader[columnName] == DBNull.Value)
            {
                return "No";
            }
            return Convert.ToBoolean(dataReader[columnName]) ? "Yes" : "No";
        }

        private List<GetTradeResponseModel> GetTradeData()
        {
            List<GetTradeResponseModel> tradeList = new List<GetTradeResponseModel>()
            {
                new GetTradeResponseModel() {
                    TradeID = 1,
                    AccountID = 1,
                    InstrumentID = 1,
                    TradeType = "Buy",
                    Quantity = 100,
                    Price = 10,
                    TradeDate = DateTime.Now,
                    Commission = 1

                },


                new GetTradeResponseModel() {
                    TradeID = 2,
                    AccountID = 1,
                    InstrumentID = 2,
                    TradeType = "Buy",
                    Quantity = 100,
                    Price = 20,
                    TradeDate = DateTime.Now,
                    Commission = 1

                },
                new GetTradeResponseModel() {
                    TradeID = 3,
                    AccountID = 2,
                    InstrumentID = 3,
                    TradeType = "Buy",
                    Quantity = 100,
                    Price = 20,
                    TradeDate = DateTime.Now,
                    Commission = 1

                },
                new GetTradeResponseModel() {
                    TradeID = 4,
                    AccountID = 2,
                    InstrumentID = 4,
                    TradeType = "Buy",
                    Quantity = 100,
                    Price = 20,
                    TradeDate = DateTime.Now,
                    Commission = 1

                },
                new GetTradeResponseModel() {
                    TradeID = 5,
                    AccountID = 1,
                    InstrumentID = 1,
                    TradeType = "Buy",
                    Quantity = 100,
                    Price = 15,
                    TradeDate = DateTime.Now,
                    Commission = 1                    
                },

                new GetTradeResponseModel() {
                    TradeID = 6,
                    AccountID = 1,
                    InstrumentID = 2,
                    TradeType = "Buy",
                    Quantity = 100,
                    Price = 18,
                    TradeDate = DateTime.Now,
                    Commission = 1

                }

            };

                
                
            return tradeList;

        }
    }
}