using SupermarketCheckout.Offers;

namespace SupermarketCheckout.Tests
{
    [TestFixture]
    public class SupermarketCheckout
    {
        StockKeepingUnit[] sampleStockKeepingUnits;

        [SetUp]
        public void SetUp()
        {
            sampleStockKeepingUnits = [
                new('A', 50, new MultiBuyOffer(3,130)),
                new('B', 30, new MultiBuyOffer(2,45)),
                new('C', 20, null),
                new('D', 15, null)
            ];
        }

        [Test]
        public void CalculateCheckout()
        {
            Assert.Multiple(() =>
            {
                var result = Checkout.Calculate("", sampleStockKeepingUnits);
                Assert.That(result.TotalPrice, Is.EqualTo(0));
                Assert.That(result.HasErrors, Is.False);

                result = Checkout.Calculate("A", sampleStockKeepingUnits);
                Assert.That(result.TotalPrice, Is.EqualTo(50));
                Assert.That(result.HasErrors, Is.False);

                result = Checkout.Calculate("AB", sampleStockKeepingUnits);
                Assert.That(result.TotalPrice, Is.EqualTo(80));
                Assert.That(result.HasErrors, Is.False);

                result = Checkout.Calculate("CDBA", sampleStockKeepingUnits);
                Assert.That(result.TotalPrice, Is.EqualTo(115));
                Assert.That(result.HasErrors, Is.False);

                result = Checkout.Calculate("AA", sampleStockKeepingUnits);
                Assert.That(result.TotalPrice, Is.EqualTo(100));
                Assert.That(result.HasErrors, Is.False);

                result = Checkout.Calculate("AAA", sampleStockKeepingUnits);
                Assert.That(result.TotalPrice, Is.EqualTo(130));
                Assert.That(result.HasErrors, Is.False);

                result = Checkout.Calculate("AAABB", sampleStockKeepingUnits);
                Assert.That(result.TotalPrice, Is.EqualTo(175));
                Assert.That(result.HasErrors, Is.False);

                result = Checkout.Calculate("X", sampleStockKeepingUnits);
                Assert.That(result.HasErrors, Is.True);
                Assert.That(result.Errors, Contains.Item("Invalid item 'X' in basket."));
            });
        }
    }
}