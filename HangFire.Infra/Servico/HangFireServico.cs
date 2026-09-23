using HangFire.Api.Aplicacao.HangfireCommand;
using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Aplicacao.MensagemErroCommand;
using HangFire.Api.Aplicacao.UsuarioCommand;
using HangFire.Api.Dominio.Entidade;
using HangFire.Api.Infra.Contexto;
using HangFire.Api.Util;
using MediatR;

namespace HangFire.Api.Servico
{
    public class HangFireServico
    {
        private readonly CommandContext _commandContext;
        private readonly IMediator _iMediator;

        public HangFireServico(CommandContext context, IMediator imediator)
        {
            _commandContext = context;
            _iMediator = imediator;
        }

        public async Task InicializacaoDoSistema()
        {
            MensagemInserirCommandRequest request = new MensagemInserirCommandRequest() { Descricao = "[InicializacaoDoSistema] Inicialização do sistema!" };
            ResultadoOperacao<MensagemInserirCommandResponse> response = await _iMediator.Send(request, new CancellationToken());
        }

        public async Task LimparJobsSucceededAntigos()
        {
 
            HangfireExcluirSucceededCommandRequest request = new HangfireExcluirSucceededCommandRequest() { };
            try
            {
                await _iMediator.Send(request, new CancellationToken());
            }
            catch (Exception ex)
            {
                string mensagemErro = ex.InnerException?.Message
                                        ?? ex.InnerException?.InnerException?.Message
                                        ?? ex.Message
                                        ?? "Erro não localizado!";
 
                string descricao = $"[LimparJobsSucceededAntigos/HangfireExcluirSucceededCommandHandler] Erro: {mensagemErro}.";
                Console.WriteLine(descricao);

                MensagemErroInserirCommandRequest requestErro = new MensagemErroInserirCommandRequest() { Descricao = descricao, Chamada = "HangFireServico" };
                ResultadoOperacao<MensagemErroInserirCommandResponse> responseErro = await _iMediator.Send(requestErro, new CancellationToken());

            }
 
        }

        public async Task TarefaInserirCincoUsuarios()
        {
            for (int i = 0; i <= 5; i++)
            {
                Usuario usuario = new Usuario().IncluirAutomaticamente();
                UsuarioInserirCommandRequest usuarioInserirCommandRequest = new UsuarioInserirCommandRequest().ConverterDto(usuario);
                var resultado = await _iMediator.Send(usuarioInserirCommandRequest);
 
            }
        }

    }
}
