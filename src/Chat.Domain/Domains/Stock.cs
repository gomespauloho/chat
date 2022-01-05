namespace Chat.Domain.Domains
{
    public class Stock
    {
        public string Code { get; set; }
        public string Amount  { get; set; }

        public Stock()
        {
        }

        public Stock(string code, string amount)
        {
            Code = code;
            Amount = amount;
        }
    }
}
