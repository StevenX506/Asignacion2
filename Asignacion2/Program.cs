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

app.MapGet("/api/ventas/ticket-promedio", async (MongoDbService mongo) =>
{
    var promedio = await mongo.ObtenerTicketPromedioAsync();
    return Results.Ok(new { ticketPromedio = promedio });
});


app.MapGet("/api/ventas/productos-mas-vendidos", async (MongoDbService mongo) =>
{
    var productos = await mongo.ObtenerProductosMasVendidosAsync();
    return Results.Ok(productos);
});


app.MapGet("/api/ventas/ingresos-por-categoria", async (MongoDbService mongo) =>
{
    var ingresos = await mongo.ObtenerIngresosPorCategoriaAsync();
    return Results.Ok(ingresos);
});


app.MapGet("/api/ventas/ventas-por-ciudad", async (MongoDbService mongo) =>
{
    var resultado = await mongo.ObtenerVentasPorCiudadAsync();
    return Results.Ok(resultado);
});


app.MapGet("/api/ventas/desempeno-por-vendedor", async (MongoDbService mongo) =>
{
    var resultado = await mongo.ObtenerDesempenoPorVendedorAsync();
    return Results.Ok(resultado);
});

app.UseDefaultFiles();
app.UseStaticFiles();
app.Run();
