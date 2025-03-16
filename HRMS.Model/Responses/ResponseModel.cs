namespace HRMS.Model.Responses
{
    public class ResponseModel<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public string? Error { get; set; }

        public ResponseModel() { }

        public ResponseModel(int statusCode, string message, T? data = default, string? error = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            Error = error;
        }
    }
}
