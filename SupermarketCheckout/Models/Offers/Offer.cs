namespace SupermarketCheckout.Models.Offers
{
    public abstract class Offer
    {
        public string Type { get; set; }
        internal abstract int Apply(int unitCount, int basePrice);

        protected Offer()
        {
            Type = GetType().Name;
        }
    }
}
