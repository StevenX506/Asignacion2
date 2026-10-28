using Asignacion2.Models;
using MongoDB.Driver;

namespace Asignacion2.Services;

public class MongoDbService
{
    private readonly IMongoCollection<Venta> _ventas;

    public MongoDbService(IConfiguration configuration)
    {
        var connectionString =
            configuration["MongoDB:ConnectionString"];

        var databaseName =
            configuration["MongoDB:DatabaseName"];

        var collectionName =
            configuration["MongoDB:CollectionName"];

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _ventas = database.GetCollection<Venta>(collectionName);
    }

    public async Task<List<Venta>> ObtenerVentasAsync()
    {
        return await _ventas.Find(_ => true).ToListAsync();
    }

    public async Task<decimal> ObtenerFacturacionTotalAsync()
    {
        var ventas = await _ventas.Find(_ => true).ToListAsync();
        return ventas.Sum(v => v.Total);
    }

    public async Task<decimal> ObtenerTicketPromedioAsync()
    {
        var ventas = await _ventas.Find(_ => true).ToListAsync();

        return ventas.Count > 0
            ? ventas.Average(v => v.Total)
            : 0;
    }

    public async Task<object> ObtenerProductosMasVendidosAsync()
    {
        var ventas = await _ventas.Find(_ => true).ToListAsync();

        var productos = ventas
            .SelectMany(v => v.Productos)
            .GroupBy(p => new { p.IdProducto, p.Nombre })
            .Select(g => new
            {
                idProducto = g.Key.IdProducto,
                nombre = g.Key.Nombre,
                cantidadVendida = g.Sum(p => p.Cantidad)
            })
            .OrderByDescending(p => p.cantidadVendida)
            .ToList();

        return productos;
    }

    public async Task<object> ObtenerIngresosPorCategoriaAsync()
    {
        var ventas = await _ventas.Find(_ => true).ToListAsync();

        var ingresos = ventas
            .SelectMany(v => v.Productos)
            .GroupBy(p => p.Categoria)
            .Select(g => new
            {
                categoria = g.Key,
                ingresos = g.Sum(p =>
                    p.Cantidad * p.PrecioUnitario * (1 - p.Descuento / 100m))
            })
            .OrderByDescending(g => g.ingresos)
            .ToList();

        return ingresos;
    }

    public async Task<object> ObtenerVentasPorCiudadAsync()
    {
        var ventas = await _ventas.Find(_ => true).ToListAsync();

        var resultado = ventas
            .GroupBy(v => v.Cliente.Ciudad)
            .Select(g => new
            {
                ciudad = g.Key,
                cantidadVentas = g.Count(),
                ingresos = g.Sum(v => v.Total)
            })
            .OrderByDescending(g => g.ingresos)
            .ToList();

        return resultado;
    }

    public async Task<object> ObtenerDesempenoPorVendedorAsync()
    {
        var ventas = await _ventas.Find(_ => true).ToListAsync();

        var resultado = ventas
            .GroupBy(v => new
            {
                v.Vendedor.Identificacion,
                v.Vendedor.Nombre
            })
            .Select(g => new
            {
                identificacion = g.Key.Identificacion,
                vendedor = g.Key.Nombre,
                cantidadVentas = g.Count(),
                ingresosTotales = g.Sum(v => v.Total),
                ticketPromedio = g.Average(v => v.Total)
            })
            .OrderByDescending(v => v.ingresosTotales)
            .ToList();

        return resultado;
    }

}
