using Hangfire;
using HangFire.Api.Aplicacao.HangfireCommand;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Infra.Contexto;
using MediatR;

namespace HangFire.Api.Servico
{
    public class HangFireServico
    {
        private readonly CommandContext _commandContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly HttpClient _httpClient;
        private readonly IMediator _mediator;

        public HangFireServico(CommandContext context, IServiceScopeFactory serviceScopeFactory, HttpClient httpClient, IMediator imediator)
        {
            _commandContext = context;
            _serviceScopeFactory = serviceScopeFactory;
            _httpClient = httpClient;
            _mediator = imediator;
        }


        public void ProgramaEstartado()
        {
            string erro = $"{DateTime.Now} Inicialização do sistema!";
            MensagemInserirCommandRequest mensagem = new MensagemInserirCommandRequest() { Descricao = erro };
            _mediator.Send(mensagem);
        }




        public void DeletarRegistrosAntigos()
        {
            //var dataLimite = DateTime.Now.AddMinutes(-1);
            //var registrosParaDeletar = _commandContext.Usuario.Where(r => r.DataCadastro <= dataLimite);
            //_commandContext.Usuario.RemoveRange(registrosParaDeletar);
            //_commandContext.SaveChanges();
        }

        public void LimparJobsSucceededAntigos()
        {
            HangfireExcluirSucceededCommandRequest mensagem = new HangfireExcluirSucceededCommandRequest() {  };
            _mediator.Send(mensagem);

            //try
            //{
            //    using var scope = _serviceScopeFactory.CreateScope();
            //    var dbContext = scope.ServiceProvider.GetRequiredService<CommandContext>();

            //    dbContext.Database.ExecuteSqlRaw(@" DELETE FROM Hangfire.Job
            //                                        WHERE StateName = 'Succeeded' AND CreatedAt < CAST(GETDATE() AS DATE); ");
            //}
            //catch (Exception ex)
            //{
            //    string erro = $"{DateTime.Now} [LimparJobsSucceededAntigos] Erro: {ex.Message.ToString()} .";
            //    MensagemInserirCommandRequest mensagem = new MensagemInserirCommandRequest() { Descricao = erro };
            //    _mediator.Send(mensagem);
            //}
        }
    }
}
