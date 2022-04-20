namespace WebHooks.API.Models
{
    public class Result
    {
        public Result()
        {

        }

        public Result(bool success)
        {
            Success = success;
        }

        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool? Success { get; set; }
        public string? Message { get; set; }
    }

    public class Result<TData> : Result
    {
        public Result() : base() { }
        public Result(TData data) : base()
        {
            Data = data;
        }

        public Result(TData data, bool success):base(success) { 
            Data = data;
        }

        public Result(TData data, bool success, string message) : base(success, message)
        {
            Data = data;
        }
        public TData? Data { get; set; }
    }

    public class ListResult<TItem> : Result
    {
        public ListResult() : base() { 
            Items = new List<TItem>();
        }

        public ListResult(List<TItem> items) :base()
        {
            Items = items;
        }

        public ListResult(List<TItem> items, bool success) : base(success)
        {
            Items = items;
        }

        public ListResult(List<TItem> items, bool success, string message) : base(success, message)
        {
            Items = items;
        }

        public List<TItem> Items { get; set; }
    }
}
