using HangFire.Api.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HangFire.Api.Dominio.Entidade
{
    [Table("Mensagem")]
    public class Mensagem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatório.")]
        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        public void Validar()
        {
            string resultado = new Validador().Validar(this);
            if (!string.IsNullOrEmpty(resultado))
            {
                throw new InvalidOperationException(resultado);
            }
        }

    }
}
