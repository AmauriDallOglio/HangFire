using Dapper;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HangFire.Api.Infra.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly CommandContext _context;
        private readonly IDbConnection _dbConnection;

        public UsuarioRepositorio(CommandContext context)
        {

            _context = context;
            _dbConnection = _context.Database.GetDbConnection();

        }

        public async Task<Usuario> InserirDapperAsync(string codigo, string nome, string email)
        {
            var sql = "INSERT INTO Usuario (Codigo, Nome, Email) VALUES (@codigo, @Nome, @Email);";
            await _dbConnection.ExecuteAsync(sql, new { Codigo = codigo, Nome = nome, Email = email });

            return new Usuario();
        }

        public async Task<Usuario> InserirAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return new Usuario();
        }

    }
}