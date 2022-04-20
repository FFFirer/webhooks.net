namespace WebHooks.API.Models
{
    public class ListDto<TData>
    {
        public ListDto()
        {
            Items = new List<TData>();
        }

        public List<TData> Items { get; set; }
    }
}
