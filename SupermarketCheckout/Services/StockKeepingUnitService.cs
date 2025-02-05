using SupermarketCheckout.Models;
using SupermarketCheckout.Models.Offers;
using SupermarketCheckout.Shared;

namespace SupermarketCheckout.Services
{
    public class StockKeepingUnitService : IStockKeepingUnitService
    {
        private readonly StockKeepingUnit[] _stockKeepingUnits = [
            new('A', 50, new MultiBuyOffer(3,130)),
            new('B', 30, new MultiBuyOffer(2,45)),
            new('C', 20, null),
            new('D', 15, null)
        ];

        public StockKeepingUnit[] GetAll() => _stockKeepingUnits;

        public StockKeepingUnit? GetById(char id) => _stockKeepingUnits.FirstOrDefault(sku => sku.ID == id);
    }
}
