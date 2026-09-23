using Hangfire;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Util;
using MediatR;

namespace HangFire.Api.Aplicacao.UsuarioCommand
{
    public class UsuarioAlterarCommandHandler : IRequestHandler<UsuarioAlterarCommandRequest, ResultadoOperacao<UsuarioAlterarCommandResponse>>
    {
        private readonly IUsuarioRepositorio _iUsuarioRepositorio;
        private readonly IMediator _iMediator;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly HangFireRegistrarMensagemComMediator _hangFireRegistrarMensagemComMediator;
        public UsuarioAlterarCommandHandler(IUsuarioRepositorio iUsuarioRepositorio, IMediator mediator, IBackgroundJobClient backgroundJobClient, HangFireRegistrarMensagemComMediator hangFireRegistrarMensagemComMediator)
        {
            _iUsuarioRepositorio = iUsuarioRepositorio;
            _iMediator = mediator;
            _backgroundJobClient = backgroundJobClient;
            _hangFireRegistrarMensagemComMediator = hangFireRegistrarMensagemComMediator;
        }

        public async Task<ResultadoOperacao<UsuarioAlterarCommandResponse>> Handle(UsuarioAlterarCommandRequest request, CancellationToken cancellationToken)
        {
            Usuario usuario = _iUsuarioRepositorio.ObterPorIdAsync(request.Id).Result;
            usuario.AlterarDados(request.Nome, request.Codigo, request.Email);

            string codigoJobPai = _backgroundJobClient.Schedule(() => _iUsuarioRepositorio.AlterarAsync(usuario), new DateTimeOffset(DateTime.Now.AddSeconds(30)));
 

            var retorno = new UsuarioAlterarCommandResponse { Mensagem = $"UsuarioInserirCommandHandler - Criado Job: {codigoJobPai} " };


            return await ResultadoOperacao<UsuarioAlterarCommandResponse>.RetornaSuccessoAsync(retorno, retorno.Mensagem);
        }
    }
}
