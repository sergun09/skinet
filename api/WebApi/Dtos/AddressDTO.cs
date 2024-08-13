using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class AddressDTO
{
    [Required(ErrorMessage ="La Rue est obligatoire")]
    public  string Line1 { get; set; }
    public string? Line2 { get; set; }

    [Required(ErrorMessage = "La ville est obligatoire")]
    public string City { get; set; }

    [Required(ErrorMessage = "Le dépatement est obligatoire")]
    public string State { get; set; }

    [Required(ErrorMessage = "Le code postal est obligatoire")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Le pays est obligatoire")]
    public string Country { get; set; }
}
