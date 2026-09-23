using Hangfire;
using HangFire.Api.Aplicacao.UsuarioCommand;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepositorio _usuarioRepository;
    private readonly IMediator _mediator;
    public UsuarioController(IUsuarioRepositorio usuarioRepository, IMediator mediator)
    {
        _usuarioRepository = usuarioRepository;
        _mediator = mediator;
    }


    [HttpPost("Inserir"), ActionName("Inserir")]
    public async Task<IActionResult> Inserir([FromBody] UsuarioInserirCommandRequest request)
    {
        var resultado = await _mediator.Send(request);
        return Ok(resultado);

    }

    [HttpPost("ValidaCadastraUsuario"), ActionName("ValidaCadastraUsuario")]
    public async Task<IActionResult> ValidaCadastraUsuario([FromBody] ValidaCadastraUsuarioRequest request)
    {
        var resultado = await _mediator.Send(request);
        return Ok(resultado);
    }


    [HttpPost("Alterar"), ActionName("Alterar")]
    public async Task<IActionResult> Alterar([FromBody] UsuarioAlterarCommandRequest request)
    {
        var resultado = await _mediator.Send(request);
        return Ok(resultado);

    }



    //https://localhost:7250/hangfire/jobs/scheduled
    [HttpPost("AgendarInsercaoSemDados")]
    public IActionResult AgendarInsercaoSemDados()
    {
        Usuario usuario = new Usuario().IncluirAutomaticamente();
        UsuarioInserirCommandRequest usuarioInserirCommandRequest = new UsuarioInserirCommandRequest().ConverterDto(usuario);
        var resultado = _mediator.Send(usuarioInserirCommandRequest);
        return Ok($"Usuário '{usuarioInserirCommandRequest.Nome}' agendado para inserção daqui a 1 minuto com o email '{usuarioInserirCommandRequest.Email}'!");

    }


    //https://localhost:7250/hangfire/jobs/scheduled
    [HttpPost("AgendarInsercaoSemDadosDapper")]
    public IActionResult AgendarInsercaoSemDadosDapper()
    {
        Usuario usuario = new Usuario().IncluirAutomaticamente();
        BackgroundJob.Schedule(() => _usuarioRepository.InserirDapperAsync(usuario.Codigo, usuario.Nome, usuario.Nome), TimeSpan.FromMinutes(1));
        return Ok($"Usuário '{usuario.Nome}' agendado para inserção daqui a 1 minuto com o email '{usuario.Email}'!");
    }
}
