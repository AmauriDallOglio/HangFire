using Hangfire;
using HangFire.Api.Dominio;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using MediatR;

namespace HangFire.Api.Aplicacao.UsuarioCommand
{
    public class InserirUsuarioCommandHandler : IRequestHandler<InserirUsuarioCommandRequest, InserirUsuarioCommandResponse>
    {
        private readonly IUsuarioRepositorio _iUsuarioRepositorio;
        private readonly IMediator _iMediator;
        public InserirUsuarioCommandHandler(IUsuarioRepositorio iUsuarioRepositorio, IMediator mediator)
        {
            _iUsuarioRepositorio = iUsuarioRepositorio;
            _iMediator = mediator;
        }

 

        public async Task<InserirUsuarioCommandResponse> Handle(InserirUsuarioCommandRequest request, CancellationToken cancellationToken)
        {
            Usuario usuario = new Usuario() { Codigo = request.Codigo, Email = request.Email, Nome = request.Nome};
 
            BackgroundJob.Schedule(() => _iUsuarioRepositorio.InserirAsync(usuario), TimeSpan.FromMinutes(1));

            Mensagem mensagem = new Mensagem();
            mensagem.Descricao = "Gravado com sucesso";
            var aaa = _iMediator.Send(mensagem);

            InserirUsuarioCommandResponse response = new InserirUsuarioCommandResponse();

            return response;
        }
    }
}

