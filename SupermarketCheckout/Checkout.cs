using System.Text.Json.Serialization;

namespace SupermarketCheckout
{
    public static class Checkout
    {
        public static int Calculate(string basket, StockKeepingUnit[] sampleStockKeepingUnits)
        {
            var unitsInBasket = ParseStockKeepingUnits(basket);
            var totalPrice = 0;

            foreach (var kvp in unitsInBasket)
            {
                var unitCount = kvp.Value;

                // Find corresponding Stock Keeping Unit
                var sampleUnit = sampleStockKeepingUnits.Single(sampleUnit => sampleUnit.ID == kvp.Key);

                totalPrice += sampleUnit.Offer != null
                    ? sampleUnit.Offer.Apply(unitCount, sampleUnit.Price)
                    : unitCount * sampleUnit.Price;
            }
            return totalPrice;

            static Dictionary<char, int> ParseStockKeepingUnits(string basket)
            {
                char[] characters = [.. basket];
                Array.Sort(characters);
                Dictionary<char, int> units = [];
                char? lastChar = null;

                for (int i = 0; i < characters.Length; i++)
                {
                    if (lastChar != characters[i])
                    {
                        units.Add(characters[i], 1);
                    }
                    else
                    {
                        units[characters[i]]++;
                    }
                    lastChar = characters[i];
                }
                return units;
            }
        }
    }
}