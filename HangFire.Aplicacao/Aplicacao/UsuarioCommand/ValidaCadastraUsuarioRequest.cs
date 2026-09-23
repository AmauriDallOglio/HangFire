using MediatR;
using HangFire.Api.Util;

namespace HangFire.Api.Aplicacao.UsuarioCommand
{
    public class ValidaCadastraUsuarioRequest : IRequest<ResultadoOperacao<ValidaCadastraUsuarioResponse>>
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
