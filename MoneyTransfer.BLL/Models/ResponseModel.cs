namespace MoneyTransfer.BLL.Models
{
    public class ResponseModel<T>
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
