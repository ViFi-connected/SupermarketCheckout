namespace SupermarketCheckout
{
    public static class Checkout
    {
        public static CheckoutResult Calculate(string basket, StockKeepingUnit[] sampleStockKeepingUnits)
        {
            var result = new CheckoutResult();
            var unitsInBasket = ParseStockKeepingUnits(basket, result);

            if (result.HasErrors)
            {
                return result;
            }

            var totalPrice = 0;

            foreach (var kvp in unitsInBasket)
            {
                var unitCount = kvp.Value;

                // Find corresponding Stock Keeping Unit
                var sampleUnit = sampleStockKeepingUnits.SingleOrDefault(sampleUnit => sampleUnit.ID == kvp.Key);

                if (sampleUnit == null)
                {
                    result.Errors.Add($"Invalid item '{kvp.Key}' in basket.");
                    continue;
                }

                totalPrice += sampleUnit.Offer != null
                    ? sampleUnit.Offer.Apply(unitCount, sampleUnit.Price)
                    : unitCount * sampleUnit.Price;
            }

            result.TotalPrice = totalPrice;
            return result;

            static Dictionary<char, int> ParseStockKeepingUnits(string basket, CheckoutResult result)
            {
                if (string.IsNullOrWhiteSpace(basket))
                {
                    result.Errors.Add("Basket cannot be empty.");
                    return [];
                }

                char[] characters = basket.ToCharArray();
                Array.Sort(characters);
                Dictionary<char, int> units = [];
                char? lastChar = null;

                for (int i = 0; i < characters.Length; i++)
                {
                    if (!char.IsLetter(characters[i]))
                    {
                        result.Errors.Add($"Invalid character '{characters[i]}' in basket.");
                        continue;
                    }

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