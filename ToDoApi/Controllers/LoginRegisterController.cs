using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using ToDoApi.Dtos.loginRegister;
using ToDoApi.Interfaces;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/loginRegister")]
    public class LoginRegisterController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public LoginRegisterController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        [SwaggerOperation("User Login")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<newUserDto>))]
        public async Task<IActionResult> Login(loginDto loginDetails)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(loginDetails.Username);
            if (user == null) return Unauthorized("Username not found");
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDetails.Password, false);
            if (!result.Succeeded) return Unauthorized("Username or password is invalid");
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(
                new newUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = await _tokenService.CreateToken(user),
                    Roles = roles.ToList(),
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] registerDto registerDetails)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);


                var appUser = new AppUser
                {
                    UserName = registerDetails.Username,
                    Email = registerDetails.Email,
                    PhoneNumber = registerDetails.Phonenumber,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDetails.Password);

                if (createdUser.Succeeded)
                {

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (!roleResult.Succeeded){
                        return StatusCode(500, roleResult.Errors);
                }
                var roles = await _userManager.GetRolesAsync(appUser);

                    return Ok(new newUserDto
                    {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = await _tokenService.CreateToken(appUser),
                        Roles = roles.ToList()
                    });
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
