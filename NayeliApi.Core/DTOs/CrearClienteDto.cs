using System.ComponentModel.DataAnnotations;

namespace NayeliApi.Core.DTOs;

public class CrearClienteDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es requerido")]
    public string Sexo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los ingresos son requeridos")]
    [Range(0, double.MaxValue, ErrorMessage = "Los ingresos deben ser mayores o iguales a 0")]
    public decimal Ingresos { get; set; }
}
