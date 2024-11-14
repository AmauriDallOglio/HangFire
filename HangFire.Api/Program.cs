using Hangfire;
using HangFire.Api.Dominio.Interface;
using HangFire.Api.Infra.Contexto;
using HangFire.Api.Infra.Repositorio;
using HangFire.Api.Servico;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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



            //Configura��o do swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //Configura��o do servi�o HangFire
            builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();
            // Configura��o do IDbConnection para o Dapper
            builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));
            //Configura��o do contexto
            builder.Services.AddDbContext<CommandContext>(options => options.UseSqlServer(connectionString));
            //Configura��o de depend�ncias de reposit�rios
            builder.Services.AddScoped<IMensagemRepositorio, MensagemRepositorio>();
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IHangFireRepositorio, HangFireRepositorio>();
            //Configura��o do Mediatr
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Aplicacao.UsuarioCommand.UsuarioInserirCommandHandler).Assembly));
            //Registro do HttpClient precisa ser registrado diretamente para o servi�o
            builder.Services.AddHttpClient<LimpaRegistroServico>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
        
            app.UseHangfireDashboard(); // Opcional: Adiciona uma dashboard para monitoramento dos jobs
            app.MapHangfireDashboard(); // Mapeia a rota padr�o para acessar a dashboard em "/hangfire"


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
 

            BackgroundJob.Enqueue<LimpaRegistroServico>(service => service.ProgramaEstartado());

            //RecurringJob.AddOrUpdate<LimpaRegistroServico>(
            //    "deletar-registros-antigos",
            //    service => service.DeletarRegistrosAntigos(),
            //    "00 36 15 * * *", // 11:34:10 = "10 34 11 * * *"
            //    TimeZoneInfo.Local);

            RecurringJob.AddOrUpdate<LimpaRegistroServico>(
                "limpar-jobs-antigos",
                service => service.LimparJobsSucceededAntigos(),
                "00 01 17 * * *", // Executa todos os dias �s 23:59
                TimeZoneInfo.Local);


            app.MapControllers();

            app.Run();
        }
    }
}
