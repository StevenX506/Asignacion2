using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Asignacion2.Models;

public class Venta
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("fecha")]
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string MetodoPago { get; set; } = string.Empty;
    public string Moneda { get; set; } = "CRC";

    public Cliente Cliente { get; set; } = new();
    public Vendedor Vendedor { get; set; } = new();

    public List<ProductoVenta> Productos { get; set; } = new();

    public decimal Subtotal { get; set; }
    public decimal DescuentoTotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Total { get; set; }

    public string CanalVenta { get; set; } = string.Empty;
    public bool TienePromocion { get; set; }
}