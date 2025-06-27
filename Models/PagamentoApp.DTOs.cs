using Microsoft.AspNetCore.Http;

namespace PagamentosApp.DTOs
{
  public class PessoaFormDto
  {
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
    public string Ramo { get; set; } = string.Empty;
    public IFormFile? Foto { get; set; }
  }
}