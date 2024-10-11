namespace NSE.Core.Models
{
    public class ResponseResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }

    public class ResponseResult
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
