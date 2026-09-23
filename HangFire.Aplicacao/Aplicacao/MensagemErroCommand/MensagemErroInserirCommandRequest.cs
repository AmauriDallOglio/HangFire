using MediatR;
using HangFire.Api.Util;

namespace HangFire.Api.Aplicacao.MensagemErroCommand
{
    public class MensagemErroInserirCommandRequest : IRequest<ResultadoOperacao<MensagemErroInserirCommandResponse>>
    {
        public string Chamada { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;


    }
}
