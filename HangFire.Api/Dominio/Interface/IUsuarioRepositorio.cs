using HangFire.Api.Dominio.Entidade;

namespace HangFire.Api.Dominio.Interface
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario> InserirDapperAsync(string codigo, string nome, string email);

        Task<Usuario> InserirAsync(Usuario usuario);

    }

 
}


