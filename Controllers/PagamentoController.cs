using Microsoft.AspNetCore.Mvc;
using PagamentosApp.Models;
using PagamentosApp.DTOs;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PagamentosApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagamentoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{mes}")]
        public IActionResult GetPorMes(int mes)
        {
            var pagamentos = _context.Pagamentos
                .Where(p => p.Mes == mes)
                .Select(p => new
                {
                    id = p.Id,
                    pessoaId = p.PessoaId,
                    mes = p.Mes,
                    ano = p.Ano,
                    dataPagamento = p.DataPagamento
                })
                .ToList();

            return Ok(pagamentos);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int pessoaId, [FromQuery] int? ano = null)
        {
            var anoAtual = ano ?? DateTime.Now.Year;

            var pagamentos = _context.Pagamentos
                .Where(p => p.PessoaId == pessoaId && p.Ano == anoAtual)
                .Select(p => new
                {
                    id = p.Id,
                    pessoaId = p.PessoaId,
                    mes = p.Mes,
                    ano = p.Ano,
                    dataPagamento = p.DataPagamento
                })
                .ToList();

            return Ok(pagamentos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PagamentoDto dto)
        {
            try
            {
                var pessoaExiste = _context.Pessoas.Any(p => p.Id == dto.PessoaId);
                if (!pessoaExiste)
                {
                    return BadRequest("Pessoa não encontrada.");
                }

                var duplicado = _context.Pagamentos.Any(p =>
                    p.PessoaId == dto.PessoaId && p.Mes == dto.Mes && p.Ano == dto.Ano);

                if (duplicado)
                {
                    return Conflict("Pagamento já existe para essa pessoa, mês e ano.");
                }

                // ✅ Converte corretamente o horário atual para horário de Brasília e garante UTC
                var brasilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

                // 🕒 Pega o horário local do Brasil (ex: 14:00)
                var horarioBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasilTimeZone);

                // ✅ Agora converte esse horário *local* de volta para UTC — mas fixando Kind = Utc
                var dataPagamentoUtc = TimeZoneInfo.ConvertTimeToUtc(horarioBrasilia, brasilTimeZone);


                var pagamento = new Pagamento
                {
                    PessoaId = dto.PessoaId,
                    Mes = dto.Mes,
                    Ano = dto.Ano,
                    DataPagamento = dataPagamentoUtc
                };

                _context.Pagamentos.Add(pagamento);
                _context.SaveChanges();

                return Created($"api/pagamento/{pagamento.Id}", pagamento);
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine($"❌ DbUpdateException: {innerMessage}");
                return StatusCode(500, $"Erro ao salvar no banco: {innerMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }

        [HttpGet("pessoa/{pessoaId}")]
        public IActionResult GetPorPessoa(int pessoaId)
        {
            var pagamentos = _context.Pagamentos
                .Where(p => p.PessoaId == pessoaId)
                .Select(p => new
                {
                    id = p.Id,
                    pessoaId = p.PessoaId,
                    mes = p.Mes,
                    ano = p.Ano,
                    dataPagamento = p.DataPagamento
                })
                .ToList();

            return Ok(pagamentos);
        }

        [HttpGet("historico")]
        public IActionResult GetHistorico()
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

            var historico = _context.Pagamentos
                .Include(p => p.Pessoa)
                .OrderByDescending(p => p.DataPagamento)
                .Select(p => new
                {
                    nome = p.Pessoa.Nome,
                    ramo = p.Pessoa.Ramo,
                    dataPagamento = TimeZoneInfo.ConvertTimeFromUtc(p.DataPagamento, timezone)
                                     .ToString("dd/MM/yyyy HH:mm"),
                    status = "Pago",
                    mes = p.Mes
                })
                .ToList();

            return Ok(historico);
        }
    }
}
