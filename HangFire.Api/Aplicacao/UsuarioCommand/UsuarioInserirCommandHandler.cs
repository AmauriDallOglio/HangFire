using Hangfire;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using MediatR;

namespace HangFire.Api.Aplicacao.UsuarioCommand
{
    public class UsuarioInserirCommandHandler : IRequestHandler<UsuarioInserirCommandRequest, UsuarioInserirCommandResponse>
    {
        private readonly IUsuarioRepositorio _iUsuarioRepositorio;
        private readonly IMediator _iMediator;
        public UsuarioInserirCommandHandler(IUsuarioRepositorio iUsuarioRepositorio, IMediator mediator)
        {
            _iUsuarioRepositorio = iUsuarioRepositorio;
            _iMediator = mediator;
        }

        public async Task<UsuarioInserirCommandResponse> Handle(UsuarioInserirCommandRequest request, CancellationToken cancellationToken)
        {
            Usuario usuario = new Usuario() { Codigo = request.Codigo, Email = request.Email, Nome = request.Nome};
            usuario.Validar();

            string codigoJob = BackgroundJob.Schedule(() => _iUsuarioRepositorio.InserirAsync(usuario), TimeSpan.FromMinutes(1));

            MensagemInserirCommandRequest mensagem = new MensagemInserirCommandRequest();
            mensagem.Descricao = $"{DateTime.Now} - UsuarioInserirCommandHandler - Criado Job: {codigoJob} ";
            var retornoMensagem = _iMediator.Send(mensagem).Result;

            string mensagemResultado = $"UsuarioInserirCommandResponse: Criado job {codigoJob} / MensagemInserirCommandRequest: Criado job: {retornoMensagem}";
            UsuarioInserirCommandResponse response = new UsuarioInserirCommandResponse() { Mensagem = mensagemResultado };

            return response;
        }
    }
}

