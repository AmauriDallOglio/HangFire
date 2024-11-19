namespace HangFire.Api.Util
{
    public class MinhaExcecaoPersonalizada : Exception
    {
        public MinhaExcecaoPersonalizada(string mensagem) : base(mensagem) { }
    }
}
