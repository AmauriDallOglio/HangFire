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

            HelperConsoleColor.Info("Builder - Configura��o da connection string");
            // var connectionString = builder.Configuration["ConnectionStrings:Gravacao"];

            string filePath1 = "C:\\Amauri\\HangFireConnection.txt";
            string connectionString = File.ReadAllText(filePath1).Replace("\\\\", "\\");

            HelperConsoleColor.Info("Builder - Configura��o do swagger");
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            HelperConsoleColor.Info("Builder - Configura��o do servi�o HangFire");
            builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();
            HelperConsoleColor.Info("Builder - Configura��o do IDbConnection para o Dapper");
            builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));
            HelperConsoleColor.Info("Builder - Configura��o do contexto");
            builder.Services.AddDbContext<CommandContext>(options => options.UseSqlServer(connectionString));
            HelperConsoleColor.Info("Builder - Configura��o de depend�ncias de reposit�rios");
            builder.Services.AddScoped<IMensagemRepositorio, MensagemRepositorio>();
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IHangFireRepositorio, HangFireRepositorio>();
            builder.Services.AddScoped<IMensagemErroRepositorio, MensagemErroRepositorio>();
            HelperConsoleColor.Info("Builder - Configura��o do Mediatr");
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Aplicacao.UsuarioCommand.UsuarioInserirCommandHandler).Assembly));
            builder.Services.AddTransient<HangFireRegistrarMensagemComMediator>();
            HelperConsoleColor.Info("Builder - Hangfire possa resolver as depend�ncias da classe HangFireServico");
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
            HelperConsoleColor.Info("App - Mapeia a rota padr�o para acessar a dashboard em hangfire");
            app.MapHangfireDashboard(); 


            //app.MapPost("usuarios/agendar", (IUsuarioRepositorio userRepository, [FromBody] Usuario user) =>
            //{
            //    BackgroundJob.Schedule(() => userRepository.Inserir(user.Codigo, user.Nome, user.Email), TimeSpan.FromMinutes(1));
            //    return Results.Ok("Usu�rio agendado para inser��o daqui a 1 minuto!");
            //});

            //app.MapPost("usuarios/enfileirar", (IUsuarioRepositorio userRepository, [FromBody] Usuario user) =>
            //{
            //    BackgroundJob.Enqueue(() => userRepository.Inserir(user.Codigo, user.Nome, user.Email));
            //    return Results.Ok("Usu�rio enfileirado para inser��o imediata!");
            //});

            HelperConsoleColor.Info("App - HangFireJobs.Inicializacao()");
            HangFireJobs.Inicializacao();

            //HelperConsoleColor.Info("Cria��o do jobs");
            //HangFireJobs.LimparJobsSucceededAntigosExecutaCadaMinuto();

            HelperConsoleColor.Info("App - TratamentoErroMiddleware");
            app.UseMiddleware<TratamentoErroMiddleware>();
            app.MapControllers();
            HelperConsoleColor.Sucesso("Finalizado!");
            app.Run();
        }
    }
}
