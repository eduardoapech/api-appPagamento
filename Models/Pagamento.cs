using System;
using System.ComponentModel.DataAnnotations.Schema; // ✅ necessário para usar [Table]
using System.Text.Json.Serialization;

namespace PagamentosApp.Models
{
    [Table("pagamentos")] // 👈 força o EF a usar exatamente esse nome no banco
    public class Pagamento
    {
        public int Id { get; set; }
        public int PessoaId { get; set; }

        [JsonIgnore]
        public Pessoa Pessoa { get; set; } = null!;

        public DateTime DataPagamento { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
    }
}
