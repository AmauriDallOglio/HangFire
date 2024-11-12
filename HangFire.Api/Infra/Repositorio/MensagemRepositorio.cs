using Dapper;
using HangFire.Api.Dominio.Interface;
using System.Data;

namespace HangFire.Api.Infra.Repositorio
{
    public class MensagemRepositorio(IDbConnection dbConnection) : IMensagemRepositorio
    {
        public void Inserir(string descricao)
        {
            var sql = "INSERT INTO Mensagem (Descricao) VALUES (@descricao);";
            dbConnection.Execute(sql, new { Descricao = descricao});
        }
    }
}
