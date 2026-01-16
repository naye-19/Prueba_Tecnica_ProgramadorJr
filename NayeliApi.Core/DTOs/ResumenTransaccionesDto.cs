namespace NayeliApi.Core.DTOs;

public class ResumenTransaccionesDto
{
    public decimal TotalDepositos { get; set; }
    public decimal TotalRetiros { get; set; }
    public decimal SaldoFinal { get; set; }
    public List<TransaccionResponseDto> Transacciones { get; set; } = new();
}
