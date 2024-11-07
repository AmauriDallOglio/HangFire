using Hangfire;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepositorio _userRepository;

    public UsuariosController(IUsuarioRepositorio userRepository)
    {
        _userRepository = userRepository;
    }

 
    [HttpPost("agendar")]
    public IActionResult AgendarInsercao([FromBody] Usuario user)
    {
        // Agenda a inserção do usuário para daqui a 1 minuto
        BackgroundJob.Schedule(() => _userRepository.Inserir(user.Codigo, user.Nome, user.Email), TimeSpan.FromMinutes(1));
        return Ok("Usuário agendado para inserção daqui a 1 minuto!");
    }

 
    [HttpPost("enfileirar")]
    public IActionResult EnfileirarInsercao([FromBody] Usuario user)
    {
        // Enfileira a inserção do usuário para execução imediata
        BackgroundJob.Enqueue(() => _userRepository.Inserir(user.Codigo, user.Nome, user.Email));
        return Ok("Usuário enfileirado para inserção imediata!");
    }
}
