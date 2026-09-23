using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HangFire.Api.Util
{
    public class AtributoBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        public DateTime DataCadastro { get; private set; } = DateTime.Now;


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
