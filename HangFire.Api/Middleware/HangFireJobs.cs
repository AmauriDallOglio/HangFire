using Hangfire;
using HangFire.Api.Servico;

namespace HangFire.Api.Middleware
{
    [Obsolete]
    public class HangFireJobs
    {

        public static void LimparJobsSucceededAntigosExecutaTodoDias2359()
        {
            RecurringJob.AddOrUpdate<HangFireServico>(
                "Limpar-Jobs-Succeeded-Antigos",
                service => service.LimparJobsSucceededAntigos(),
                "00 02 08 * * *", // Executa todos os dias às 23:59
                TimeZoneInfo.Local);
        }

        public static void LimparJobsSucceededAntigosExecutaCadaHora()
        {
            RecurringJob.AddOrUpdate<HangFireServico>(
                "Limpar-Jobs-Succeeded-Antigos",  // Nome do job
                service => service.LimparJobsSucceededAntigos(), // Método da instância
                Cron.Hourly,
                TimeZoneInfo.Local            // Fuso horário local
            );
        }

        public static void LimparJobsSucceededAntigosExecutaCada15Minutos()
        {
            RecurringJob.AddOrUpdate<HangFireServico>(
                "Limpar-Jobs-Succeeded-Antigos",  // Nome do job
                service => service.LimparJobsSucceededAntigos(), // Método da instância
                "*/15 * * * *",                           // Frequência: a cada 15 minutos
                TimeZoneInfo.Local            // Fuso horário local
            );
        }

        public static void LimparJobsSucceededAntigosExecutaCadaMinuto()
        {
            RecurringJob.AddOrUpdate<HangFireServico>(
                "Limpar-Jobs-Succeeded-Antigos",  // Nome do job
                service => service.LimparJobsSucceededAntigos(), // Método da instância
                Cron.Minutely,                            // Frequência: a cada minuto
                TimeZoneInfo.Local            // Fuso horário local
            );
        }

    }
}
