using HangFire.Api.Dominio.Entidade;

namespace HangFire.Api.Dominio.Interface
{
    public interface IMensagemRepositorio
    {
        Task<int> InserirDapperAsync(string descricao);
        Task<int> InserirAsync(Mensagem mensagem);
    }
}
