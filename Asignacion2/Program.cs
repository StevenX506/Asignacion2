using Asignacion2.Models;
using Asignacion2.Services;
using MongoDB.Bson.Serialization.Conventions;

var builder = WebApplication.CreateBuilder(args);

var conventionPack = new ConventionPack
{
    new CamelCaseElementNameConvention()
};

ConventionRegistry.Register(
    "CamelCase",
    conventionPack,
    type => true
);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/ventas", async (MongoDbService mongo) =>
{
    var ventas = await mongo.ObtenerVentasAsync();
    return Results.Ok(ventas);
});

app.MapGet("/api/ventas/facturacion-total", async (MongoDbService mongo) =>
{
    var total = await mongo.ObtenerFacturacionTotalAsync();
    return Results.Ok(new { facturacionTotal = total });
});

app.Run();
