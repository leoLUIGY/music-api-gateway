using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace music_api_gateway.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// Registra um novo usuário.
        /// </summary>
        /// <param name="request">Dados necessários para o cadastro, contendo e-mail e senha.</param>
        /// <returns>Retorna os dados do usuário criado.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var client = _httpClientFactory.CreateClient();

            var domain = _configuration["Auth0:Domain"];
            var managementToken = await GetManagementToken();

            var body = new
            {
                email = request.Email,
                password = request.Password,
                connection = _configuration["Auth0:Connection"],
                email_verified = false
            };

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://{domain}/api/v2/users"
            );

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", managementToken);

            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.SendAsync(httpRequest);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, content);

            return Ok(JsonSerializer.Deserialize<object>(content));
        }

        /// <summary>
        /// Realiza a autenticação de um usuário.
        /// </summary>
        /// <param name="request">Credenciais do usuário, contendo e-mail e senha.</param>
        /// <returns>Retorna o token de acesso utilizado para autenticar as requisições.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient();

            var domain = _configuration["Auth0:Domain"];

            var body = new
            {
                grant_type = "password",
                username = request.Email,
                password = request.Password,
                audience = _configuration["Auth0:Audience"],
                client_id = _configuration["Auth0:ClientId"],
                client_secret = _configuration["Auth0:ClientSecret"],
                realm = _configuration["Auth0:Connection"],
                scope = "openid profile email"
            };

            var response = await client.PostAsJsonAsync(
                $"https://{domain}/oauth/token",
                body
            );

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, content);

            return Ok(JsonSerializer.Deserialize<object>(content));
        }

        /// <summary>
        /// Obtém os dados do usuário autenticado.
        /// </summary>
        /// <returns>Retorna os dados do usuário atualmente autenticado.</returns>
        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var managementToken = await GetManagementToken();

            var client = _httpClientFactory.CreateClient();

            var domain = _configuration["Auth0:Domain"];

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://{domain}/api/v2/users/{Uri.EscapeDataString(userId)}"
            );

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", managementToken);

            var response = await client.SendAsync(httpRequest);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, content);

            return Ok(JsonSerializer.Deserialize<object>(content));
        }

        /// <summary>
        /// Remove o usuário autenticado.
        /// </summary>
        /// <returns>Retorna uma resposta sem conteúdo após a remoção do usuário.</returns>
        [Authorize]
        [HttpDelete("user")]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var managementToken = await GetManagementToken();

            var client = _httpClientFactory.CreateClient();

            var domain = _configuration["Auth0:Domain"];

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Delete,
                $"https://{domain}/api/v2/users/{Uri.EscapeDataString(userId)}"
            );

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", managementToken);

            var response = await client.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, content);
            }

            return NoContent();
        }


        private async Task<string> GetManagementToken()
        {
            var client = _httpClientFactory.CreateClient();

            var domain = _configuration["Auth0:Domain"];

            var body = new
            {
                client_id = _configuration["Auth0:ManagementClientId"],
                client_secret = _configuration["Auth0:ManagementClientSecret"],
                audience = $"https://{domain}/api/v2/",
                grant_type = "client_credentials"
            };

            var response = await client.PostAsJsonAsync(
                $"https://{domain}/oauth/token",
                body
            );

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<JsonElement>();

            return result.GetProperty("access_token").GetString()!;
        }
    }
}
