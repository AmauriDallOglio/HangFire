using System.ComponentModel.DataAnnotations.Schema;

namespace HangFire.Api.Dominio
{
    [Table("Mensagem")]
    public class Mensagem
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
