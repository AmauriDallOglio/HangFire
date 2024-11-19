using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HangFire.Api.Util;

namespace HangFire.Api.Dominio.Entidade
{
    [Table("MensagemErro")]
    public class MensagemErro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "A data de cadastro é obrigatória.")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A chamada é obrigatória.")]
        [StringLength(200, ErrorMessage = "A chamada deve ter no máximo 200 caracteres.")]
        public string Chamada { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
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
