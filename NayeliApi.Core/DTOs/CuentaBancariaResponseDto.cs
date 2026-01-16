namespace NayeliApi.Core.DTOs;

public class CuentaBancariaResponseDto
{
    public Guid Id { get; set; }
    public string NumeroCuenta { get; set; } = string.Empty;
    public Guid ClienteId { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public decimal SaldoActual { get; set; }
}
