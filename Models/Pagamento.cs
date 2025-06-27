using System;
using System.Text.Json.Serialization;

namespace PagamentosApp.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int PessoaId { get; set; }

        [JsonIgnore] // Ignora este campo ao receber JSON via POST
        public Pessoa Pessoa { get; set; } = null!;

        public DateTime DataPagamento { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
    }
}
