using Hangfire;
using HangFire.Api.Aplicacao.MensagemCommand;
using MediatR;

namespace HangFire.Api.Util
{
    public class HangFireRegistrarMensagemComMediator
    {
        //Essa classe será responsável por encapsular a lógica de registro de mensagens.
        private readonly IMediator _mediator;

        public HangFireRegistrarMensagemComMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void RegistrarMensagem(MensagemInserirCommandRequest mensagem)
        {
            // Envia a mensagem via Mediator
            _mediator.Send(mensagem).Wait();
        }
    }
}
