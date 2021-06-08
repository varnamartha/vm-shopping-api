using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vm_shopping_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   
    public class OrderController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok("Welcome to vm Shopping");
        }
    }
}
