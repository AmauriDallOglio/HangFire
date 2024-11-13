using Hangfire;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
using HangFire.Api.Infra.Repositorio;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace HangFire.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           // var connectionString = builder.Configuration["ConnectionStrings:Gravacao"];

            string filePath1 = "C:\\Amauri\\HangFireConnection.txt";
            string connectionString = File.ReadAllText(filePath1).Replace("\\\\", "\\");




            builder.Services.AddControllers();
 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();
            // Configuração do IDbConnection para o Dapper
            builder.Services.AddSingleton<IDbConnection>(sp => new SqlConnection(connectionString));
            //Configuração do contexto
            builder.Services.AddDbContext<CommandContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IMensagemRepositorio, MensagemRepositorio>();
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Aplicacao.UsuarioCommand.InserirUsuarioCommandHandler).Assembly));
 


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
        
            app.UseHangfireDashboard();

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

            app.MapControllers();

            app.Run();
        }
    }
}
