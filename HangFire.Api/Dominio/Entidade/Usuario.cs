using HangFire.Api.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HangFire.Api.Dominio.Entidade
{
    [Table("Usuario")]
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O código é obrigatório.")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail é inválido.")]
        public string Email { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public Usuario Incluir()
        {
            Id = 1;
            Nome = "Nome";
            Codigo = "Codigo";
            Email = "Email";

            return this;
        }

        public void Validar()
        {
            string resultado = new Validador().Validar(this);
            if (!string.IsNullOrEmpty(resultado))
            {
                throw new InvalidOperationException(resultado);
            }
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
