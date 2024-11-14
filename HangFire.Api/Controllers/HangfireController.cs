using HangFire.Api.Aplicacao.HangfireCommand;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Infra.Contexto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HangFire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {
        private readonly CommandContext _commandContext;
        private readonly IMediator _iMediator;
        public HangfireController(CommandContext commandContext, IMediator iMediator )
        {
            _commandContext = commandContext;
            _iMediator = iMediator;

        }


        [HttpPost("HangfireEscluirSucceeded"), ActionName("HangfireEscluirSucceeded")]
        public async Task<IActionResult> HangfireEscluirSucceeded([FromBody] HangfireExcluirSucceededCommandRequest request)
        {
            var response = await _iMediator.Send(request);
            return Ok(response);

        }



        [HttpGet("ObterJobSucessoDapper")]
        public async Task<IActionResult> ObterJobSucessoDapper()
        {
            try
            {
                var sql = @"
                    SELECT 
                        DATEADD(HOUR, -3, CreatedAt) AS CreatedAt_Brasilia,
                        CreatedAt,
                        Id,
                        StateName
                    FROM 
                        Hangfire.Job
                    WHERE 
                        StateName = 'Succeeded' 
                        AND CreatedAt < GETDATE();";

                var jobs = await _commandContext.Set<HangfireJob>()
                               .FromSqlRaw(sql)
                               .AsNoTracking()
                               .ToListAsync();

                return Ok(jobs);
            }
            catch (Exception ex)
            {
                MensagemInserirCommandRequest mensagem = new MensagemInserirCommandRequest();
                var texto = $"{DateTime.Now} - Erro ao obter jobs com sucesso: {ex.Message}";
                mensagem.Descricao = texto;
                var bbb = _iMediator.Send(mensagem);
                return StatusCode(500, texto);
            }
        }
    }
}
