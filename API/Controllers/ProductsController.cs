using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly DataContext _context;

    public ProductsController(ILogger<ProductsController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    // GET /products
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        var products = _context.Products.ToList();
        return Ok(products);
    }

    // GET /products/1
    [HttpGet("{id}")]
    public ActionResult<Product> GetProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    // POST /products
    [HttpPost]
    public ActionResult<Product> CreateProduct(Product product)
    {
        product.CreatedDate = DateTime.Now;
        product.LastUpdatedDate = DateTime.Now;

        _context.Products.Add(product);
        var saved = _context.SaveChanges() > 0;

        if (saved)
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);

        return BadRequest("Failed to create product");
    }

    // PUT /products/1
    [HttpPut("{id}")]
    public ActionResult<Product> UpdateProduct(int id, Product product)
    {
        var existing = _context.Products.Find(id);
        if (existing == null) return NotFound();

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.IsOnSale = product.IsOnSale;
        existing.SalePrice = product.SalePrice;
        existing.CurrentStock = product.CurrentStock;
        existing.ImageUrl = product.ImageUrl;
        existing.LastUpdatedDate = DateTime.Now;

        var saved = _context.SaveChanges() > 0;
        if (saved) return Ok(existing);
        return BadRequest("Failed to update product");
    }

    // DELETE /products/1
    [HttpDelete("{id}")]
    public ActionResult DeleteProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        var saved = _context.SaveChanges() > 0;
        if (saved) return NoContent();
        return BadRequest("Failed to delete product");
    }
}
