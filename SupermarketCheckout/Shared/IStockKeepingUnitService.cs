using SupermarketCheckout.Models;

namespace SupermarketCheckout.Shared
{
    public interface IStockKeepingUnitService
    {
        StockKeepingUnit[] GetAll();
        StockKeepingUnit? GetById(char id);
    }
}
