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

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);


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

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);


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


    [HttpGet("search")]
public ActionResult<IEnumerable<Product>> SearchProducts(
    [FromQuery] string? name = null,
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null,
    [FromQuery] bool? isOnSale = null,
    [FromQuery] bool? inStock = null,
    [FromQuery] string sortBy = "name",
    [FromQuery] string sortOrder = "asc")
{
    var query = _context.Products.AsQueryable();

    // filters
    if (!string.IsNullOrEmpty(name))
        query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));

    if (minPrice.HasValue)
        query = query.Where(p => p.Price >= minPrice.Value);

    if (maxPrice.HasValue)
        query = query.Where(p => p.Price <= maxPrice.Value);

    if (isOnSale.HasValue)
        query = query.Where(p => p.IsOnSale == isOnSale.Value);

    if (inStock.HasValue && inStock.Value)
        query = query.Where(p => p.CurrentStock > 0);

    // run the query, then sort in memory (works fine with SQLite)
    var products = query.ToList();

    products = sortBy.ToLower() switch
    {
        "price" => sortOrder.ToLower() == "desc"
            ? products.OrderByDescending(p => p.Price).ToList()
            : products.OrderBy(p => p.Price).ToList(),
        "created" => sortOrder.ToLower() == "desc"
            ? products.OrderByDescending(p => p.CreatedDate).ToList()
            : products.OrderBy(p => p.CreatedDate).ToList(),
        "stock" => sortOrder.ToLower() == "desc"
            ? products.OrderByDescending(p => p.CurrentStock).ToList()
            : products.OrderBy(p => p.CurrentStock).ToList(),
        _ => sortOrder.ToLower() == "desc"
            ? products.OrderByDescending(p => p.Name).ToList()
            : products.OrderBy(p => p.Name).ToList()
    };

    return Ok(products);
}

}
