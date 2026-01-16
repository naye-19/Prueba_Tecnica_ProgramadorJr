namespace NayeliApi.Core.Entities;

public class CuentaBancaria
{
    public Guid Id { get; set; }
    public string NumeroCuenta { get; set; } = string.Empty;
    public Guid ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public decimal SaldoActual { get; set; }

    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}
