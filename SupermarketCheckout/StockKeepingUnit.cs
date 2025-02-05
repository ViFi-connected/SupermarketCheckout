using SupermarketCheckout.Offers;

namespace SupermarketCheckout
{
    public record StockKeepingUnit(char ID, int Price, Offer? Offer);
}
