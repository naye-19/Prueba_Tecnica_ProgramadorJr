namespace NayeliApi.Core.Entities;

public class Transaccion
{
    public Guid Id { get; set; }
    public Guid CuentaBancariaId { get; set; }
    public CuentaBancaria CuentaBancaria { get; set; } = null!;
    public TipoTransaccion Tipo { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public decimal SaldoDespues { get; set; }
}
