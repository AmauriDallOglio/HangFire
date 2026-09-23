using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
using HangFire.Api.Util;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HangFire.Api.Infra.Repositorio
{
    public class MensagemErroRepositorio : IMensagemErroRepositorio
    {
        private readonly CommandContext _commandContext;
        private readonly IDbConnection _dbConnection;
        public MensagemErroRepositorio(CommandContext commandContext)
        {
            _commandContext = commandContext;
            _dbConnection = _commandContext.Database.GetDbConnection();
        }

 
        public async Task<int> InserirAsync(MensagemErro mensagemErro)
        {
            int gravado = 0;
            try
            {
                await _commandContext.MensagemErro.AddAsync(mensagemErro);
                gravado = await _commandContext.SaveChangesAsync();
                HelperConsoleColor.Sucesso("MensagemErroRepositorio/InserirAsync: Sucesso!");
            }
            catch (Exception ex)
            {
                await new ArquivoLog().IncluirLinha("logs/error_log.txt", ex, "MensagemErroRepositorio/InserirAsync", "MensagemRepositori: Erro ao gravar registro!");
            }
            return gravado;
        }
    }
}
