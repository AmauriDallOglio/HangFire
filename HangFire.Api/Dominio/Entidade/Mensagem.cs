using HangFire.Api.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HangFire.Api.Dominio.Entidade
{
    [Table("Mensagem")]
    public class Mensagem : AtributoBase
    {
 

        [Required(ErrorMessage = "A descrição é obrigatório.")]
        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
        public string Descricao { get; private set; } = string.Empty;

        public Mensagem IncluirDados(string descricao)
        {
            Descricao = descricao;
            return this;
        }

        public Mensagem AlterarDescricao(string descricao)
        {
            Descricao = descricao;
            return this;

        }
     

    }
}
