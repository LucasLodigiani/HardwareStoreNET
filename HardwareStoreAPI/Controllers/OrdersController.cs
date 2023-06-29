using Common.Dtos;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

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
        [Authorize(Roles = "Admin")]
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
                User userJwtData = GetUserJWTData();
                orderModifyDto.UserId = userJwtData.Id;

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
                User userJwtData = GetUserJWTData();
                orderDto.UserId = userJwtData.Id;

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

        private User GetUserJWTData()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Decodificar el JWT
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);

            User user = new User();

            // Acceder a los campos del JWT
            user.Id = Guid.Parse(decodedToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
            user.Username = decodedToken.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
            user.Role = decodedToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            return user;
        }
    }
}
