using Hangfire;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Util;
using MediatR;

namespace HangFire.Api.Aplicacao.MensagemErroCommand
{
    public class MensagemErroInserirCommandHandler : IRequestHandler<MensagemErroInserirCommandRequest, ResultadoOperacao<MensagemErroInserirCommandResponse>>
    {
        private readonly IMensagemErroRepositorio _iMensagemErroRepositorio;
        private readonly IMediator _iMediator;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public MensagemErroInserirCommandHandler(IMensagemErroRepositorio iMensagemErroRepositorio, IMediator mediator, IBackgroundJobClient backgroundJobClient)
        {
            _iMensagemErroRepositorio = iMensagemErroRepositorio;
            _iMediator = mediator;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<ResultadoOperacao<MensagemErroInserirCommandResponse>> Handle(MensagemErroInserirCommandRequest request, CancellationToken cancellationToken)
        {
            MensagemErro mensagemErro = new MensagemErro().IncluirDados(request.Descricao, request.Chamada);
            mensagemErro.Validar();

            mensagemErro.AlterarDescricao($"{DateTime.Now} - MensagemErroInserirCommandHandler - {request.Descricao}");

  
            string codigoJob = _backgroundJobClient.Schedule(() => _iMensagemErroRepositorio.InserirAsync(mensagemErro), new DateTimeOffset(DateTime.Now.AddSeconds(15)));
            HelperConsoleColor.Info($"Adicionado na fila id: {codigoJob}");
            var response = new MensagemErroInserirCommandResponse() { Mensagem = $"Adicionado na fila id: {codigoJob}" };
            return await ResultadoOperacao<MensagemErroInserirCommandResponse>.RetornaSuccessoAsync(response, response.Mensagem);
        }
    }
}
