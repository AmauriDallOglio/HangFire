using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HangFire.Api.Util;

namespace HangFire.Api.Dominio.Entidade
{
    [Table("MensagemErro")]
    public class MensagemErro : AtributoBase
    {
 
        [Required(ErrorMessage = "A chamada é obrigatória.")]
        [StringLength(200, ErrorMessage = "A chamada deve ter no máximo 200 caracteres.")]
        public string Chamada { get; private set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; private set; } = string.Empty;

        public MensagemErro IncluirDados(string descricao, string chamada)
        {
            Descricao = descricao;
            Chamada = chamada;

            return this;
        }

        public MensagemErro AlterarDescricao(string descricao)
        {
            Descricao = descricao;
 

            return this;
        }


    }
}
