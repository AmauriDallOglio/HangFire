using Dapper;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
using HangFire.Api.Util;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HangFire.Api.Infra.Repositorio
{
    public class MensagemRepositorio : IMensagemRepositorio
    {
        private readonly CommandContext _commandContext;
        private readonly IDbConnection _dbConnection;
        public MensagemRepositorio(CommandContext commandContext) 
        {
            _commandContext = commandContext;
            _dbConnection = _commandContext.Database.GetDbConnection();
        }

        public async Task<int> InserirDapperAsync(string descricao)
        {
            var sql = "INSERT INTO Mensagem (Descricao) VALUES (@descricao);";
            var resultado = await _dbConnection.ExecuteAsync(sql, new { Descricao = descricao});
            return resultado;
        }

        public async Task<int> InserirAsync(Mensagem mensagem)
        {
            int gravado = 0;
            try
            {
                await _commandContext.Mensagem.AddAsync(mensagem);
                gravado = await _commandContext.SaveChangesAsync();
                HelperConsoleColor.Sucesso("MensagemRepositorio/InserirAsync: Sucesso!");
            }
            catch (Exception ex)
            {
                await new ArquivoLog().IncluirLinha("logs/error_log.txt", ex, "MensagemRepositorio/InserirAsync", "MensagemRepositori: Erro ao gravar registro!");
            }
            return gravado;
        }
    }
}
