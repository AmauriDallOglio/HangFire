using Hangfire;
using HangFire.Api.Dominio;
using HangFire.Api.Dominio.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MensagemController : ControllerBase
    {
        private readonly IMensagemRepositorio _mensagemRepositorio;
        public MensagemController(IMensagemRepositorio mensagemRepositorio)
        {
            _mensagemRepositorio = mensagemRepositorio;

        }

        [HttpGet("Conexao")]
        public IActionResult Conexao()
        {
 
            return Ok("Ok");
        }

        [HttpPost("InserirMensagem")]
        public IActionResult InserirMensagem([FromBody] Mensagem mensagem)
        {
 
            BackgroundJob.Schedule(() => _mensagemRepositorio.Inserir(mensagem.Descricao), TimeSpan.FromMinutes(1));
            return Ok("Mensagem agendada para inserção daqui a 1 minuto!");
        }
    }
}
