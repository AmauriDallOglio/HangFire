using MediatR;

namespace HangFire.Api.Aplicacao.MensagemCommand
{
    public class MensagemInserirCommandRequest : IRequest<MensagemInserirCommandResponse>
    {
 

            public string Descricao { get; set; } = string.Empty;
 
    }
}
