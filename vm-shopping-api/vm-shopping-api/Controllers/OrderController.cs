using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using vm_shopping_business.Interfaces;
using vm_shopping_models.Entities;

namespace vm_shopping_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   
    public class OrderController : ControllerBase
    {
        public readonly IOrderBusiness orderBusiness;

        public OrderController(IOrderBusiness orderBusiness)
        {
            this.orderBusiness = orderBusiness;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok("VM Shopping running");
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]OrderRequest orderRequest)
        {           
            try
            {
                OrderResponse orderResponse = await orderBusiness.CreateOrderAsync(orderRequest);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                //ToDo: Logg
                return StatusCode(500);
            }
        }
    }
}
