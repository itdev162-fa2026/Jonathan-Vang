namespace Domain;

using System.ComponentModel.DataAnnotations;
using Domain.Validation;

[ValidSalePrice]
public class Product
{
    public int Id { get; set; }              // auto PK
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsOnSale { get; set; }
    public decimal? SalePrice { get; set; }  // nullable
    public int CurrentStock { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
}
