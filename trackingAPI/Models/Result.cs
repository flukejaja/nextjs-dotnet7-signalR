namespace trackingAPI.Models
{
    public class ResultList<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public bool IsCompleted { get; set; } = true;
        public string Message { get; set; } = "";

    }
    public class ResultBool
    {
        public bool IsCompleted { get; set; } = true;
        public string Message { get; set; } = "";

    }
    public class ResultObject<T>
    {
        public T Data { get; set; } = default!;
        public bool IsCompleted { get; set; } = true;
        public string Message { get; set; } = "";

    }
}

