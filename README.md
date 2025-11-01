# Hangfire Demo Project

A comprehensive ASP.NET Core 8 demonstration of Hangfire background job processing featuring job scheduling, retry mechanisms, and job persistence.

## Features Implemented

### 1. **Job Scheduling**
- **Fire-and-Forget Jobs**: Execute immediately in the background
- **Delayed Jobs**: Schedule jobs to run after a specified time delay
- **Recurring Jobs**: Automatically scheduled jobs using Cron expressions
  - Simple recurring job: Runs every minute
- Data processing job: Runs every hour

### 2. **Retry Mechanism**
- Automatic retry on job failures
- Exponential backoff strategy
- Demo job that fails twice then succeeds on the third attempt

### 3. **Job Persistence**
- All jobs stored in SQL Server database
- Jobs survive application restarts
- Full job history and state tracking

### 4. **Additional Features**
- Long-running job support
- Job continuations (chain jobs)
- Batch job processing
- Real-time dashboard for monitoring
- Comprehensive logging

## Project Structure

```
HangfireDemo/
??? Controllers/
?   ??? HomeController.cs          # Job trigger endpoints
??? Services/
?   ??? IJobService.cs        # Job service interface
?   ??? JobService.cs    # Job implementations
??? Views/
?   ??? Home/
?    ??? Index.cshtml    # UI with job trigger buttons
??? Program.cs     # Hangfire configuration
??? appsettings.json    # Connection string
```

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server LocalDB (or SQL Server instance)
- Visual Studio 2022 or VS Code

### Installation Steps

1. **Restore NuGet packages** (if not already done):
   ```bash
   dotnet restore
   ```

2. **Update Connection String** (if needed):
   Edit `appsettings.json` and modify the connection string to match your SQL Server instance:
   ```json
   "ConnectionStrings": {
     "HangfireConnection": "Server=(localdb)\\mssqllocaldb;Database=HangfireDemo;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```
   
   **Note**: `TrustServerCertificate=True` is required for LocalDB. For production environments with proper SSL certificates, you may remove this parameter.

3. **Run the Application**:
   ```bash
   dotnet run
   ```
   
   The database and Hangfire tables will be created automatically on first run.

4. **Access the Application**:
   - Main page: `https://localhost:5001` (or the port shown in console)
   - Hangfire Dashboard: `https://localhost:5001/hangfire`

## Using the Demo

### Main Page Features

1. **Fire-and-Forget Job**: Click "Enqueue Simple Job" to execute a job immediately
2. **Job with Retry**: Click "Enqueue Retry Job" to see automatic retry behavior
3. **Delayed Job**: Click "Schedule Delayed Job" to schedule a job 2 minutes in the future
4. **Long-Running Job**: Click "Start Long Job" to execute a 10-second job
5. **Data Processing**: Enter data and click "Process Data" to see persistence
6. **Continuation Job**: Click "Start Chain Jobs" to see job chaining
7. **Batch Jobs**: Click "Enqueue 5 Jobs" to process multiple jobs in parallel

### Hangfire Dashboard

Access `/hangfire` to:
- Monitor job execution in real-time
- View succeeded, failed, and processing jobs
- See recurring job schedules
- Manually retry failed jobs
- View detailed job information and logs

## Key Hangfire Concepts Demonstrated

### 1. Background Jobs
```csharp
// Fire-and-forget
BackgroundJob.Enqueue<IJobService>(x => x.SimpleJob("message"));

// Delayed
BackgroundJob.Schedule<IJobService>(x => x.SimpleJob("message"), TimeSpan.FromMinutes(2));

// Continuation
BackgroundJob.ContinueJobWith<IJobService>(parentJobId, x => x.SendEmailJob("email", "subject"));
```

### 2. Recurring Jobs
```csharp
RecurringJob.AddOrUpdate<IJobService>(
    "job-id",
    service => service.SimpleJob("message"),
    Cron.Minutely);
```

### 3. Job Persistence
All jobs are persisted in SQL Server tables:
- `HangFire.Job` - Job definitions
- `HangFire.State` - Job states (enqueued, processing, succeeded, failed)
- `HangFire.Set` - Scheduled and recurring job sets
- And more...

### 4. Automatic Retry
Hangfire automatically retries failed jobs with exponential backoff. Default: 10 attempts.

## Packages Used

- **Hangfire.Core** (1.8.21): Core Hangfire library
- **Hangfire.AspNetCore** (1.8.21): ASP.NET Core integration
- **Hangfire.SqlServer** (1.8.21): SQL Server storage provider
- **Microsoft.Data.SqlClient** (6.1.2): SQL Server data provider (required by Hangfire.SqlServer)

## Troubleshooting

### SSL Certificate Error
If you see an error like "The certificate chain was issued by an authority that is not trusted":
- Add `TrustServerCertificate=True` to your connection string (already included in the template)
- This is safe for LocalDB development environments
- For production, use proper SSL certificates

### Database Connection Issues
- Ensure SQL Server LocalDB is installed
- Verify the connection string in `appsettings.json`
- Check that the database is accessible

### Jobs Not Executing
- Check the Hangfire Dashboard for error messages
- Review application logs for exceptions
- Ensure Hangfire Server is running (check console output)

### Dashboard Not Accessible
- Verify the application is running
- Check that `/hangfire` path is accessible
- Review middleware configuration in `Program.cs`

## Learning Resources

- [Hangfire Official Documentation](https://docs.hangfire.io/)
- [Background Jobs in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services)
- [Cron Expression Generator](https://crontab.guru/)

## Notes

- The project uses SQL Server LocalDB by default, which is suitable for development
- For production, use a dedicated SQL Server instance
- Consider securing the Hangfire Dashboard with authentication
- Adjust recurring job schedules based on your needs
- Monitor database size as job history accumulates

## Next Steps

To extend this demo:
1. Add authentication to the Hangfire Dashboard
2. Implement custom job filters
3. Add email notifications on job failures
4. Create more complex job workflows
5. Add job parameter validation
6. Implement job cancellation tokens

---

**Created with ASP.NET Core 8 and Hangfire 1.8.21**
