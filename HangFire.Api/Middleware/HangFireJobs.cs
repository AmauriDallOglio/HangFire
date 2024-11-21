using Hangfire;
using HangFire.Api.Servico;

namespace HangFire.Api.Middleware
{
    [Obsolete]
    public class HangFireJobs
    {
        public static void Inicializacao()
        {
            //Executados uma única vez
            BackgroundJob.Enqueue<HangFireServico>(service => service.InicializacaoDoSistema());
        }

        public static void LimparJobsSucceededAntigosExecutaTodoDias2359()
        {
            //Executados repetidamente com base em uma programação CRON
            RecurringJob.AddOrUpdate<HangFireServico>(
                "Limpar-Jobs-Succeeded-Antigos-2359h",
                service => service.LimparJobsSucceededAntigos(),
                "00 02 08 * * *", // Executa todos os dias às 23:59
                TimeZoneInfo.Local);
        }

        public static void LimparJobsSucceededAntigosExecutaCadaHora()
        {
            //Executados repetidamente com base em uma programação CRON
            RecurringJob.AddOrUpdate<HangFireServico>(
                "Limpar-Jobs-Succeeded-Antigos-PorHora",  // Nome do job
                service => service.LimparJobsSucceededAntigos(), // Método da instância
                Cron.Hourly,
                TimeZoneInfo.Local            // Fuso horário local
            );
        }

        public static void LimparJobsSucceededAntigosExecutaCada15Minutos()
        {
            //Executados repetidamente com base em uma programação CRON
            RecurringJob.AddOrUpdate<HangFireServico>(
                "Limpar-Jobs-Succeeded-Antigos-Cada15Minutos",  // Nome do job
                service => service.LimparJobsSucceededAntigos(), // Método da instância
                "*/15 * * * *",                           // Frequência: a cada 15 minutos
                TimeZoneInfo.Local            // Fuso horário local
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LimparJobsSucceededAntigosExecutaCadaMinuto()
        {
            //Executados repetidamente com base em uma programação CRON
            RecurringJob.AddOrUpdate<HangFireServico>(
                "Limpar-Jobs-Succeeded-Antigos-CadaMinuto",  // Nome do job
                service => service.LimparJobsSucceededAntigos(), // Método da instância
                Cron.Minutely,                            // Frequência: a cada minuto
                TimeZoneInfo.Local            // Fuso horário local
            );
        }

        public static DateTimeOffset Segundos15()
        {
            var schudeleDateTime = DateTime.Now.AddSeconds(15);
            var datetimeoffSet = new DateTimeOffset(schudeleDateTime);
            return datetimeoffSet;
        }

        public static DateTimeOffset Segundos30()
        {
            var schudeleDateTime = DateTime.Now.AddSeconds(30);
            var datetimeoffSet = new DateTimeOffset(schudeleDateTime);
            return datetimeoffSet;
        }

    }
}
