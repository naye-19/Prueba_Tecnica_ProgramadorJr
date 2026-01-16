using NayeliApi.Core.Entities;

namespace NayeliApi.Core.DTOs;

public class TransaccionResponseDto
{
    public Guid Id { get; set; }
    public TipoTransaccion Tipo { get; set; }
    public string TipoDescripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public decimal SaldoDespues { get; set; }
}
