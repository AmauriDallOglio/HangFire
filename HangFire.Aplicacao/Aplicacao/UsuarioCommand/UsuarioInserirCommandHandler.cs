using Hangfire;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Util;
using MediatR;

namespace HangFire.Api.Aplicacao.UsuarioCommand
{
    public class UsuarioInserirCommandHandler : IRequestHandler<UsuarioInserirCommandRequest, ResultadoOperacao<UsuarioInserirCommandResponse>>
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

        public async Task<ResultadoOperacao<UsuarioInserirCommandResponse>> Handle(UsuarioInserirCommandRequest request, CancellationToken cancellationToken)
        {
            Usuario usuario = new Usuario().InserirDados(request.Nome, request.Codigo, request.Email);
            usuario.Validar();

            int gravado = await _iUsuarioRepositorio.InserirAsync(usuario);

            MensagemInserirCommandRequest mensagem = new MensagemInserirCommandRequest()
            {
                Descricao = $"UsuarioInserirCommandHandler - Usuário '{usuario.Email}' inserido. Registros gravados: {gravado}"
            };
            string codigoJobFilho = _backgroundJobClient.Enqueue(() => _hangFireRegistrarMensagemComMediator.RegistrarMensagemAsync(mensagem));


            string mensagemResultado = mensagem.Descricao + $" / MensagemInserirCommandRequest: Criado job de mensagem: {codigoJobFilho}";
            HelperConsoleColor.Info(mensagemResultado);

            var response = new UsuarioInserirCommandResponse { Mensagem = mensagemResultado };
            return await ResultadoOperacao<UsuarioInserirCommandResponse>.RetornaSuccessoAsync(response, response.Mensagem);
        }
    }
}

