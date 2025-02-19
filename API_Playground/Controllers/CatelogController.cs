using API_Playground.Services;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary;
using SQLite;

namespace API_Playground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatelogController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Product>> Get()
        {
            using (var db = new SQLiteConnection(DatabaseService.DatabasePath, SQLiteOpenFlags.ReadOnly))
            {
                string selectQuery = "SELECT * FROM Product";
                List<Product>? products = db.Query<Product>(selectQuery);

                if (products != null && products.Count > 0)
                {
                    return Ok(products);
                }
                else return NotFound("No products was found");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct([FromRoute]int id)
        {
            using (var db = new SQLiteConnection(DatabaseService.DatabasePath, SQLiteOpenFlags.ReadOnly))
            {
                string selectQuery = $"SELECT * FROM Product WHERE Id = ?";
                Product? product = db.Query<Product>(selectQuery, id).FirstOrDefault() ?? null;

                if (product != null)
                {
                    return Ok(product);
                }
                else return NotFound($"No product with the id '{id}' was found");
            }
        }

        [HttpGet("test")]
        public Task<List<Product>> ApiServiceGetCatelog()
        {
            ProductApiService pas = new();

            return pas.GetCatelog();
        }

        [HttpGet("test/{id}")]
        public Task<Product> ApiServiceGetProduct([FromRoute]int id)
        {
            ProductApiService pas = new();
            return pas.GetProduct(id);
        }
    }
}
