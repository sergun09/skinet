using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class CreateProductDTO
{
    [Required(ErrorMessage ="Le nom du produit est obligatoire")]
    public string Name { get; init; }

    [Required(ErrorMessage ="La description du produit est obligatoire")]
    public string Description { get; init; }
    
    [Range(0.01,double.MaxValue, ErrorMessage ="Le prix est obligatoire")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "La marque du produit est obligatoire")]
    public string Type { get; set; }
    
    [Required(ErrorMessage ="La marque du produit est obligatoire")]
    public string Brand { get; set; }

    [Range(1,int.MaxValue,ErrorMessage ="La quantité en stock doit être au minimum de 1")]
    public int QuantityInStock { get; set; }

    [Required]
    public IFormFile File { get; set; }

    public string? PictureUrl { get; set; }
}
