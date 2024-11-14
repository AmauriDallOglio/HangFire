using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Dominio.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MensagemController : ControllerBase
    {
        private readonly IMensagemRepositorio _mensagemRepositorio;
        private readonly IMediator _iMediator;
        public MensagemController(IMensagemRepositorio mensagemRepositorio, IMediator iMediator)
        {
            _mensagemRepositorio = mensagemRepositorio;
            _iMediator = iMediator;

        }

        [HttpGet("Conexao")]
        public IActionResult Conexao()
        {
 
            return Ok("Ok");
        }

        [HttpPost("InserirMensagem")]
        public IActionResult InserirMensagem([FromBody] MensagemInserirCommandRequest mensagem)
        {
            var resultado = _iMediator.Send(mensagem);
            return Ok("Mensagem agendada para inserção daqui a 1 minuto! ");
        }

        //[HttpPost("InserirDapperMensagem")]
        //public IActionResult InserirDappperMensagem([FromBody] Mensagem mensagem)
        //{

        //    BackgroundJob.Schedule(() => _mensagemRepositorio.InserirDapperAsync(mensagem.Descricao), TimeSpan.FromMinutes(1));
        //    return Ok("Mensagem agendada para inserção daqui a 1 minuto!");
        //}
    }

}

