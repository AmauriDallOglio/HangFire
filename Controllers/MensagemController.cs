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

 
        [HttpPost("Inserir")]
        public async Task<IActionResult> Inserir([FromBody] MensagemInserirCommandRequest mensagem)
        {

            var resultado = await _iMediator.Send(mensagem);
            return Ok(resultado);
        }

 
    }

}

