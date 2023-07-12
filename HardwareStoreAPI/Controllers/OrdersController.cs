using Common.Dtos;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HardwareStoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<OrderViewDto>>> GetAllOrders()
        {
            try
            {
                IList<OrderViewDto> orders = await _orderService.GetAllOrders();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(OrderUpdateDto orderModifyDto)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                orderModifyDto.UserId = Guid.Parse(userId);

                if (ModelState.IsValid)
                {
                    var (status, order) = await _orderService.UpdateOrder(orderModifyDto);
                    if(status == 0)
                    {
                        return NotFound();
                    }
                    else if(status == 2)
                    {
                        return Unauthorized();
                    }
                    return Ok(order);
                }
                else
                {
                    var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                    return BadRequest(validationErrors);
                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderCreateDto orderDto)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                orderDto.UserId = Guid.Parse(userId);

                if (ModelState.IsValid)
                {
                    OrderViewDto newOrder = await _orderService.CreateOrder(orderDto);

                    return Ok(newOrder);
                }
                else
                {
                    var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                    return BadRequest(validationErrors);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
