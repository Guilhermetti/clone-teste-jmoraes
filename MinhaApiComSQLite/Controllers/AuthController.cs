using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(TokenService tokenService) : ControllerBase
    {
        /// <summary>
        /// Realiza login e gera um token JWT.
        /// </summary>
        /// <param name="user">Credenciais do usuário.</param>
        /// <returns>Token JWT em caso de sucesso.</returns>
        /// <response code="200">Retorna o token de autenticação.</response>
        /// <response code="401">Credenciais inválidas.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] User user)
        {
            if (user.Username == "admin" && user.Password == "admin")
            {
                var token = tokenService.GenerateToken(user.Username);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}