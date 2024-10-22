using EFCoreRelationshipsTutorial.DTO;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreRelationshipsTutorial.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {

        private readonly JobService _jobService;

        public JobController(JobService jobService)
        {
            _jobService = jobService;
        }

        // Endpoint para agendar um job simples
        [HttpPost("fire-and-forget")]
        public IActionResult ScheduleFireAndForgetJob()
        {
            // Agendar um job para ser executado imediatamente (Fire-and-Forget)
            BackgroundJob.Enqueue(() => _jobService.FetchCharacters());

            return Ok("Job agendado com sucesso!");
        }

        [HttpPost("recurring")]
        public IActionResult ScheduleRecurringJob([FromBody] RecurringJobRequest request)
        {
            // Validar o intervalo e gerar a expressão cron adequada
            string cronExpression;

            switch (request.IntervalType.ToLower())
            {
                case "minutos":
                    cronExpression = Cron.MinuteInterval(request.Interval);
                    break;
                case "horas":
                    cronExpression = Cron.HourInterval(request.Interval);
                    break;
                case "dias":
                    cronExpression = Cron.DayInterval(request.Interval);
                    break;
                default:
                    return BadRequest("Tipo de intervalo inválido. Use 'minutos', 'horas' ou 'dias'.");
            }

            RecurringJob.AddOrUpdate(request.JobName, () => _jobService.FetchCharacters(), cronExpression);

            return Ok($"Job recorrente '{request.JobName}' agendado com sucesso para executar a cada {request.Interval} {request.IntervalType}.");
        }

        // Endpoint para agendar um job para ser executado em um momento futuro
        [HttpPost("delayed")]
        public IActionResult ScheduleDelayedJob()
        {
            // Agendar um job para ser executado após 1 minuto
            BackgroundJob.Schedule(() => Console.WriteLine("Delayed job executado após 1 minuto!"), TimeSpan.FromMinutes(1));

            return Ok("Job agendado para ser executado após 1 minuto!");
        }
    }
}
