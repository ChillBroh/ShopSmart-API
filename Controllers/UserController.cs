using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using webApi.DTOs.UserDtos;
using webApi.Interfaces.Service;
using webApi.Models;
using webApi.Services;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto user)
    {
        if (await _userService.EmailExists(user.Email) && await _userService.UserExists(user.UserName))
        {
            return BadRequest("Both the email and username you entered are already taken.");
        }
        else if(await _userService.EmailExists(user.Email))
        {
            return BadRequest("Email is already Exsits");
        }
        else if(await _userService.UserExists(user.UserName))
        {
            return BadRequest("User name is already Exsits");
        }
       
        var newUser = new ApplicationUser
        {
            UserId = Guid.NewGuid(),
            UserName = user.UserName,
            Email = user.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Role = user.Role.ToString(), 
            PhoneNumber = user.PhoneNumber,
            AdminOrCSRApproved = user.Role != webApi.Models.Enums.UserRole.Customer,
        };
        await _userService.CreateUser(newUser);
        return Ok(new { message = "User registered successfully." });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModelDto loginModel)
    {
        if(loginModel.Email == null)
        {
            return BadRequest("Email is Requierd");
        }
        var user = await _userService.GetUserByEmail(loginModel.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
        if(user.AdminOrCSRApproved == false)
        {
            return Unauthorized("The user account requires approval by an Administrator or Customer Service Representative.");
        }

        var loggedInUser = new LoggedInUserDto
        {
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName,
            UserId = user.UserId,
            IsCartAvailable = false,
            Token = GenerateJwtToken(user)
        };
        return Ok(new { loggedInUser });
    }

    //[HttpGet("GetUserRole")]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "CSR, Administrator")] 
    //public IActionResult GetUserRole()
    //{
    //    // Retrieve the current user's claims from the token
    //    var claimsIdentity = User.Identity as ClaimsIdentity;

    //    if (claimsIdentity != null)
    //    {
    //        // Extract the role claim
    //        var roleClaim = claimsIdentity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

    //        if (roleClaim != null)
    //        {
    //            // Return the role in the response
    //            return Ok(new { Role = roleClaim.Value });
    //        }
    //    }

    //    return BadRequest("Role not found for the user.");
    //}

    [HttpGet("GetPendingApprovalUsers")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "CSR, Administrator")]
    public async Task<IActionResult> GetPendingApprovalUsers()
    {
        var users = await _userService.GetUsersPendingApprovalAsync();
        return Ok(users);
    }

    [HttpPost("ApproveUser/{userId}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "CSR, Administrator")]
    public async Task<IActionResult> ApproveUser(Guid userId)
    {
        var result = await _userService.ApproveUserByUserIdAsync(userId);

        if (!result)
        {
            return NotFound("User not found or already approved.");
        }

        return Ok(new { message = "User approved successfully." });
    }




    private string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "DefaultSecretKeyIsThisOne");
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = signingCredentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]    
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
