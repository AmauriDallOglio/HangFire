using Hangfire;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Middleware;
using HangFire.Api.Util;
using MediatR;

namespace HangFire.Api.Aplicacao.UsuarioCommand
{
    public class UsuarioInserirCommandHandler : IRequestHandler<UsuarioInserirCommandRequest, UsuarioInserirCommandResponse>
    {
        private readonly IUsuarioRepositorio _iUsuarioRepositorio;
        private readonly IMediator _iMediator;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly HangFireRegistrarMensagemComMediator _hangFireRegistrarMensagemComMediator;
        public UsuarioInserirCommandHandler(IUsuarioRepositorio iUsuarioRepositorio, IMediator mediator, IBackgroundJobClient backgroundJobClient, HangFireRegistrarMensagemComMediator hangFireRegistrarMensagemComMediator)
        {
            _iUsuarioRepositorio = iUsuarioRepositorio;
            _iMediator = mediator;
            _backgroundJobClient = backgroundJobClient;
            _hangFireRegistrarMensagemComMediator = hangFireRegistrarMensagemComMediator;

        }

        public async Task<UsuarioInserirCommandResponse> Handle(UsuarioInserirCommandRequest request, CancellationToken cancellationToken)
        {
            Usuario usuario = new Usuario().InserirDados(request.Nome, request.Codigo, request.Email);
            usuario.Validar();

            // Job pai
            string codigoJobPai = _backgroundJobClient.Schedule(() => _iUsuarioRepositorio.InserirAsync(usuario), HangFireJobs.Segundos30());

            // Mensagem a ser enviada no job filho
            MensagemInserirCommandRequest mensagem = new MensagemInserirCommandRequest()
            {
                Descricao = $"UsuarioInserirCommandHandler - Criado Job: {codigoJobPai} "
            };
            string codigoJobFilho = _backgroundJobClient.ContinueJobWith(codigoJobPai, () => _hangFireRegistrarMensagemComMediator.RegistrarMensagem(mensagem));


            string mensagemResultado = mensagem.Descricao + $" / MensagemInserirCommandRequest: Criado job filho: {codigoJobFilho}";
            HelperConsoleColor.Info(mensagemResultado);

            return new UsuarioInserirCommandResponse { Mensagem = mensagemResultado };
        }
    }
}

