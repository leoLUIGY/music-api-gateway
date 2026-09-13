namespace music_api_gateway.DTOs.Music
{
    public class CreateMusicDto
    {
        public string nome { get; set; }
        public string data_criacao { get; set; }
        public string nome_criador { get; set; }
        public string genero { get; set; }
    }
}
