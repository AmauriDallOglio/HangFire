using Dapper;
using HangFire.Api.Dominio.Interface;
using System.Data;
using System.Xml.Linq;

namespace HangFire.Api.Infra.Repositorio
{
    public class UsuarioRepositorio(IDbConnection dbConnection) : IUsuarioRepositorio
    {
        public void Inserir(string codigo, string nome, string email)
        {
            var sql = "INSERT INTO Usuario (Codigo, Nome, Email) VALUES (@codigo, @Nome, @Email);";
            dbConnection.Execute(sql, new { Codigo = codigo, Nome = nome, Email = email });
        }
    }
}

 