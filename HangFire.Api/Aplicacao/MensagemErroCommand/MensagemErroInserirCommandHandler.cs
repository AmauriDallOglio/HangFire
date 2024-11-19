using Hangfire;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using MediatR;

namespace HangFire.Api.Aplicacao.MensagemErroCommand
{
    public class MensagemErroInserirCommandHandler : IRequestHandler<MensagemErroInserirCommandRequest, MensagemErroInserirCommandResponse>
    {
        private readonly IMensagemErroRepositorio _iMensagemErroRepositorio;
        private readonly IMediator _iMediator;
        public MensagemErroInserirCommandHandler(IMensagemErroRepositorio iMensagemErroRepositorio, IMediator mediator)
        {
            _iMensagemErroRepositorio = iMensagemErroRepositorio;
            _iMediator = mediator;
        }

        public async Task<MensagemErroInserirCommandResponse> Handle(MensagemErroInserirCommandRequest request, CancellationToken cancellationToken)
        {
            MensagemErro mensagemErro = new MensagemErro() { Descricao = request.Descricao, Chamada = request.Chamada };
            mensagemErro.Validar();

            mensagemErro.Descricao = $"{DateTime.Now} - MensagemErroInserirCommandHandler - {request.Descricao}";
            string codigoJob = BackgroundJob.Schedule(() => _iMensagemErroRepositorio.InserirAsync(mensagemErro), TimeSpan.FromMinutes(1));
            return new MensagemErroInserirCommandResponse() { Mensagem = $"Adicionado na fila id: {codigoJob}" };
        }
    }
}
