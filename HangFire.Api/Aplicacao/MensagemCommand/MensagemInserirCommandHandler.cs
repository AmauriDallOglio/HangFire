using Hangfire;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using MediatR;

namespace HangFire.Api.Aplicacao.MensagemCommand
{

    public class MensagemInserirCommandHandler : IRequestHandler<MensagemInserirCommandRequest, MensagemInserirCommandResponse>
    {
        private readonly IMensagemRepositorio _iMensagemRepositorio;
        private readonly IMediator _iMediator;
        public MensagemInserirCommandHandler(IMensagemRepositorio iMensagemRepositorio, IMediator mediator)
        {
            _iMensagemRepositorio = iMensagemRepositorio;
            _iMediator = mediator;
        }

        public async Task<MensagemInserirCommandResponse> Handle(MensagemInserirCommandRequest request, CancellationToken cancellationToken)
        {
            Mensagem mensagem = new Mensagem() { Descricao = request.Descricao};
            mensagem.Validar();

            string codigo = BackgroundJob.Schedule(() => _iMensagemRepositorio.InserirAsync(mensagem), TimeSpan.FromMinutes(1));

            return new MensagemInserirCommandResponse() { Mensagem = $"Adicionado na fila id: {codigo}" };
        }
    }
}