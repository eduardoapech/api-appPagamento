using Microsoft.AspNetCore.Mvc;
using PagamentosApp.Models;
using PagamentosApp.DTOs;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PagamentosApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _baseUrl = "https://api-apppagamento-1.onrender.com"; // ✅ URL do Render

        public PessoaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_context.Pessoas.ToList());

        [HttpPost]
        [RequestSizeLimit(10_000_000)] // até 10MB
        public async Task<IActionResult> Post([FromForm] PessoaFormDto dto)
        {
            string? fotoUrl = null;

            if (dto.Foto != null && dto.Foto.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Foto.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Foto.CopyToAsync(stream);
                }

                // ✅ Corrigido: agora a URL tem o domínio completo
                fotoUrl = $"{_baseUrl}/uploads/{fileName}".Replace("\\", "/");
            }

            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Idade = dto.Idade,
                Ramo = dto.Ramo,
                FotoUrl = fotoUrl
            };

            _context.Pessoas.Add(pessoa);
            _context.SaveChanges();

            return Created($"api/pessoa/{pessoa.Id}", pessoa);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa == null) return NotFound();
            _context.Pessoas.Remove(pessoa);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        [RequestSizeLimit(10_000_000)] // Até 10MB se enviar imagem novamente
        public async Task<IActionResult> Put(int id, [FromForm] PessoaFormDto dto)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa == null) return NotFound();

            pessoa.Nome = dto.Nome;
            pessoa.Idade = dto.Idade;
            pessoa.Ramo = dto.Ramo;

            if (dto.Foto != null && dto.Foto.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Foto.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Foto.CopyToAsync(stream);
                }

                // ✅ Corrigido: URL da imagem atualizada com domínio do Render
                pessoa.FotoUrl = $"{_baseUrl}/uploads/{fileName}".Replace("\\", "/");
            }

            _context.SaveChanges();
            return Ok(pessoa);
        }
    }
}
