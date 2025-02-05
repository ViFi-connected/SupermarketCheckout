using SupermarketCheckout;
using SupermarketCheckout.Offers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var sampleStockKeepingUnits = new StockKeepingUnit[] {
    new('A', 50, new MultiBuyOffer(3,130)),
    new('B', 30, new MultiBuyOffer(2,45)),
    new('C', 20, null),
    new('D', 15, null)
};

var checkoutApi = app.MapGroup("/checkout");

checkoutApi.MapGet("/", () => sampleStockKeepingUnits);
checkoutApi.MapGet("/{id}", (char id) =>
    sampleStockKeepingUnits.FirstOrDefault(a => a.ID == id) is { } checkout
        ? Results.Ok(checkout)
        : Results.NotFound());

checkoutApi.MapPost("/{basket}", (string basket) =>
{
    var result = Checkout.Calculate(basket, sampleStockKeepingUnits);
    return result.HasErrors ? Results.BadRequest(result.Errors) : Results.Ok(result.TotalPrice);
});

app.Run();

[JsonSerializable(typeof(StockKeepingUnit[]))]
[JsonSerializable(typeof(Offer))]
[JsonSerializable(typeof(List<string>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}