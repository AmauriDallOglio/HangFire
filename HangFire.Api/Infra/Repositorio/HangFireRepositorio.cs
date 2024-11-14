using Dapper;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HangFire.Api.Infra.Repositorio
{
    public class HangFireRepositorio : IHangFireRepositorio
    {
        private readonly CommandContext _commandContext;
        private readonly IDbConnection _dbConnection;
        public HangFireRepositorio(CommandContext commandContext)
        {
            _commandContext = commandContext;
            _dbConnection = _commandContext.Database.GetDbConnection();
        }



        public async Task<int> ExcluirRegistrosSucceeded()
        {
            var sql = @"DELETE FROM Hangfire.Job WHERE StateName = 'Succeeded' ";

            var resultado = await _dbConnection.ExecuteAsync(sql);

            return resultado;
        }

    }
}
