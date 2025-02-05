using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.OpenApi.Models;
using SupermarketCheckout;
using SupermarketCheckout.Offers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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

var logger = app.Logger;

var sampleStockKeepingUnits = new StockKeepingUnit[] {
    new('A', 50, new MultiBuyOffer(3,130)),
    new('B', 30, new MultiBuyOffer(2,45)),
    new('C', 20, null),
    new('D', 15, null)
};

var checkoutApi = app.MapGroup("/checkout");

checkoutApi.MapGet("/", () =>
{
    logger.LogInformation("Fetching all stock keeping units.");
    return sampleStockKeepingUnits;
});

checkoutApi.MapGet("/{id}", (string id) =>
{
    if (id.Length != 1)
    {
        logger.LogWarning("Invalid ID length: {ID}", id);
        return Results.BadRequest("ID must be a single character.");
    }

    char charId = id[0];
    logger.LogInformation("Fetching stock keeping unit with ID: {ID}", charId);
    return sampleStockKeepingUnits.FirstOrDefault(a => a.ID == charId) is { } sampleUnit
        ? Results.Ok(sampleUnit)
        : Results.NotFound();
});

checkoutApi.MapPost("/{basket}", (string basket) =>
{
    logger.LogInformation("Calculating total price for basket: {Basket}", basket);
    var result = Checkout.Calculate(basket, sampleStockKeepingUnits);
    if (result.HasErrors)
    {
        logger.LogWarning("Errors occurred while calculating total price: {Errors}", string.Join(", ", result.Errors));
        return Results.BadRequest(result.Errors);
    }
    logger.LogInformation("Total price calculated successfully: {TotalPrice}", result.TotalPrice);
    return Results.Ok(result.TotalPrice);
});

app.Run();

[JsonSerializable(typeof(StockKeepingUnit[]))]
[JsonSerializable(typeof(Offer))]
[JsonSerializable(typeof(List<string>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}