namespace SupermarketCheckout
{
    public class CheckoutResult
    {
        public int TotalPrice { get; set; }
        public List<string> Errors { get; set; } = [];

        public bool HasErrors => Errors.Count != 0;
    }
}
