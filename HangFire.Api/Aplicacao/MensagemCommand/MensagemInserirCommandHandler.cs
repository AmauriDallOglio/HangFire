using Hangfire;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Middleware;
using HangFire.Api.Util;
using MediatR;

namespace HangFire.Api.Aplicacao.MensagemCommand
{
    public class MensagemInserirCommandHandler : IRequestHandler<MensagemInserirCommandRequest, MensagemInserirCommandResponse>
    {
        private readonly IMensagemRepositorio _iMensagemRepositorio;
        private readonly IMediator _iMediator;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public MensagemInserirCommandHandler(IMensagemRepositorio iMensagemRepositorio, IMediator mediator, IBackgroundJobClient backgroundJobClient)
        {
            _iMensagemRepositorio = iMensagemRepositorio;
            _iMediator = mediator;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<MensagemInserirCommandResponse> Handle(MensagemInserirCommandRequest request, CancellationToken cancellationToken)
        {
            Mensagem mensagem = new Mensagem().IncluirDados(request.Descricao);
            mensagem.Validar();

            mensagem.AlterarDescricao($"{DateTime.Now} - MensagemInserirCommandHandler - {request.Descricao}");
 
            string codigoJob = _backgroundJobClient.Schedule(() => _iMensagemRepositorio.InserirAsync(mensagem), HangFireJobs.Segundos15());

            HelperConsoleColor.Info($"MensagemInserirCommandHandler - Adicionado na fila id: {codigoJob}");

            return new MensagemInserirCommandResponse() { Mensagem = $"MensagemInserirCommandHandler - Adicionado na fila id: {codigoJob}" };
        }
    }
}