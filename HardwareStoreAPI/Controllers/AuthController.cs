using Common.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace HardwareStoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(LoginDto loginDto)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    string jwtToken = await _authService.Authenticate(loginDto);

                    return Ok(jwtToken);

                }
                else
                {
                    var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                    return BadRequest(validationErrors);
                }
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto userDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = await _authService.CreateUser(userDto);

                    return Ok(result);
                }
                else
                {
                    var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                    return BadRequest(validationErrors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
