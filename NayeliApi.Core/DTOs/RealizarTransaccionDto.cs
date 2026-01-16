using System.ComponentModel.DataAnnotations;

namespace NayeliApi.Core.DTOs;

public class RealizarTransaccionDto
{
    [Required(ErrorMessage = "El número de cuenta es requerido")]
    public string NumeroCuenta { get; set; } = string.Empty;

    [Required(ErrorMessage = "El monto es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Monto { get; set; }
}
