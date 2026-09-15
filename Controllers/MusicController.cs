using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using music_api_gateway.DTOs.Music;
using Microsoft.AspNetCore.Authorization;

namespace music_api_gateway.Controllers
{
    [ApiController]
    [Route("api/music")]
    [Authorize]
    public class MusicController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MusicController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Obtém uma musica especifica
        /// </summary>
        /// <param name="id">ID da musica</param>
        /// <returns>Retorna informações sobre a musica especifica</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMusic(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"http://localhost:5000/musica/{id}");

            var content = await response.Content.ReadAsStringAsync();

            return Content(
                    content,
                    response.Content.Headers.ContentType?.ToString()
                );
        }

        /// <summary>
        /// Obtem todas as musicas
        /// </summary>
        /// <returns>Retorna uma lista com dados de todas as musicas</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMusics()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("http://localhost:5000/musicas");

            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = response.Content.Headers.ContentType?.ToString()
            };
        }


        /// <summary>
        /// Cria uma nova musica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMusic([FromBody] CreateMusicDto music)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsJsonAsync("http://localhost:5000/musica",music);

            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = response.Content.Headers.ContentType?.ToString()
            };
        }

        /// <summary>
        /// Altera uma musica existente
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMusic([FromBody] UpdateMusicDto music)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PutAsJsonAsync($"http://localhost:5000/musica", music);

            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = response.Content.Headers.ContentType?.ToString()
            };
        }

        /// <summary>
        /// Deletar uma musica
        /// </summary>
        /// <param name="id">Id da musica</param>
        /// <returns></returns>
        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMusic(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.DeleteAsync($"http://localhost:5000/musica/{id}");

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
