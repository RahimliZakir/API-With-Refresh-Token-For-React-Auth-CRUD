namespace Application.WebAPI.AppCode.Application.Infrastructure
{
    public class CommandJsonResponse
    {
        public string Message { get; set; } = null!;

        public bool Error { get; set; }

        public CommandJsonResponse(string message, bool error)
        {
            Message = message;
            Error = error;
        }
    }

    public class CommandJsonResponse<T> : CommandJsonResponse
        where T : class
    {
        public T? Data { get; set; }

        public CommandJsonResponse(string message, bool error, T? data)
            : base(message, error)
        {
            Data = data;
        }
    }
}
