using HangFire.Api.Dominio.Entidade;

namespace HangFire.Api.Dominio.Interface
{
    public interface IMensagemErroRepositorio
    {
        Task<int> InserirAsync(MensagemErro mensagemErro);
    }
}
