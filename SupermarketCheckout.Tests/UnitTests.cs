namespace SupermarketCheckout.UnitTests
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
                Assert.That(Checkout.Calculate("", sampleStockKeepingUnits), Is.EqualTo(0));
                Assert.That(Checkout.Calculate("A", sampleStockKeepingUnits), Is.EqualTo(50));
                Assert.That(Checkout.Calculate("AB", sampleStockKeepingUnits), Is.EqualTo(80));
                Assert.That(Checkout.Calculate("CDBA", sampleStockKeepingUnits), Is.EqualTo(115));
                Assert.That(Checkout.Calculate("AA", sampleStockKeepingUnits), Is.EqualTo(100));
                Assert.That(Checkout.Calculate("AAA", sampleStockKeepingUnits), Is.EqualTo(130));
                Assert.That(Checkout.Calculate("AAABB", sampleStockKeepingUnits), Is.EqualTo(175));
            });
        }
    }
}