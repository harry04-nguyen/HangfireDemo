# Hangfire Demo - Troubleshooting Guide

## Common Issues and Solutions

### 1. SSL Certificate Error

**Error Message:**
```
A connection was successfully established with the server, but then an error occurred during the login process. 
(provider: SSL Provider, error: 0 - The certificate chain was issued by an authority that is not trusted.)
```

**Solution:**
Add `TrustServerCertificate=True` to your connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "HangfireConnection": "Server=(localdb)\\mssqllocaldb;Database=HangfireDemo;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

**Why:** LocalDB uses self-signed certificates that aren't trusted by default in newer versions of Microsoft.Data.SqlClient.

---

### 2. Missing Microsoft.Data.SqlClient Package

**Error Message:**
```
Please add a NuGet package reference to either 'Microsoft.Data.SqlClient' or 'System.Data.SqlClient' in your application project.
```

**Solution:**
```bash
dotnet add package Microsoft.Data.SqlClient
```

**Why:** Hangfire.SqlServer requires a SQL client provider but doesn't include one to let you choose which to use.

---

### 3. SQL Server LocalDB Not Installed

**Error Message:**
```
A network-related or instance-specific error occurred while establishing a connection to SQL Server.
```

**Solution:**
1. Install SQL Server LocalDB:
   - Download from: https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb
   - Or install with Visual Studio (select "Data storage and processing" workload)

2. Verify installation:
 ```bash
   sqllocaldb info
   ```

3. Start LocalDB if needed:
   ```bash
   sqllocaldb start mssqllocaldb
   ```

---

### 4. Database Permission Issues

**Error Message:**
```
Cannot open database "HangfireDemo" requested by the login. The login failed.
```

**Solution:**
- Ensure Windows Authentication is enabled (default with LocalDB)
- Check that your Windows user has access to LocalDB
- Try running Visual Studio or terminal as Administrator

---

### 5. Port Already in Use

**Error Message:**
```
Failed to bind to address https://127.0.0.1:5001: address already in use.
```

**Solution:**
Option 1 - Use a different port:
```bash
dotnet run --urls "https://localhost:5002;http://localhost:5003"
```

Option 2 - Stop the process using the port:
```bash
# Find the process
netstat -ano | findstr :5001

# Kill the process (replace PID with actual process ID)
taskkill /PID <PID> /F
```

---

### 6. Jobs Not Executing

**Symptoms:**
- Jobs appear in dashboard but stay in "Enqueued" state
- No logs appearing in console

**Solutions:**

1. **Check Hangfire Server is running:**
   Look for this message in console output:
   ```
   Hangfire Server started.
   ```

2. **Verify service registration:**
   In `Program.cs`, ensure you have:
   ```csharp
   builder.Services.AddHangfireServer();
   ```

3. **Check for exceptions:**
   Review console output for any stack traces

4. **Verify job service is registered:**
   ```csharp
   builder.Services.AddScoped<IJobService, JobService>();
   ```

---

### 7. Dashboard Shows 404 Error

**Error:** Dashboard at `/hangfire` returns 404

**Solutions:**

1. **Verify middleware order:**
   In `Program.cs`, ensure:
   ```csharp
   app.UseRouting();
   app.UseAuthorization();
   app.UseHangfireDashboard("/hangfire");
   ```

2. **Check URL:**
   - Use: `https://localhost:5001/hangfire` (with 's' in https)
   - Not: `http://localhost:5001/hangfire`

3. **Clear browser cache:**
   Try incognito/private browsing mode

---

### 8. Recurring Jobs Not Running

**Symptoms:**
- Recurring jobs appear in dashboard but don't execute

**Solutions:**

1. **Check Cron expression:**
   ```csharp
   // Wrong - string instead of Cron helper
   RecurringJob.AddOrUpdate<IJobService>("id", x => x.SimpleJob(""), "* * * * *");
   
   // Correct - using Cron helper
   RecurringJob.AddOrUpdate<IJobService>("id", x => x.SimpleJob(""), Cron.Minutely);
   ```

2. **Trigger manually:**
   Go to Dashboard ? Recurring Jobs ? Click "Trigger now"

3. **Check timezone:**
 Recurring jobs use UTC by default

---

### 9. "Table already exists" Error

**Error Message:**
```
There is already an object named 'Job' in the database.
```

**Solution:**
This is usually harmless on first run. Hangfire checks if tables exist before creating them.

If it persists:
```sql
-- Connect to database and drop Hangfire schema
DROP SCHEMA HangFire;
```

Then restart the application.

---

### 10. High Memory Usage

**Symptoms:**
- Application using excessive memory
- Performance degradation over time

**Solutions:**

1. **Limit job retention:**
   ```csharp
   builder.Services.AddHangfire(config => config
  .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
       {
           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
     SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
           QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
           DisableGlobalLocks = true,
           // Add this to auto-delete old jobs
     JobExpirationCheckInterval = TimeSpan.FromHours(1)
       }));
   ```

2. **Manually clean old jobs:**
   Dashboard ? Succeeded jobs ? Select all ? Delete

3. **Configure automatic cleanup:**
   ```csharp
   GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3 });
   ```

---

### 11. Jobs Failing with Dependency Injection Error

**Error Message:**
```
Unable to resolve service for type 'IJobService'
```

**Solution:**
Ensure service lifetime is appropriate:
```csharp
// Use Scoped for services that use DbContext or HttpContext
builder.Services.AddScoped<IJobService, JobService>();

// Use Transient for stateless services
builder.Services.AddTransient<IJobService, JobService>();

// Avoid Singleton for services with dependencies on scoped services
```

---

## Getting Help

If you encounter an issue not listed here:

1. **Check Hangfire logs in console**
2. **Review Hangfire Dashboard** ? Failed jobs ? Click on job ? View exception details
3. **Enable detailed logging** in `appsettings.json`:
   ```json
   "Logging": {
     "LogLevel": {
       "Default": "Debug",
 "Hangfire": "Debug"
     }
   }
   ```
4. **Check official docs:** https://docs.hangfire.io/
5. **Search GitHub issues:** https://github.com/HangfireIO/Hangfire/issues

---

## Quick Health Check

Run these checks to verify everything is working:

? **Database Connection:**
```bash
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT @@VERSION"
```

? **Application builds:**
```bash
dotnet build
```

? **Packages restored:**
```bash
dotnet restore
```

? **Application runs:**
```bash
dotnet run
```

? **Dashboard accessible:**
Open browser ? https://localhost:5001/hangfire

? **Hangfire Server running:**
Check console output for "Hangfire Server started"

---

**Last Updated:** 2025
**Hangfire Version:** 1.8.21
**.NET Version:** 8.0
