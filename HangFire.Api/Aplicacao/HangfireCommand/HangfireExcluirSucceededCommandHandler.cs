using Hangfire;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Dominio.Interface;
using MediatR;

namespace HangFire.Api.Aplicacao.HangfireCommand
{
    public class HangfireExcluirSucceededCommandHandler : IRequestHandler<HangfireExcluirSucceededCommandRequest, HangfireExcluirSucceededCommandResponse>
    {

        private readonly IHangFireRepositorio _iHangFireRepositorio;
        private readonly IMediator _iMediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public HangfireExcluirSucceededCommandHandler(IHangFireRepositorio iHangFireRepositorio, IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _iHangFireRepositorio = iHangFireRepositorio;
            _iMediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<HangfireExcluirSucceededCommandResponse> Handle(HangfireExcluirSucceededCommandRequest request, CancellationToken cancellationToken)
        {

            string codigoJob = BackgroundJob.Schedule(() => _iHangFireRepositorio.ExcluirRegistrosSucceeded(), TimeSpan.FromMinutes(1));

            string mensagemResultado =  $"HangfireExcluirSucceededCommandHandler - Criado Job: {codigoJob} ";
            MensagemInserirCommandRequest mensagemInserirCommandRequest = new MensagemInserirCommandRequest() { Descricao = mensagemResultado };
            MensagemInserirCommandResponse retornoMensagem = _iMediator.Send(mensagemInserirCommandRequest).Result;
            mensagemResultado += $" / Mensagem job: {retornoMensagem.Mensagem}";


            HangfireExcluirSucceededCommandResponse response = new HangfireExcluirSucceededCommandResponse() { Mensagem = mensagemResultado };
            return response;
        }
    }
}
