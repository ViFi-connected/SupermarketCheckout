using Moq;
using SupermarketCheckout.Logic;
using SupermarketCheckout.Models;
using SupermarketCheckout.Models.Offers;
using SupermarketCheckout.Shared;

namespace SupermarketCheckout.Tests
{
    [TestFixture]
    public class CheckoutTests
    {
        private Mock<IStockKeepingUnitService> _mockStockService;
        private Checkout _checkout;

        [SetUp]
        public void SetUp()
        {
            _mockStockService = new Mock<IStockKeepingUnitService>();

            var sampleStockKeepingUnits = new StockKeepingUnit[] {
                new('A', 50, new MultiBuyOffer(3,130)),
                new('B', 30, new MultiBuyOffer(2,45)),
                new('C', 20, null),
                new('D', 15, null)
            };

            _mockStockService.Setup(s => s.GetAll()).Returns(sampleStockKeepingUnits);
            _mockStockService.Setup(s => s.GetById(It.IsAny<char>())).Returns((char id) => sampleStockKeepingUnits.FirstOrDefault(sku => sku.ID == id));

            _checkout = new Checkout(_mockStockService.Object);
        }

        [Test]
        public void CalculateCheckout()
        {
            Assert.Multiple(() =>
            {
                var result = _checkout.Calculate("");
                Assert.That(result.TotalPrice, Is.EqualTo(0));
                Assert.That(result.HasErrors, Is.False);

                result = _checkout.Calculate("A");
                Assert.That(result.TotalPrice, Is.EqualTo(50));
                Assert.That(result.HasErrors, Is.False);

                result = _checkout.Calculate("AB");
                Assert.That(result.TotalPrice, Is.EqualTo(80));
                Assert.That(result.HasErrors, Is.False);

                result = _checkout.Calculate("CDBA");
                Assert.That(result.TotalPrice, Is.EqualTo(115));
                Assert.That(result.HasErrors, Is.False);

                result = _checkout.Calculate("AA");
                Assert.That(result.TotalPrice, Is.EqualTo(100));
                Assert.That(result.HasErrors, Is.False);

                result = _checkout.Calculate("AAA");
                Assert.That(result.TotalPrice, Is.EqualTo(130));
                Assert.That(result.HasErrors, Is.False);

                result = _checkout.Calculate("AAABB");
                Assert.That(result.TotalPrice, Is.EqualTo(175));
                Assert.That(result.HasErrors, Is.False);

                result = _checkout.Calculate("X");
                Assert.That(result.HasErrors, Is.True);
                Assert.That(result.Errors, Contains.Item("Invalid item 'X' in basket."));
            });
        }
    }
}