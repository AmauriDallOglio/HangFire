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

 
    [HttpGet("Conexao")]
    public IActionResult Conexao()
    {
        return Ok("Ok");
    }


    [HttpPost("Inserir"), ActionName("Inserir")]
    public async Task<IActionResult> Inserir([FromBody] UsuarioInserirCommandRequest request)
    {
        var resultado = await _mediator.Send(request);
        return Ok(resultado);

    }



    //[HttpPost("AgendarInsercao")]
    //public IActionResult AgendarInsercao([FromBody] Usuario usuario)
    //{
    //    BackgroundJob.Schedule(() => _usuarioRepository.InserirAsync(usuario), TimeSpan.FromMinutes(1));
    //    return Ok("Usuário agendado para inserção daqui a 1 minuto!");
    //}


    //[HttpPost("EnfileirarInsercao")]
    //public IActionResult EnfileirarInsercao([FromBody] Usuario usuario)
    //{
    //    BackgroundJob.Enqueue(() => _usuarioRepository.InserirAsync(usuario));
    //    return Ok("Usuário enfileirado para inserção imediata!");
    //}

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
