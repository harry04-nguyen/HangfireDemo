# Quick Start Guide

## Run the Project

1. Open terminal in project directory
2. Run: `dotnet run`
3. Open browser: `https://localhost:5001` (check console for actual port)

## Access Points

- **Main App**: `https://localhost:5001/`
- **Hangfire Dashboard**: `https://localhost:5001/hangfire`

## Quick Test

1. Click "Enqueue Simple Job" on the home page
2. Open Hangfire Dashboard
3. Watch the job execute in real-time
4. Try other job types!

## What to Watch

### In Hangfire Dashboard:
- **Jobs** tab: See all job executions
- **Recurring Jobs** tab: View scheduled recurring jobs
- **Servers** tab: Monitor Hangfire server status
- **Retries** tab: See failed jobs that will retry

### In Console:
- Watch log messages as jobs execute
- See retry attempts for failing jobs
- Monitor job completion times

## Test Scenarios

### Fire-and-Forget
1. Click "Enqueue Simple Job"
2. See immediate execution in dashboard

### Retry Mechanism
1. Click "Enqueue Retry Job"
2. Watch it fail twice in the dashboard
3. See it succeed on third attempt

### Delayed Job
1. Click "Schedule Delayed Job"
2. Check "Scheduled" in dashboard
3. Wait 2 minutes to see it execute

### Recurring Jobs
1. Go to Hangfire Dashboard ? Recurring Jobs
2. See "simple-recurring-job" running every minute
3. See "data-processing-job" scheduled hourly

### Job Persistence
1. Enqueue several jobs
2. Stop the application (Ctrl+C)
3. Restart the application
4. Check dashboard - job history is preserved!

## Common Commands

```bash
# Run the project
dotnet run

# Build the project
dotnet build

# Clean build artifacts
dotnet clean

# Restore packages
dotnet restore
```

## Troubleshooting

**Can't connect to database?**
- The database is created automatically
- Check connection string in appsettings.json
- Ensure `TrustServerCertificate=True` is in the connection string (required for LocalDB)

**Jobs not executing?**
- Check console for errors
- Verify Hangfire server is started (look for "Hangfire Server started" message)

**Dashboard not loading?**
- Ensure app is running
- Try: `https://localhost:5001/hangfire` (note the 's' in https)

**SSL Certificate errors?**
- This is already fixed in appsettings.json with `TrustServerCertificate=True`
- If you still see errors, verify the connection string

## Next Steps

1. Explore all job types on the home page
2. Monitor jobs in the Hangfire Dashboard
3. Review code in `Services/JobService.cs`
4. Check `Program.cs` for Hangfire configuration
5. Review `Controllers/HomeController.cs` for job enqueueing

---

Enjoy exploring Hangfire! ??
