using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
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
            await _commandContext.MensagemErro.AddAsync(mensagemErro);
            int gravado = await _commandContext.SaveChangesAsync();
            return gravado;
        }
    }
}
