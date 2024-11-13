using HangFire.Api.Dominio.Entidade;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HangFire.Api.Infra.Contexto
{
    public class CommandContext : DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
