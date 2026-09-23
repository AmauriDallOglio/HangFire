using HangFire.Api.Dominio.Entidade;
using Microsoft.EntityFrameworkCore;

namespace HangFire.Api.Infra.Contexto
{
    public class CommandContext : DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> options) : base(options)
        {
        
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Mensagem> Mensagem { get; set; }
        public DbSet<MensagemErro> MensagemErro { get; set; }
        public DbSet<HangfireJob> HangfireJob { get; set; }
    }
}
