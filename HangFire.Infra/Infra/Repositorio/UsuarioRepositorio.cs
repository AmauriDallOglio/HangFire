using Dapper;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
using HangFire.Api.Util;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HangFire.Api.Infra.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly CommandContext _commandContext;
        private readonly IDbConnection _dbConnection;

        public UsuarioRepositorio(CommandContext context)
        {
            _commandContext = context;
            _dbConnection = _commandContext.Database.GetDbConnection();
        }

        public async Task<int> InserirDapperAsync(string codigo, string nome, string email)
        {
            var sql = "INSERT INTO Usuario (Codigo, Nome, Email) VALUES (@codigo, @Nome, @Email);";
            var resultado = await _dbConnection.ExecuteAsync(sql, new { Codigo = codigo, Nome = nome, Email = email });

            return resultado;
        }

        public async Task<int> InserirAsync(Usuario usuario)
        {
 
            var resultado = await _commandContext.Usuario.AddAsync(usuario);
            int gravado = await _commandContext.SaveChangesAsync();
            HelperConsoleColor.Sucesso("UsuarioRepositorio/InserirAsync: Sucesso!");
 

            return gravado;
        }

        public async Task<int> AlterarAsync(Usuario usuario)
        {

            var resultado = _commandContext.Usuario.Update(usuario);
            int gravado = await _commandContext.SaveChangesAsync();
 


            return gravado;
        }



        public async Task<Usuario> ObterPorIdAsync(int id)
        {
            Usuario resultado = await _commandContext.Usuario.AsNoTracking().Where(a => a.Id == id ).FirstOrDefaultAsync();
            return resultado;
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            Usuario? resultado = await _commandContext.Usuario.AsNoTracking().Where(a => a.Email == email).FirstOrDefaultAsync();
            return resultado;
        }

    }
}
