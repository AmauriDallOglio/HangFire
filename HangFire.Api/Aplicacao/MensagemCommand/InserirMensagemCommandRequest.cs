using HangFire.Api.Aplicacao.UsuarioCommand;
using MediatR;

namespace HangFire.Api.Aplicacao.MensagemCommand
{
    public class InserirMensagemCommandRequest : IRequest<InserirMensagemCommandResponse>
    {
 

            public string Descricao { get; set; } = string.Empty;
 
    }
}
