namespace PagamentosApp.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public string Ramo { get; set; } = string.Empty; // Lobinho ou Escoteiro
        public string? FotoUrl { get; set; } // opcional
    }
}
