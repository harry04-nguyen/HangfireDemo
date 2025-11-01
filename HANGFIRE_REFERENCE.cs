//// Hangfire Job Types Reference

//// ============================================
//// 1. FIRE-AND-FORGET JOBS
//// ============================================
//// Execute once, immediately, in background
//// Use case: Send email, generate report, log action

//BackgroundJob.Enqueue<IJobService>(x => x.SimpleJob("Hello"));

//// ============================================
//// 2. DELAYED JOBS
//// ============================================
//// Execute once, after specified time delay
//// Use case: Send reminder, cleanup old data

//BackgroundJob.Schedule<IJobService>(
//    x => x.SimpleJob("Delayed"),
//    TimeSpan.FromMinutes(30));

//// ============================================
//// 3. RECURRING JOBS
//// ============================================
//// Execute repeatedly on a schedule (Cron)
//// Use case: Daily reports, hourly sync, cleanup

//RecurringJob.AddOrUpdate<IJobService>(
//  "my-job-id",
//    x => x.SimpleJob("Recurring"),
//    Cron.Daily);

//// Common Cron expressions:
//// Cron.Minutely()      - Every minute
//// Cron.Hourly()   - Every hour
//// Cron.Daily()- Every day at midnight
//// Cron.Weekly()        - Every Sunday at midnight
//// Cron.Monthly()       - First day of month at midnight
//// Cron.Yearly()        - January 1st at midnight
//// "*/5 * * * *"        - Every 5 minutes
//// "0 9 * * *"          - Every day at 9 AM
//// "0 0 * * 1"   - Every Monday at midnight

//// ============================================
//// 4. CONTINUATION JOBS
//// ============================================
//// Execute after parent job completes
//// Use case: Multi-step workflows, dependent tasks

//var parentId = BackgroundJob.Enqueue<IJobService>(
//    x => x.DataProcessingJob("data"));

//BackgroundJob.ContinueJobWith<IJobService>(
//    parentId,
//    x => x.SendEmailJob("user@example.com", "Complete"));

//// ============================================
//// 5. BATCH JOBS
//// ============================================
//// Execute multiple jobs together
//// Use case: Parallel processing, bulk operations

//for (int i = 0; i < 10; i++)
//{
//    var index = i;
//    BackgroundJob.Enqueue<IJobService>(
//x => x.SimpleJob($"Batch job {index}"));
//}

//// ============================================
//// JOB RETRY CONFIGURATION
//// ============================================
//// Hangfire automatically retries failed jobs
//// Default: 10 attempts with exponential backoff

//// Custom retry attempts (using AutomaticRetry attribute):
//[AutomaticRetry(Attempts = 3)]
//public async Task MyRetryJob()
//{
//    // Job implementation
//}

//// Disable automatic retry:
//[AutomaticRetry(Attempts = 0)]
//public async Task NoRetryJob()
//{
//// Job implementation
//}

//// ============================================
//// JOB CANCELLATION
//// ============================================
//// Support for cancellation tokens

//public async Task CancellableJob(CancellationToken cancellationToken)
//{
//    for (int i = 0; i < 100; i++)
//    {
//        cancellationToken.ThrowIfCancellationRequested();
//        await Task.Delay(1000, cancellationToken);
//    }
//}

//// ============================================
//// JOB FILTERS
//// ============================================
//// Add custom behavior to jobs

//// Preserve original queue attribute
//[Queue("critical")]
//public async Task CriticalJob()
//{
//    // This job will be processed by 'critical' queue
//}

//// Disable concurrent execution
//[DisableConcurrentExecution(timeoutInSeconds: 60)]
//public async Task SingleInstanceJob()
//{
//    // Only one instance can run at a time
//}

//// ============================================
//// USEFUL DASHBOARD URLS
//// ============================================
//// /hangfire   - Dashboard home
//// /hangfire/jobs/enqueued      - Enqueued jobs
//// /hangfire/jobs/processing    - Currently processing
//// /hangfire/jobs/scheduled     - Scheduled jobs
//// /hangfire/jobs/succeeded     - Successful jobs
//// /hangfire/jobs/failed   - Failed jobs
//// /hangfire/recurring          - Recurring jobs
//// /hangfire/retries            - Jobs pending retry

//// ============================================
//// DATABASE TABLES (SQL Server)
//// ============================================
//// HangFire.Job    - Job definitions
//// HangFire.State        - Job state history
//// HangFire.Set              - Scheduled/recurring jobs
//// HangFire.List - Job queues
//// HangFire.Hash         - Job parameters
//// HangFire.Counter   - Statistics counters
//// HangFire.AggregatedCounter   - Aggregated statistics
//// HangFire.Server    - Active servers

//// ============================================
//// BEST PRACTICES
//// ============================================
//// 1. Keep jobs small and focused
//// 2. Make jobs idempotent (safe to retry)
//// 3. Use proper exception handling
//// 4. Log important events
//// 5. Monitor dashboard regularly
//// 6. Set appropriate retry limits
//// 7. Use queues for priority jobs
//// 8. Clean up old job data periodically
//// 9. Secure dashboard in production
//// 10. Use dependency injection for services
