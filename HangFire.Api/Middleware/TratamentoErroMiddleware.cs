using HangFire.Api.Aplicacao.MensagemCommand;
using HangFire.Api.Util;
using MediatR;

namespace HangFire.Api.Middleware
{
    public class TratamentoErroMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _caminhoLog = "logs/error_log.txt";
        private readonly IMediator _mediator;
        private static PathString _PathString { get; set; }

        public TratamentoErroMiddleware(RequestDelegate next, IMediator mediator)
        {
            _next = next;
            _mediator = mediator;
            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }
            _PathString = string.Empty;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _PathString = context.Request.Path;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {      
                await TratamentoExceptionAsync(context, ex);
            }
        }

        private async Task TratamentoExceptionAsync(HttpContext context, Exception exception)
        {
           

            var httpDicionarioCodigoErros = new Dictionary<Type, int>
            {
                { typeof(ArgumentException), StatusCodes.Status400BadRequest },
                { typeof(KeyNotFoundException), StatusCodes.Status404NotFound },
                { typeof(InvalidOperationException), StatusCodes.Status409Conflict },
                { typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized },
                { typeof(FormatException), StatusCodes.Status422UnprocessableEntity },
                { typeof(NullReferenceException), StatusCodes.Status500InternalServerError },
                { typeof(DivideByZeroException), StatusCodes.Status400BadRequest },
                { typeof(MinhaExcecaoPersonalizada), StatusCodes.Status418ImATeapot } 
            };

            var httpCodigoErro = httpDicionarioCodigoErros.TryGetValue(exception.GetType(), out var code)
                ? code
                : StatusCodes.Status500InternalServerError;

            string mensagemDoLog = await new ArquivoLog().IncluirLinha(_caminhoLog, exception, _PathString, "Erro inesperado");
          

            var response = new
            {
                Codigo = httpCodigoErro,
                Mensagem = exception.Message,
                Detalhe = mensagemDoLog,
            };

            try
            {
                //No ASP.NET Core, middlewares são geralmente singleton, o uso de using com CreateScope é uma prática válida e eficaz para resolver o problema de
                //ciclos de vida scoped em middlewares singleton no ASP.NET Core. Ele garante que o escopo criado seja descartado automaticamente após o uso.
                using (var scope = context.RequestServices.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    string mensagem = mensagemDoLog;
                    MensagemInserirCommandRequest mensagemInserirCommandRequest = new MensagemInserirCommandRequest { Descricao = mensagem };
                    await mediator.Send(mensagemInserirCommandRequest);
                }

                //////Para gerar o log
                //string mensagemd = $"{DateTime.Now} TratamentoErroMiddleware: {exception.Message}";
                //MensagemInserirCommandRequest mensagemInserirCommandRequestd = new MensagemInserirCommandRequest { Descricao = mensagemd };
                //await _mediator.Send(mensagemInserirCommandRequestd);
            }
            catch (Exception mediatorEx)
            {
                await new ArquivoLog().IncluirLinha(_caminhoLog, mediatorEx, _PathString, "Erro no CreateScope");
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }



    }
}
