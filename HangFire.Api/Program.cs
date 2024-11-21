using Hangfire;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
using HangFire.Api.Infra.Repositorio;
using HangFire.Api.Middleware;
using HangFire.Api.Servico;
using HangFire.Api.Util;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HangFire.Api
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            HelperConsoleColor.Alerta("Inicializando");

            var builder = WebApplication.CreateBuilder(args);

            HelperConsoleColor.Info("Builder - Configuração da connection string");
            // var connectionString = builder.Configuration["ConnectionStrings:Gravacao"];

            string filePath1 = "C:\\Amauri\\HangFireConnection.txt";
            string connectionString = File.ReadAllText(filePath1).Replace("\\\\", "\\");

            HelperConsoleColor.Info("Builder - Configuração do swagger");
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            HelperConsoleColor.Info("Builder - Configuração do serviço HangFire");
            builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();
            HelperConsoleColor.Info("Builder - Configuração do IDbConnection para o Dapper");
            builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));
            HelperConsoleColor.Info("Builder - Configuração do contexto");
            builder.Services.AddDbContext<CommandContext>(options => options.UseSqlServer(connectionString));
            HelperConsoleColor.Info("Builder - Configuração de dependências de repositórios");
            builder.Services.AddScoped<IMensagemRepositorio, MensagemRepositorio>();
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IHangFireRepositorio, HangFireRepositorio>();
            builder.Services.AddScoped<IMensagemErroRepositorio, MensagemErroRepositorio>();
            HelperConsoleColor.Info("Builder - Configuração do Mediatr");
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Aplicacao.UsuarioCommand.UsuarioInserirCommandHandler).Assembly));
            builder.Services.AddTransient<HangFireRegistrarMensagemComMediator>();
            HelperConsoleColor.Info("Builder - Hangfire possa resolver as dependências da classe HangFireServico");
            builder.Services.AddHttpClient<HangFireServico>();


            var app = builder.Build();

     
            HelperConsoleColor.Info("App - Configure the HTTP request pipeline.");
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            HelperConsoleColor.Info("App - Adiciona uma dashboard para monitoramento dos jobs");
            app.UseHangfireDashboard(); 
            HelperConsoleColor.Info("App - Mapeia a rota padrão para acessar a dashboard em hangfire");
            app.MapHangfireDashboard(); 


            //app.MapPost("usuarios/agendar", (IUsuarioRepositorio userRepository, [FromBody] Usuario user) =>
            //{
            //    BackgroundJob.Schedule(() => userRepository.Inserir(user.Codigo, user.Nome, user.Email), TimeSpan.FromMinutes(1));
            //    return Results.Ok("Usuário agendado para inserção daqui a 1 minuto!");
            //});

            //app.MapPost("usuarios/enfileirar", (IUsuarioRepositorio userRepository, [FromBody] Usuario user) =>
            //{
            //    BackgroundJob.Enqueue(() => userRepository.Inserir(user.Codigo, user.Nome, user.Email));
            //    return Results.Ok("Usuário enfileirado para inserção imediata!");
            //});

            HelperConsoleColor.Info("App - HangFireJobs.Inicializacao()");
            HangFireJobs.Inicializacao();

            //HelperConsoleColor.Info("Criação do jobs");
            //HangFireJobs.LimparJobsSucceededAntigosExecutaCadaMinuto();

            HelperConsoleColor.Info("App - TratamentoErroMiddleware");
            app.UseMiddleware<TratamentoErroMiddleware>();
            app.MapControllers();
            HelperConsoleColor.Sucesso("Finalizado!");
            app.Run();
        }
    }
}
