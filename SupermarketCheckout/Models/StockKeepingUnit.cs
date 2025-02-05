using SupermarketCheckout.Models.Offers;

namespace SupermarketCheckout.Models
{
    public record StockKeepingUnit(char ID, int Price, Offer? Offer);
}
