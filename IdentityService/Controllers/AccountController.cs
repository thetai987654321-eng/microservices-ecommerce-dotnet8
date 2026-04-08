using IdentityService.DTOs;
using IdentityService.Interfaces;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

   
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        // Kiểm tra xem Email đã tồn tại chưa
        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email.ToLower()))
        {
            return BadRequest("Email is already taken");
        }

        var user = new ApplicationUser
        {
            FullName = registerDto.FullName,
            UserName = registerDto.Email.ToLower(),
            Email = registerDto.Email.ToLower()
        };

        
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) 
            return BadRequest(result.Errors);

        return new UserDto
        {
            Email = user.Email ?? string.Empty,
            FullName = user.FullName ?? string.Empty,
            Token = _tokenService.CreateToken(user)
        };
    }

    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        
        var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

        if (user == null) return Unauthorized("Invalid email or password");

        // Kiểm tra mật khẩu có khớp với bản mã hóa trong DB không
        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized("Invalid email or password");

        
        return new UserDto
        {
            Email = user.Email ?? string.Empty,
            FullName = user.FullName ?? string.Empty,
            Token = _tokenService.CreateToken(user)
        };
    }
}