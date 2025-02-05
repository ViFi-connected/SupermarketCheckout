using SupermarketCheckout;
using SupermarketCheckout.Offers;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Routing.Constraints;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.SetParameterPolicy<RegexInlineRouteConstraint>("regex");
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Supermarket Checkout API", Version = "v1" });
});

var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Supermarket Checkout API v1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

var sampleStockKeepingUnits = new StockKeepingUnit[] {
    new('A', 50, new MultiBuyOffer(3,130)),
    new('B', 30, new MultiBuyOffer(2,45)),
    new('C', 20, null),
    new('D', 15, null)
};

var checkoutApi = app.MapGroup("/checkout");

checkoutApi.MapGet("/", () => sampleStockKeepingUnits);
checkoutApi.MapGet("/{id}", (char id) =>
    sampleStockKeepingUnits.FirstOrDefault(a => a.ID == id) is { } sampleUnit
        ? Results.Ok(sampleUnit)
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