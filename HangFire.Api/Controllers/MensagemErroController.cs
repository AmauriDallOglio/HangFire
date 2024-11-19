using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Aplicacao.MensagemErroCommand;
using HangFire.Api.Dominio.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MensagemErroController : ControllerBase
    {
        private readonly IMensagemRepositorio _mensagemRepositorio;
        private readonly IMediator _iMediator;
        public MensagemErroController(IMensagemRepositorio mensagemRepositorio, IMediator iMediator)
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
        public async Task<IActionResult> Inserir([FromBody] MensagemErroInserirCommandRequest mensagem)
        {

            var resultado = await _iMediator.Send(mensagem);
            return Ok(resultado);
        }
    }
}
