namespace MindHeal.Models.DTOs
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
