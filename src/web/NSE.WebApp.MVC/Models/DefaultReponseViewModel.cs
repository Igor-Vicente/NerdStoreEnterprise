namespace NSE.WebApp.MVC.Models
{
    public class DefaultResponseViewModel<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
