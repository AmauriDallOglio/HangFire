using MediatR;

namespace HangFire.Api.Aplicacao.UsuarioCommand
{
    public class InserirUsuarioCommandRequest : IRequest<InserirUsuarioCommandResponse>
    {
    
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    
}
