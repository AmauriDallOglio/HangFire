namespace HangFire.Api.Util
{
    public class ArquivoLog
    {
        public async Task<string> IncluirLinha(string caminhoNomeArquivo, Exception ex, string requestPath, string mensagemBasica)
        {
            var mensagemPersonalizada = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | TratamentoErroMiddleware | Path: {requestPath} | {mensagemBasica}: {ex.Message} {Environment.NewLine}";
            File.AppendAllText(caminhoNomeArquivo, mensagemPersonalizada);
            Console.WriteLine($"Erro: {mensagemPersonalizada}");

            return mensagemPersonalizada;
        }
    }
}
