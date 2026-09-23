namespace HangFire.Api.Dominio.Interface
{
    public interface IHangFireRepositorio
    {
        public Task<int> ExcluirRegistrosSucceeded();
    }
}
