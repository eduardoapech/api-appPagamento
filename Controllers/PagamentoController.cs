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
                // Verifica se a pessoa existe no banco
                var pessoaExiste = _context.Pessoas.Any(p => p.Id == dto.PessoaId);
                if (!pessoaExiste)
                {
                    return BadRequest("Pessoa não encontrada.");
                }

                var pagamento = new Pagamento
                {
                    PessoaId = dto.PessoaId,
                    Mes = dto.Mes,
                    Ano = dto.Ano,
                    DataPagamento = DateTime.UtcNow
                };

                _context.Pagamentos.Add(pagamento);
                _context.SaveChanges();

                return Created($"api/pagamento/{pagamento.Id}", pagamento);
            }
            catch (Exception ex)
            {
                // 🔴 Isso vai te mostrar a exceção real
                return StatusCode(500, $"Erro interno: {ex.Message}");
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
            var historico = _context.Pagamentos
                .Include(p => p.Pessoa)
                .OrderByDescending(p => p.DataPagamento)
                .Select(p => new
                {
                    nome = p.Pessoa.Nome,
                    ramo = p.Pessoa.Ramo,
                    dataPagamento = p.DataPagamento.ToString("dd/MM/yyyy HH:mm"),
                    status = "Pago",
                    mes = p.Mes // ← Adicionado aqui
                })
                .ToList();

            return Ok(historico);
        }
    }
}
