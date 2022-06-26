namespace WebHooks.API.Models
{
    public class ApiResult
    {
        public ApiResult()
        {

        }

        public bool? Success { get; set; }
        public string? Error { get; set; }
        public dynamic? Result { get; set; }

        public bool __wrapped { get; set; } = true;
    }
}
