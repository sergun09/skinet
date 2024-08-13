using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class RegisterDTO
{
    [Required(ErrorMessage = "Le prénom est obligatoire")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom est obligatoire")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'email est obligatoire")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string Password { get; set; } = string.Empty;

}
