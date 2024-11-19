using HangFire.Api.Dominio.Entidade;

namespace HangFire.Api.Dominio.Interface
{
    public interface IUsuarioRepositorio
    {
        Task<int> InserirDapperAsync(string codigo, string nome, string email);
        Task<int> InserirAsync(Usuario usuario);
    }
}


