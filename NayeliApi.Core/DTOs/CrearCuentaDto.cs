using System.ComponentModel.DataAnnotations;

namespace NayeliApi.Core.DTOs;

public class CrearCuentaDto
{
    [Required(ErrorMessage = "El ID del cliente es requerido")]
    public Guid ClienteId { get; set; }
}
