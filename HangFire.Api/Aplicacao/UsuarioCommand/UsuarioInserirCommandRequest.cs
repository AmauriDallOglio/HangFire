using HangFire.Api.Dominio.Entidade;
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

        public UsuarioInserirCommandRequest ConverterDto(Usuario usuario)
        {
            Nome = usuario.Nome;
            Codigo = usuario.Codigo;
            Email = usuario.Email;
            DataCadastro = usuario.DataCadastro;
            return this;
        }
    }
    
}
