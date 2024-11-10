﻿using Hangfire;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepositorio _usuarioRepository;
    public UsuarioController(IUsuarioRepositorio usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;

    }

 
    [HttpGet("Conexao")]
    public IActionResult Conexao()
    {
        // Retorna uma resposta 200 OK com o conteúdo de texto "Ok"
        return Ok("Ok");
    }



    [HttpPost("AgendarInsercao")]
    public IActionResult AgendarInsercao([FromBody] Usuario usuario)
    {
        // Agenda a inserção do usuário para daqui a 1 minuto
        BackgroundJob.Schedule(() => _usuarioRepository.Inserir(usuario.Codigo, usuario.Nome, usuario.Email), TimeSpan.FromMinutes(1));
        return Ok("Usuário agendado para inserção daqui a 1 minuto!");
    }


    [HttpPost("EnfileirarInsercao")]
    public IActionResult EnfileirarInsercao([FromBody] Usuario usuario)
    {
        // Enfileira a inserção do usuário para execução imediata
        BackgroundJob.Enqueue(() => _usuarioRepository.Inserir(usuario.Codigo, usuario.Nome, usuario.Email));
        return Ok("Usuário enfileirado para inserção imediata!");
    }


    //https://localhost:7250/hangfire/jobs/scheduled
    [HttpPost("AgendarInsercaoSemDados")]
    public IActionResult AgendarInsercaoSemDados()
    {
        string codigo = $"{DateTime.Now}";
        var nome = $"Usuario_{Guid.NewGuid().ToString().Substring(0, 8)}";
        var email = $"{nome.ToLower()}@example.com";

        BackgroundJob.Schedule(() => _usuarioRepository.Inserir(codigo, nome, email), TimeSpan.FromMinutes(1));

        return Ok($"Usuário '{nome}' agendado para inserção daqui a 1 minuto com o email '{email}'!");

    }



}
