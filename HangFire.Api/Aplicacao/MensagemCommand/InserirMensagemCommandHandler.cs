using HangFire.Api.Aplicacao.UsuarioCommand;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Dominio;
using Hangfire;
using MediatR;

namespace HangFire.Api.Aplicacao.MensagemCommand
{
 
    public class InserirMensagemCommandHandler : IRequestHandler<InserirMensagemCommandRequest, InserirMensagemCommandResponse>
    {
        private readonly IUsuarioRepositorio _iUsuarioRepositorio;
        private readonly IMediator _iMediator;
        public InserirMensagemCommandHandler(IUsuarioRepositorio iUsuarioRepositorio, IMediator mediator)
        {
            _iUsuarioRepositorio = iUsuarioRepositorio;
            _iMediator = mediator;
        }



        public async Task<InserirMensagemCommandResponse> Handle(InserirMensagemCommandRequest request, CancellationToken cancellationToken)
        {
             

            BackgroundJob.Schedule(() => _iUsuarioRepositorio.InserirAsync(usuario), TimeSpan.FromMinutes(1));

            Mensagem mensagem = new Mensagem();
            mensagem.Descricao = "Gravado com sucesso";
            var aaa = _iMediator.Send(mensagem);

            InserirMensagemCommandResponse response = new InserirMensagemCommandResponse();

            return response;
        }
    }
}