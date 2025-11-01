namespace HangfireDemo.Services
{
    public class JobService : IJobService
    {
        private readonly ILogger<JobService> _logger;

        public JobService(ILogger<JobService> logger)
        {
            _logger = logger;
        }

        public async Task SimpleJob(string message)
        {
            _logger.LogInformation($"[SimpleJob] Executing at {DateTime.Now}: {message}");
            await Task.Delay(1000); // Simulate work
            _logger.LogInformation($"[SimpleJob] Completed at {DateTime.Now}");
        }

        public async Task JobWithRetry(int attemptNumber)
        {
            _logger.LogWarning($"[JobWithRetry] Attempt #{attemptNumber} at {DateTime.Now}");

            // This job will fail on the first 2 attempts, then succeed on the 3rd
            if (attemptNumber < 3)
            {
                _logger.LogError($"[JobWithRetry] Simulating failure on attempt #{attemptNumber}");
                throw new InvalidOperationException(
                    $"Simulated failure on attempt {attemptNumber}"
                );
            }

            await Task.Delay(500);
            _logger.LogInformation(
                $"[JobWithRetry] Successfully completed on attempt #{attemptNumber}"
            );
        }

        public async Task LongRunningJob(int durationInSeconds)
        {
            _logger.LogInformation(
                $"[LongRunningJob] Starting long-running job for {durationInSeconds} seconds at {DateTime.Now}"
            );

            for (int i = 1; i <= durationInSeconds; i++)
            {
                await Task.Delay(1000);
                _logger.LogInformation(
                    $"[LongRunningJob] Progress: {i}/{durationInSeconds} seconds elapsed"
                );
            }

            _logger.LogInformation($"[LongRunningJob] Completed at {DateTime.Now}");
        }

        public async Task DataProcessingJob(string data)
        {
            _logger.LogInformation(
                $"[DataProcessingJob] Started processing data at {DateTime.Now}"
            );
            _logger.LogInformation($"[DataProcessingJob] Data received: {data}");

            // Simulate data processing
            await Task.Delay(2000);

            var processedData = data.ToUpper();
            _logger.LogInformation($"[DataProcessingJob] Data processed: {processedData}");

            // Simulate saving to database
            await Task.Delay(1000);
            _logger.LogInformation(
                $"[DataProcessingJob] Data persisted to database at {DateTime.Now}"
            );
        }

        public async Task SendEmailJob(string recipient, string subject)
        {
            _logger.LogInformation($"[SendEmailJob] Preparing to send email at {DateTime.Now}");
            _logger.LogInformation($"[SendEmailJob] To: {recipient}, Subject: {subject}");

            // Simulate email sending
            await Task.Delay(1500);

            _logger.LogInformation($"[SendEmailJob] Email sent successfully at {DateTime.Now}");
        }
    }
}
