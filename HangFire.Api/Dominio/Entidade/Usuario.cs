using HangFire.Api.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HangFire.Api.Dominio.Entidade
{
    [Table("Usuario")]
    public class Usuario : AtributoBase
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; private set; } = string.Empty;

        [Required(ErrorMessage = "O código é obrigatório.")]
        public string Codigo { get; private set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail é inválido.")]
        public string Email { get; private set; } = string.Empty;


        public Usuario InserirDados(string nome, string codigo, string email)
        {
            Nome = nome;
            Codigo = codigo;
            Email = email;

            return this;
        }
 

        public Usuario IncluirAutomaticamente()
        {
            Codigo = $"{DateTime.Now}";
            Nome = $"Usuario_{Guid.NewGuid().ToString().Substring(0, 8)}";
            Email = $"{Nome.ToLower()}@example.com";
            return this;
        }
    }
}
