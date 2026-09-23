using HangFire.Api.Aplicacao.UsuarioCommand;
using MediatR;

namespace HangFire.Api.Util
{
    public class HangFireCadastrarUsuarioComMediator
    {
        private readonly IMediator _mediator;

        public HangFireCadastrarUsuarioComMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CadastrarUsuarioAsync(UsuarioInserirCommandRequest usuario)
        {
            await _mediator.Send(usuario);
        }
    }
}
