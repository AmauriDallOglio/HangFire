using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Util;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LaboratorioController : ControllerBase
    {
        private readonly IMensagemRepositorio _mensagemRepositorio;
        private readonly IMediator _iMediator;
        public LaboratorioController(IMensagemRepositorio mensagemRepositorio, IMediator iMediator)
        {
            _mensagemRepositorio = mensagemRepositorio;
            _iMediator = iMediator;
        }

        [HttpGet("Conexao")]
        public IActionResult Conexao()
        {
            return Ok("Ok");
        }


        [HttpGet("HorarioJob")]
        public IActionResult HorarioJob(CancellationToken cancellationToken)
        {
            // Defina a cultura e fuso horário
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"); // Exemplo para São Paulo
            DateTime horarioLocal = TimeZoneInfo.ConvertTime(DateTime.Now, timeZone);

            Console.WriteLine($"O job foi processado às {horarioLocal}");

            MensagemInserirCommandRequest mensagem = new MensagemInserirCommandRequest() { Descricao = $"{DateTime.Now} - O job foi processado às {horarioLocal}" };
 
            var resultado = _iMediator.Send(mensagem, cancellationToken);
            return Ok("Mensagem agendada para inserção daqui a 1 minuto! ");
        }

        [HttpGet("GerarErroArgumentException")]
        public IActionResult GerarErroArgumentException()
        {
            throw new ArgumentException("ArgumentException: Lançada quando um argumento inválido é passado para um método.: A descrição não pode ser nula ou vazia.");
        }

        [HttpGet("GerarErroApplicationException")]
        public IActionResult GerarErroApplicationException()
        {
            throw new ApplicationException("ApplicationException: Projetada para representar exceções específicas de aplicativos.: Erro ao processar a solicitação do aplicativo.");
        }

        [HttpGet("GerarErroKeyNotFoundException")]
        public IActionResult GerarErroKeyNotFoundException()
        {
            throw new KeyNotFoundException("KeyNotFoundException: Lançada ao tentar acessar uma chave que não existe em um dicionário.: O recurso solicitado não foi encontrado.");
        }

        [HttpGet("GerarErroInvalidOperationException")]
        public IActionResult GerarErroInvalidOperationException()
        {
            throw new InvalidOperationException("InvalidOperationException: Lançada quando o estado do objeto não suporta a operação solicitada.: A operação solicitada é inválida no estado atual.");
        }

        [HttpGet("GerarErroUnauthorizedAccessException")]
        public IActionResult GerarErroUnauthorizedAccessException()
        {
            throw new UnauthorizedAccessException("UnauthorizedAccessException: Lançada quando o acesso a um recurso não é permitido.: O acesso ao recurso foi negado.");
        }

        [HttpGet("GerarErroFormatException")]
        public IActionResult GerarErroFormatException()
        {
            throw new FormatException("FormatException: Lançada quando o formato de um valor é inválido.: O formato dos dados fornecidos é inválido.");
        }

        [HttpGet("GerarErroNullReferenceException")]
        public IActionResult GerarErroNullReferenceException()
        {
            throw new NullReferenceException("NullReferenceException: Lançada ao tentar acessar membros de um objeto nulo.: Tentativa de acessar um objeto nulo.");
        }

        [HttpGet("GerarErroDivideByZeroException")]
        public IActionResult GerarErroDivideByZeroException()
        {
            throw new DivideByZeroException("DivideByZeroException: Lançada ao tentar dividir um número por zero.: Tentativa de divisão por zero.");
        }

        [HttpGet("GerarErroCustomException")]
        public IActionResult GerarErroCustomException()
        {
            throw new MinhaExcecaoPersonalizada("Erro personalizado ocorrido.");
        }

 

    }
}
