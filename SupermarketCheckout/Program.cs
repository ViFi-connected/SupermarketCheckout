using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.OpenApi.Models;
using SupermarketCheckout.Logic;
using SupermarketCheckout.Models;
using SupermarketCheckout.Models.Offers;
using SupermarketCheckout.Services;
using SupermarketCheckout.Shared;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Supermarket Checkout API", Version = "v1" });
});

builder.Services.AddSingleton<IStockKeepingUnitService, StockKeepingUnitService>();
builder.Services.AddTransient<Checkout>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Supermarket Checkout API v1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

var logger = app.Logger;

var checkoutApi = app.MapGroup("/checkout");

checkoutApi.MapGet("/", (IStockKeepingUnitService stockService) =>
{
    logger.LogInformation("Fetching all stock keeping units.");
    return stockService.GetAll();
});

checkoutApi.MapGet("/{id}", (string id, IStockKeepingUnitService stockService) =>
{
    if (id.Length != 1)
    {
        logger.LogWarning("Invalid ID length: {ID}", id);
        return Results.BadRequest("ID must be a single character.");
    }

    char charId = id[0];
    logger.LogInformation("Fetching stock keeping unit with ID: {ID}", charId);
    return stockService.GetById(charId) is { } sampleUnit
        ? Results.Ok(sampleUnit)
        : Results.NotFound();
});

checkoutApi.MapPost("/{basket}", (string basket, Checkout checkout) =>
{
    logger.LogInformation("Calculating total price for basket: {Basket}", basket);
    var result = checkout.Calculate(basket);
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