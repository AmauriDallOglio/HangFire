using Dapper;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
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
            //try
            //{
                await _commandContext.Mensagem.AddAsync(mensagem);
                var gravado = await _commandContext.SaveChangesAsync();
                return gravado;
            //}
            //catch (DbUpdateException ex)
            //{
            //    // Tratamento específico para erros de atualização no Entity Framework
            //    Console.WriteLine($"Erro ao inserir mensagem com Entity Framework: {ex.Message}");
            //    throw new InvalidOperationException("Ocorreu um erro ao inserir a mensagem no banco de dados.", ex);
            //}
            //catch (Exception ex)
            //{
            //    // Tratamento genérico para outros tipos de exceção
            //    Console.WriteLine($"Erro inesperado: {ex.Message}");
            //    throw new InvalidOperationException("Ocorreu um erro ao processar a operação de inserção.", ex);
            //}


        }
    }
}
