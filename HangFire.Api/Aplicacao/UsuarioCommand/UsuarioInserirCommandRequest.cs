using MediatR;
using System.Text.Json.Serialization;

namespace HangFire.Api.Aplicacao.UsuarioCommand
{
    public class UsuarioInserirCommandRequest : IRequest<UsuarioInserirCommandResponse>
    {
    
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime DataCadastro { get; set; }
    }
    
}
