namespace music_api_gateway.DTOs.Preference
{
    public class CreatePreferenceDto
    {
        public string nome { get; set; }
        public string genero { get; set; }
        public string artista { get; set; }
        public string data_criacao { get; set; }
    }
}
