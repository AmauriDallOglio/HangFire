using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HangFire.Api.Dominio.Entidade
{
    public class HangfireJob
    {
        public Int64 Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CreatedAt_Brasilia { get; set; }
        public string StateName { get; set; }
    }
}
