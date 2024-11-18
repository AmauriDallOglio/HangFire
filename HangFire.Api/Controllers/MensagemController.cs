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

 
        [HttpPost("Inserir")]
        public async Task<IActionResult> Inserir([FromBody] MensagemInserirCommandRequest mensagem)
        {

            var resultado = await _iMediator.Send(mensagem);
            return Ok(resultado);
        }

        //[HttpPost("InserirDapperMensagem")]
        //public IActionResult InserirDappperMensagem([FromBody] Mensagem mensagem)
        //{

        //    BackgroundJob.Schedule(() => _mensagemRepositorio.InserirDapperAsync(mensagem.Descricao), TimeSpan.FromMinutes(1));
        //    return Ok("Mensagem agendada para inserção daqui a 1 minuto!");
        //}
    }

}

