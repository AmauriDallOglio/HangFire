using Hangfire;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Middleware;
using HangFire.Api.Util;
using MediatR;

namespace HangFire.Api.Aplicacao.HangfireCommand
{
    public class HangfireExcluirSucceededCommandHandler : IRequestHandler<HangfireExcluirSucceededCommandRequest, HangfireExcluirSucceededCommandResponse>
    {

        private readonly IHangFireRepositorio _iHangFireRepositorio;
        private readonly IMediator _iMediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HangfireExcluirSucceededCommandHandler(IHangFireRepositorio iHangFireRepositorio, IMediator mediator, IServiceScopeFactory serviceScopeFactory, IBackgroundJobClient backgroundJobClient)
        {
            _iHangFireRepositorio = iHangFireRepositorio;
            _iMediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<HangfireExcluirSucceededCommandResponse> Handle(HangfireExcluirSucceededCommandRequest request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateTimeOffset = HangFireJobs.Segundos15();
            string codigoJob = BackgroundJob.Schedule(() => _iHangFireRepositorio.ExcluirRegistrosSucceeded(), dateTimeOffset);

            string mensagemResultado =  $"HangfireExcluirSucceededCommandHandler - Criado Job: {codigoJob} ";
            MensagemInserirCommandRequest mensagemInserirCommandRequest = new MensagemInserirCommandRequest() { Descricao = mensagemResultado };
            MensagemInserirCommandResponse retornoMensagem = _iMediator.Send(mensagemInserirCommandRequest).Result;
            mensagemResultado += $" / Mensagem job: {retornoMensagem.Mensagem}";


            HangfireExcluirSucceededCommandResponse response = new HangfireExcluirSucceededCommandResponse() { Mensagem = mensagemResultado };
            HelperConsoleColor.Info(mensagemResultado);
            return response;
        }
    }
}
