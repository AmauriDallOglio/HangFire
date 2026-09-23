using MediatR;
using HangFire.Api.Util;

namespace HangFire.Api.Aplicacao.HangfireCommand
{
    public class HangfireExcluirSucceededCommandRequest : IRequest<ResultadoOperacao<HangfireExcluirSucceededCommandResponse>>
    {
  
    }
}
