using HangFire.Api.Aplicacao.HangfireCommand;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Aplicacao.MensagemErroCommand;
using HangFire.Api.Infra.Contexto;
using MediatR;

namespace HangFire.Api.Servico
{
    public class HangFireServico
    {
        private readonly CommandContext _commandContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly HttpClient _httpClient;
        private readonly IMediator _iMediator;

        public HangFireServico(CommandContext context, IServiceScopeFactory serviceScopeFactory, HttpClient httpClient, IMediator imediator)
        {
            _commandContext = context;
            _serviceScopeFactory = serviceScopeFactory;
            _httpClient = httpClient;
            _iMediator = imediator;
        }

        public void InicializacaoDoSistema()
        {
            MensagemInserirCommandRequest request = new MensagemInserirCommandRequest() { Descricao = "[InicializacaoDoSistema] Inicialização do sistema!" };
            MensagemInserirCommandResponse response = _iMediator.Send(request, new CancellationToken()).Result;
        }

        public void LimparJobsSucceededAntigos()
        {
 
            HangfireExcluirSucceededCommandRequest request = new HangfireExcluirSucceededCommandRequest() { };
            Task<HangfireExcluirSucceededCommandResponse> response = _iMediator.Send(request, new CancellationToken());
            if (response.IsFaulted || response.IsCanceled)
            {
                string mensagemErro = response.Exception?.InnerException?.Message
                                        ?? response.Exception?.InnerException?.InnerException?.Message
                                        ?? "Erro não localizado!";
 
                string descricao = $"[LimparJobsSucceededAntigos/HangfireExcluirSucceededCommandHandler] Erro: {mensagemErro}.";
                Console.WriteLine(descricao);

                MensagemErroInserirCommandRequest requestErro = new MensagemErroInserirCommandRequest() { Descricao = descricao, Chamada = "HangFireServico" };
                MensagemErroInserirCommandResponse responseErro = _iMediator.Send(requestErro, new CancellationToken()).Result;

            }
 
        }





        //public void LimparJobsSucceededAntigos2()
        //{
        //    try
        //    {
        //        using var scope = _serviceScopeFactory.CreateScope();
        //        var dbContext = scope.ServiceProvider.GetRequiredService<CommandContext>();

        //        dbContext.Database.ExecuteSqlRaw(@" DELETE FROM Hangfire.Job
        //                                            WHERE StateName = 'Succeeded' AND CreatedAt < CAST(GETDATE() AS DATE); ");
        //    }
        //    catch (Exception ex)
        //    {
        //        string erro = $"{DateTime.Now} [LimparJobsSucceededAntigos] Erro: {ex.Message.ToString()} .";
        //        MensagemInserirCommandRequest mensagem = new MensagemInserirCommandRequest() { Descricao = erro };
        //        _mediator.Send(mensagem);
        //    }
        //}



        //public void DeletarRegistrosAntigos()
        //{
        //    var dataLimite = DateTime.Now.AddMinutes(-1);
        //    var registrosParaDeletar = _commandContext.Usuario.Where(r => r.DataCadastro <= dataLimite);
        //    _commandContext.Usuario.RemoveRange(registrosParaDeletar);
        //    _commandContext.SaveChanges();
        //}

    }
}
