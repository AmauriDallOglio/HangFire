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
            string codigoJob = string.Empty;
            try
            {
                codigoJob = BackgroundJob.Schedule(() => _iHangFireRepositorio.ExcluirRegistrosSucceeded(), TimeSpan.FromMinutes(1));
            }
            catch (Exception ex)
            {
                string erro = $"{DateTime.Now} [LimparJobsAntigos] Erro: {ex.Message}.";
                MensagemInserirCommandRequest mensagem = new MensagemInserirCommandRequest() { Descricao = erro };
                await _iMediator.Send(mensagem, cancellationToken);
            }

            return new HangfireExcluirSucceededCommandResponse()
            {
                Mensagem = $"HangfireExcluirSucceededCommandHandler: {codigoJob}"
            };
        }
    }
}
