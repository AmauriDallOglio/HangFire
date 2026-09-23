using Hangfire;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Util;
using MediatR;

namespace HangFire.Api.Aplicacao.HangfireCommand
{
    public class HangfireExcluirSucceededCommandHandler : IRequestHandler<HangfireExcluirSucceededCommandRequest, ResultadoOperacao<HangfireExcluirSucceededCommandResponse>>
    {

        private readonly IHangFireRepositorio _iHangFireRepositorio;
        private readonly IMediator _iMediator;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HangfireExcluirSucceededCommandHandler(IHangFireRepositorio iHangFireRepositorio, IMediator mediator, IBackgroundJobClient backgroundJobClient)
        {
            _iHangFireRepositorio = iHangFireRepositorio;
            _iMediator = mediator;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<ResultadoOperacao<HangfireExcluirSucceededCommandResponse>> Handle(HangfireExcluirSucceededCommandRequest request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(DateTime.Now.AddSeconds(15));
            string codigoJob = _backgroundJobClient.Schedule(() => _iHangFireRepositorio.ExcluirRegistrosSucceeded(), dateTimeOffset);

            string mensagemResultado =  $"HangfireExcluirSucceededCommandHandler - Criado Job: {codigoJob} ";
            MensagemInserirCommandRequest mensagemInserirCommandRequest = new MensagemInserirCommandRequest() { Descricao = mensagemResultado };
            ResultadoOperacao<MensagemInserirCommandResponse> retornoMensagem = await _iMediator.Send(mensagemInserirCommandRequest, cancellationToken);
            mensagemResultado += $" / Mensagem job: {retornoMensagem.Mensagem}";


            HangfireExcluirSucceededCommandResponse response = new HangfireExcluirSucceededCommandResponse() { Mensagem = mensagemResultado };
            HelperConsoleColor.Info(mensagemResultado);
            return await ResultadoOperacao<HangfireExcluirSucceededCommandResponse>.RetornaSuccessoAsync(response, response.Mensagem);
        }
    }
}
