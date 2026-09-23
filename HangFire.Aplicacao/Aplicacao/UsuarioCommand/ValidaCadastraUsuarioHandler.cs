using Hangfire;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Util;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HangFire.Api.Aplicacao.UsuarioCommand
{
    public class ValidaCadastraUsuarioHandler : IRequestHandler<ValidaCadastraUsuarioRequest, ResultadoOperacao<ValidaCadastraUsuarioResponse>>
    {
        private readonly IUsuarioRepositorio _iUsuarioRepositorio;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly HangFireCadastrarUsuarioComMediator _hangFireCadastrarUsuarioComMediator;

        public ValidaCadastraUsuarioHandler(
            IUsuarioRepositorio iUsuarioRepositorio,
            IBackgroundJobClient backgroundJobClient,
            HangFireCadastrarUsuarioComMediator hangFireCadastrarUsuarioComMediator)
        {
            _iUsuarioRepositorio = iUsuarioRepositorio;
            _backgroundJobClient = backgroundJobClient;
            _hangFireCadastrarUsuarioComMediator = hangFireCadastrarUsuarioComMediator;
        }

        public async Task<ResultadoOperacao<ValidaCadastraUsuarioResponse>> Handle(ValidaCadastraUsuarioRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || !new EmailAddressAttribute().IsValid(request.Email))
            {
                return await ResultadoOperacao<ValidaCadastraUsuarioResponse>.RetornaFalhaAsync(
                    $"O e-mail '{request.Email}' é inválido.",
                    CodigoRetornoOperacao.DadosInvalidos);
            }

            var usuario = await _iUsuarioRepositorio.ObterPorEmailAsync(request.Email);

            if (usuario != null)
            {
                return await ResultadoOperacao<ValidaCadastraUsuarioResponse>.RetornaFalhaAsync(
                    $"Usuário com o email '{request.Email}' já cadastrado.",
                    CodigoRetornoOperacao.ConflitoRegraNegocio);
            }

            UsuarioInserirCommandRequest usuarioInserirCommandRequest = new UsuarioInserirCommandRequest()
            {
                Nome = request.Nome,
                Codigo = request.Codigo,
                Email = request.Email
            };

            string codigoJob = _backgroundJobClient.Schedule(
                () => _hangFireCadastrarUsuarioComMediator.CadastrarUsuarioAsync(usuarioInserirCommandRequest),
                new DateTimeOffset(DateTime.Now.AddMinutes(1)));

            var response = new ValidaCadastraUsuarioResponse
            {
                Mensagem = $"Usuário com o email '{request.Email}' não encontrado. UsuarioInserirCommandRequest agendado para daqui a 1 minuto no job: {codigoJob}"
            };

            return await ResultadoOperacao<ValidaCadastraUsuarioResponse>.RetornaSuccessoAsync(response, response.Mensagem);
        }
    }
}
