namespace SupermarketCheckout.Offers
{
    public class MultiBuyOffer(int _validCount, int _offerPrice) : Offer
    {
        internal override int Apply(int unitCount, int basePrice)
        {
            var totalValidCount = unitCount / _validCount;
            var totalOfferPrice = totalValidCount * _offerPrice;
            var result = totalOfferPrice + (unitCount % _validCount) * basePrice;
            return result;
        }
    }
}
