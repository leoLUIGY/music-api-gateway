namespace music_api_gateway.DTOs.Preference
{
    public class UpdatePreferenceDto
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string genero { get; set; }
        public string artista { get; set; }
        public string data_criacao { get; set; }
    }
}
