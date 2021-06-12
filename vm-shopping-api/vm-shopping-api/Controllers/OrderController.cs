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

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                OrderResponse orderResponse = await orderBusiness.GetOrder(id);
                if (orderResponse != null) {
                    return Ok(orderResponse);
                } 
                return NotFound();
            }
            catch (Exception ex)
            {
                //ToDo: Logg
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]OrderRequest orderRequest)
        {           
            try
            {
                OrderResponse orderResponse = await orderBusiness.CreateOrder(orderRequest);
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
