using AppDev_MidtermExam_.DTO;
using AppDev_MidtermExam_.Models;
using AppDev_MidtermExam_.Properties.Data;
using AppDev_MidtermExam_.Services;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppDev_MidtermExam_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseHelper _context;
        private readonly TokenService _tokenService;

        public AuthController(DatabaseHelper context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            var sql = @"SELECT UserId, Username, PasswordHash, Role
                    FROM Users
                    WHERE Username = @Username";

            using var connection = _context.CreateConnection();
            var user = connection.QueryFirstOrDefault<Users>(sql, new { dto.Username });

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password." });

            if (dto.Password != user.PasswordHash)
                return Unauthorized(new { message = "Invalid username or password." });

            // Generate the JWT
            var token = _tokenService.GenerateToken(user);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }
    }
}
