namespace HangfireDemo.Services
{
    public interface IJobService
    {
        /// <summary>
        /// A simple job that logs a message
        /// </summary>
        Task SimpleJob(string message);

        /// <summary>
        /// A job that demonstrates retry mechanism by throwing an exception
        /// </summary>
        Task JobWithRetry(int attemptNumber);

        /// <summary>
        /// A job that simulates long-running work
        /// </summary>
        Task LongRunningJob(int durationInSeconds);

        /// <summary>
        /// A job that processes data and shows persistence
        /// </summary>
        Task DataProcessingJob(string data);

        /// <summary>
        /// A job that sends email notification
        /// </summary>
        Task SendEmailJob(string recipient, string subject);
    }
}
