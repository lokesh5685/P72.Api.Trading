namespace P72.Api.Trading.Models
{
    public class LoggingDataModel
    {
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
        public string? MachineName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Endpoint { get; set; }
        public string? UniqueRequestId { get; set; }
    }

}
