using MediatR;

namespace HangFire.Api.Aplicacao.MensagemErroCommand
{
    public class MensagemErroInserirCommandRequest : IRequest<MensagemErroInserirCommandResponse>
    {
        public string Chamada { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;


    }
}
