using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary;

namespace API_Playground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatelogController : ControllerBase
    {
        private List<Product> catelog = new List<Product>
        {
            new(0, 100, "Vodka", "70%"),
            new(1, 200, "Gin", "30%"),
            new(2, 300, "Rum", "50%"),
            new(3, 400, "Snaps", "80%"),
            new(4, 500, "Wine", "18%"),
            new(5, 600, "Moonshine", "96%"),
        };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(catelog);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute]int id)
        {
            Product? product = catelog.FirstOrDefault(sorting => sorting.Id == id);

            if (product == null)
            {
                return NotFound("Product not found");
            }
            else
            {
                return Ok(product);
            }
        }
    }
}
