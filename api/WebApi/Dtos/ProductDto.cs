using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class ProductDto
{
    [Required(ErrorMessage = "Le nom du produit est obligatoire")]
    public string Name { get; set; }
    [Required(ErrorMessage = "La description du produit est obligatoire")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Le prix du produit est obligatoire")]
    [Range(1,15000, ErrorMessage = "Le prix doit être compris entre 1 et 15000")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Le type du produit est obligatoire")]
    public string Type { get; set; }
    [Required(ErrorMessage = "La marque du produit est obligatoire")]
    public string Brand { get; set; }
    [Required(ErrorMessage = "La quantité en stock du produit est obligatoire")]
    public int QuantityInStock { get; set; }
}
