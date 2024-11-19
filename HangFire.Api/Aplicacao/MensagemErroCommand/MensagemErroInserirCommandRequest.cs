using MediatR;

namespace HangFire.Api.Aplicacao.MensagemErroCommand
{
    public class MensagemErroInserirCommandRequest : IRequest<MensagemErroInserirCommandResponse>
    {
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public string Chamada { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;


    }
}
