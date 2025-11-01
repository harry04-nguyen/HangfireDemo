using Hangfire;
using Hangfire.SqlServer;
using HangfireDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Job Service
builder.Services.AddScoped<IJobService, JobService>();

// Configure Hangfire with SQL Server storage for persistence
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
 .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
  QueuePollInterval = TimeSpan.Zero,
  UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Add Hangfire server
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
 app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Add Hangfire Dashboard (accessible at /hangfire)
app.UseHangfireDashboard("/hangfire");

// Configure recurring jobs - these will persist in the database
RecurringJob.AddOrUpdate<IJobService>(
    "simple-recurring-job",
    service => service.SimpleJob("This is a recurring job!"),
    Cron.Minutely); // Runs every minute

RecurringJob.AddOrUpdate<IJobService>(
    "data-processing-job",
    service => service.DataProcessingJob("Scheduled data for processing"),
    Cron.Hourly); // Runs every hour

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
