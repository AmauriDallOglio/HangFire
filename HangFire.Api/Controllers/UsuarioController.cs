using Hangfire;
using HangFire.Api.Aplicacao.UsuarioCommand;
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
        // Retorna uma resposta 200 OK com o conteúdo de texto "Ok"
        return Ok("Ok");
    }


    [HttpPost("Inserir"), ActionName("Inserir")]
    public async Task<IActionResult> Inserir([FromBody] UsuarioInserirCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);

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
        string codigo = $"{DateTime.Now}";
        var nome = $"Usuario_{Guid.NewGuid().ToString().Substring(0, 8)}";
        var email = $"{nome.ToLower()}@example.com";

        UsuarioInserirCommandRequest usuario = new UsuarioInserirCommandRequest()
        {
            Codigo = codigo,
            Nome = nome,
            Email = email,
            DataCadastro = DateTime.Now,
        }; 

        var resultado = _mediator.Send(usuario);

        return Ok($"Usuário '{nome}' agendado para inserção daqui a 1 minuto com o email '{email}'!");

    }


    //https://localhost:7250/hangfire/jobs/scheduled
    [HttpPost("AgendarInsercaoSemDadosDapper")]
    public IActionResult AgendarInsercaoSemDadosDapper()
    {
        string codigo = $"{DateTime.Now}";
        var nome = $"Usuario_{Guid.NewGuid().ToString().Substring(0, 8)}";
        var email = $"{nome.ToLower()}@example.com";

        //Usuario usuario = new Usuario() { Codigo = codigo, Nome = nome, Email = email }; 

        BackgroundJob.Schedule(() => _usuarioRepository.InserirDapperAsync(codigo, nome, email), TimeSpan.FromMinutes(1));

        return Ok($"Usuário '{nome}' agendado para inserção daqui a 1 minuto com o email '{email}'!");

    }



}
