using MediatR;
using HangFire.Api.Util;

namespace HangFire.Api.Aplicacao.MensagemCommand
{
    public class MensagemInserirCommandRequest : IRequest<ResultadoOperacao<MensagemInserirCommandResponse>>
    {
        public string Descricao { get; set; } = string.Empty;
 
    }
}
