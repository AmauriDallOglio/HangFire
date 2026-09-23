namespace HangFire.Api.Util
{
    public class ResultadoOperacao
    {
        public bool Sucesso { get; set; } = true;
        public string Mensagem { get; set; } = string.Empty;
        public string? Detalhe { get; set; }
        public CodigoRetornoOperacao CodigoRetorno { get; set; } = CodigoRetornoOperacao.Sucesso;
        public TipoRetorno TipoRetorno { get; set; }
        public long TempoResposta { get; set; }
    }

    public class ResultadoOperacao<T> : ResultadoOperacao
    {
        public T? Resultado { get; set; }

        public static Task<ResultadoOperacao<T>> RetornaSuccessoAsync(T data, string mensagem, CodigoRetornoOperacao codigoRetorno = CodigoRetornoOperacao.Sucesso)
        {
            return Task.FromResult(new ResultadoOperacao<T>
            {
                Resultado = data,
                Sucesso = true,
                Mensagem = mensagem,
                CodigoRetorno = codigoRetorno,
                TipoRetorno = TipoRetorno.Sucesso
            });
        }

        public static Task<ResultadoOperacao<T>> RetornaSuccessoAsync(string mensagem, CodigoRetornoOperacao codigoRetorno = CodigoRetornoOperacao.Sucesso)
        {
            return Task.FromResult(new ResultadoOperacao<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                CodigoRetorno = codigoRetorno,
                TipoRetorno = TipoRetorno.Sucesso
            });
        }

        public static Task<ResultadoOperacao<T>> RetornaFalhaAsync(string mensagem, CodigoRetornoOperacao codigoRetorno = CodigoRetornoOperacao.FalhaNegocio, string detalhe = "")
        {
            return Task.FromResult(new ResultadoOperacao<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Detalhe = detalhe,
                CodigoRetorno = codigoRetorno,
                TipoRetorno = TipoRetorno.Falha
            });
        }
    }

    public enum TipoRetorno : byte
    {
        Sucesso = 0,
        Falha = 1,
        Parcial = 2
    }

    public enum CodigoRetornoOperacao : int
    {
        Sucesso = 100,
        SucessoParcial = 101,
        FalhaNegocio = 200,
        DadosInvalidos = 201,
        ObjetoNaoEncontrado = 202,
        OperacaoInvalida = 203,
        ConflitoRegraNegocio = 204
    }
}
