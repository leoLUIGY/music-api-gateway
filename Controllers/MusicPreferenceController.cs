using Microsoft.AspNetCore.Mvc;
using music_api_gateway.DTOs.Music;
using music_api_gateway.DTOs.Preference;

namespace music_api_gateway.Controllers
{
    [ApiController]
    [Route("api/preference")]
    public class MusicPreferenceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MusicPreferenceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Obtém uma preferencia de musica especifica
        /// </summary>
        /// <param name="id">ID da preferencia</param>
        /// <returns>Retorna informações sobre a preferencia de musica especifica</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPreference(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"http://localhost:5002/preference/{id}");

            var content = await response.Content.ReadAsStringAsync();

            return Content(
                    content,
                    response.Content.Headers.ContentType?.ToString()
                );
        }

        /// <summary>
        /// Obtem todas as preferencias de musicas
        /// </summary>
        /// <returns>Retorna uma lista com dados de todas as preferencias de musicas</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPreferences()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("http://localhost:5002/preferences");

            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = response.Content.Headers.ContentType?.ToString()
            };
        }


        /// <summary>
        /// Cria uma nova preferencia de musica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePreference([FromBody] CreatePreferenceDto preference)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsJsonAsync("http://localhost:5002/preference", preference);

            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = response.Content.Headers.ContentType?.ToString()
            };
        }

        /// <summary>
        /// Altera uma preferencia de musica existente
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePreference([FromBody] UpdatePreferenceDto preference)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PutAsJsonAsync($"http://localhost:5002/preference", preference);

            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = response.Content.Headers.ContentType?.ToString()
            };
        }

        /// <summary>
        /// Deletar uma preferencia de musica
        /// </summary>
        /// <param name="id">Id da preferencia</param>
        /// <returns></returns>
        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePreference(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.DeleteAsync($"http://localhost:5002/preference/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return NoContent();
            }

            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = response.Content.Headers.ContentType?.ToString()
            };
        }
    }
}
