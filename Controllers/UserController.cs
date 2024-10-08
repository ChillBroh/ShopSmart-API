using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webApi.DTOs.UserDtos;
using webApi.Interfaces.Service;
using webApi.Models;
using webApi.Models.Enums;
using webApi.Services;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JWTService _jwtService;

    public UserController(IUserService userService, JWTService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto user)
    {
        if (await _userService.EmailExists(user.Email) && await _userService.UserExists(user.UserName))
        {
            return BadRequest("Both the email and username you entered are already taken.");
        }
        else if (await _userService.EmailExists(user.Email))
        {
            return BadRequest("Email is already Exsits");
        }
        else if (await _userService.UserExists(user.UserName))
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
            ActiveUser = true
        };
        await _userService.CreateUser(newUser);
        return Ok(new { message = "User registered successfully." });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModelDto loginModel)
    {
        if (loginModel.Email == null)
        {
            return BadRequest("Email is Requierd");
        }
        var user = await _userService.GetUserByEmail(loginModel.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
        if (user.AdminOrCSRApproved == false)
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
            Token = _jwtService.GenerateJwtToken(user)
        };
        return Ok(new { loggedInUser });
    }

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

    [HttpPut("ActivateOrDeactivateUser")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "CSR, Administrator")]
    public async Task<IActionResult> ActivateOrDeactivateUser(Guid userId,UserActivateDeactivate userActivateDeactivate)
    {
        var user = await _userService.GetUserById(userId);
        if (UserActivateDeactivate.Activate.Equals(userActivateDeactivate))
        {
            if (user.ActiveUser)
            {
                return Ok(new { message = "User alredy in active state." });
            }
            else
            {
               await _userService.ActiveateDeactivateUser(userId, userActivateDeactivate);
               return Ok(new { message = "User activeted successfully." });
            }
        }
        else
        {
            if (!user.ActiveUser)
            {
                return Ok(new { message = "User alredy in deactive state." });
            }
            else
            {
                await _userService.ActiveateDeactivateUser(userId, userActivateDeactivate);
                return Ok(new { message = "User deactiveted successfully." });
            }

        }
    }




}
