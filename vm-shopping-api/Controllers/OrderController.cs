using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public readonly INotificationBusiness notificationBusiness;

        public OrderController(IOrderBusiness orderBusiness, INotificationBusiness notificationBusiness)
        {
            this.orderBusiness = orderBusiness;
            this.notificationBusiness = notificationBusiness;
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

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult> GetClientOrders([FromQuery(Name = "email")] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return new BadRequestResult();
                }

                List<OrderResponse> orders = await orderBusiness.GetClientOrders(email);
                return Ok(orders);
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

        [HttpPost]
        [Route("notification")]
        public async Task<ActionResult> OrderPaymentNotification([FromBody] PaymentNotificationRequest paymentNotificationRequest)
        {
            try
            {
                var notificationResponse = await notificationBusiness.PaymentNotificationSync(paymentNotificationRequest);
                return Ok(notificationResponse);
            }
            catch (Exception ex)
            {
                //ToDo: Logg
                return StatusCode(500);
            }
        }
    }
}
