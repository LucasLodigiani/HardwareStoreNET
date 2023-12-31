﻿using Common.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HardwareStoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IList<UserDto>>> GetUsers()
        {
            try
            {
                var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (userRole != "Admin")
                {
                    return Forbid();
                }

                IList<UserDto> userDtos = await _userService.GetAllUsers();

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (userRole != "Admin")
                {
                    return Forbid();
                }

                if (id == Guid.Empty)
                {
                    throw new Exception("No hay nada ingresado en el id");
                }

                bool result = await _userService.DeleteUser(id);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ModifyUser(UserDto userDto)
        {
            try
            {
                var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (userRole != "Admin")
                {
                    return Forbid();
                }


                if (ModelState.IsValid)
                {
                    bool result = await _userService.UpdateUser(userDto);
                    if (result == false)
                    {
                        return NotFound();
                    }

                    return NoContent();
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

        [HttpGet("id")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid id)
        {

            try
            {
                var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (userRole != "Admin")
                {
                    return Forbid();
                }

                if (id == Guid.Empty)
                {
                    throw new Exception("No hay nada ingresado en el id");
                }

                UserDto? user = await _userService.GetUserById(id);

                if(user == null)
                {
                    return NotFound();
                }

                return Ok(user);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



    }
}
