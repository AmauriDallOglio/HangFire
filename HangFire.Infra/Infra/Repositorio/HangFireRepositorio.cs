using Dapper;
using Hangfire;
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
                //var sql = @"DELETE FROM Hangfire.Job WHERE StateName = 'Succeeded' AND CreatedAt <= cast(GETDATE()-1 AS datetime);";
                //resultado = await _dbConnection.ExecuteAsync(sql);
                //HelperConsoleColor.Sucesso("HangFireRepositorio/ExcluirRegistrosSucceeded: Sucesso!");
                //return resultado;

                var dataLimite = DateTime.UtcNow.AddDays(-1);
                var monitor = JobStorage.Current.GetMonitoringApi();
                var pagina = 0;
                const int quantidadePorPagina = 100;

                while (true)
                {
                    var jobs = monitor.SucceededJobs(pagina * quantidadePorPagina, quantidadePorPagina);

                    if (!jobs.Any())
                    {
                        break;
                    }

                    foreach (var job in jobs)
                    {
                        if (job.Value.SucceededAt <= dataLimite && BackgroundJob.Delete(job.Key))
                        {
                            resultado++;
                        }
                    }

                    pagina++;
                }

                resultado += await CorrigirContadorSucceeded();
                HelperConsoleColor.Sucesso("HangFireRepositorio/ExcluirRegistrosSucceeded: Sucesso!");
                return resultado;
            }
            catch (Exception ex)
            {
                await new ArquivoLog().IncluirLinha("logs/error_log.txt", ex, "MensagemRepositorio", "MensagemRepositori: Erro ao gravar registro!");
            }
            return resultado;
        }

        private async Task<int> CorrigirContadorSucceeded()
        {
            var sql = @"
                DELETE FROM Hangfire.[Set]
                WHERE [Key] = 'succeeded'
                    AND NOT EXISTS (
                        SELECT 1
                        FROM Hangfire.Job
                        WHERE CAST(Hangfire.Job.Id AS nvarchar(100)) = Hangfire.[Set].[Value]
                            AND Hangfire.Job.StateName = 'Succeeded'
                    );

                DELETE FROM Hangfire.Counter
                WHERE [Key] = 'stats:succeeded';

                UPDATE Hangfire.AggregatedCounter
                SET [Value] = (
                    SELECT COUNT(1)
                    FROM Hangfire.Job
                    WHERE StateName = 'Succeeded'
                )
                WHERE [Key] = 'stats:succeeded';

                IF NOT EXISTS (
                    SELECT 1
                    FROM Hangfire.AggregatedCounter
                    WHERE [Key] = 'stats:succeeded'
                )
                BEGIN
                    INSERT INTO Hangfire.AggregatedCounter ([Key], [Value], [ExpireAt])
                    SELECT 'stats:succeeded', COUNT(1), NULL
                    FROM Hangfire.Job
                    WHERE StateName = 'Succeeded';
                END";

            return await _dbConnection.ExecuteAsync(sql);
        }

    }
}
