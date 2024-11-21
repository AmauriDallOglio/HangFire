using Dapper;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
using HangFire.Api.Util;
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
            int resultado = 0;
            try
            {
                var sql = @"DELETE FROM Hangfire.Job WHERE StateName = 'Succeeded' AND CreatedAt <= cast(GETDATE()-1 AS datetime);";

                resultado = await _dbConnection.ExecuteAsync(sql);
                HelperConsoleColor.Sucesso("HangFireRepositorio/ExcluirRegistrosSucceeded: Sucesso!");
                return resultado;
            }
            catch (Exception ex)
            {
                await new ArquivoLog().IncluirLinha("logs/error_log.txt", ex, "MensagemRepositorio", "MensagemRepositori: Erro ao gravar registro!");
            }
            return resultado;
        }

    }
}
