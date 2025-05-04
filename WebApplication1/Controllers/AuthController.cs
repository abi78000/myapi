using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebApplication1.Models;

namespace UserAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static string _connectionString => "Host=db.nxfhsxslwsatkqwoldqu.supabase.co;Port=5432;Username=postgres;Password=Abiraj@1234*;Database=postgres";


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            // Check if email already exists
            var checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            using var checkCmd = new NpgsqlCommand(checkQuery, connection);
            checkCmd.Parameters.AddWithValue("@Email", model.Email);
            if ((long)checkCmd.ExecuteScalar() > 0)
                return BadRequest("Email already exists.");

            // Insert new user
            var insertQuery = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
            using var insertCmd = new NpgsqlCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@Name", model.Name);
            insertCmd.Parameters.AddWithValue("@Email", model.Email);
            insertCmd.Parameters.AddWithValue("@Password", model.Password);
            insertCmd.ExecuteNonQuery();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", model.Email);
            command.Parameters.AddWithValue("@Password", model.Password);

            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count == 1)
            {
                return Ok("Login successful.");
            }

            return Unauthorized("Invalid credentials.");
        }
    }
}
