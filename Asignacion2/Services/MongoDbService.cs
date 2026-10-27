
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
}
