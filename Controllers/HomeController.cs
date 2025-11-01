using System.Diagnostics;
using Hangfire;
using HangfireDemo.Models;
using HangfireDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
  public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;

  public HomeController(ILogger<HomeController> logger, IBackgroundJobClient backgroundJobClient)
 {
          _logger = logger;
         _backgroundJobClient = backgroundJobClient;
        }

        public IActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            return View();
     }

        // Fire-and-forget job
        [HttpPost]
        public IActionResult EnqueueSimpleJob()
        {
   var jobId = _backgroundJobClient.Enqueue<IJobService>(x => x.SimpleJob("Fire-and-forget job triggered!"));
          TempData["Message"] = $"Simple job enqueued with ID: {jobId}";
            return RedirectToAction("Index");
     }

   // Job with retry mechanism - demonstrates automatic retry
     [HttpPost]
  public IActionResult EnqueueJobWithRetry()
    {
        var jobId = _backgroundJobClient.Enqueue<IJobService>(x => x.JobWithRetry(1));
        TempData["Message"] = $"Job with retry enqueued with ID: {jobId}. This will fail initially and retry.";
return RedirectToAction("Index");
      }

     // Delayed job - executes after a specified delay
        [HttpPost]
        public IActionResult EnqueueDelayedJob()
        {
    var jobId = _backgroundJobClient.Schedule<IJobService>(
            x => x.SimpleJob("This is a delayed job!"),
        TimeSpan.FromMinutes(2));
   TempData["Message"] = $"Delayed job scheduled with ID: {jobId}. Will execute in 2 minutes.";
            return RedirectToAction("Index");
        }

        // Long-running job
 [HttpPost]
        public IActionResult EnqueueLongRunningJob()
        {
      var jobId = _backgroundJobClient.Enqueue<IJobService>(x => x.LongRunningJob(10));
       TempData["Message"] = $"Long-running job enqueued with ID: {jobId}. Will run for 10 seconds.";
            return RedirectToAction("Index");
        }

        // Data processing job - demonstrates persistence
        [HttpPost]
  public IActionResult EnqueueDataProcessingJob(string data)
        {
         if (string.IsNullOrWhiteSpace(data))
                data = "Sample data for processing";

    var jobId = _backgroundJobClient.Enqueue<IJobService>(x => x.DataProcessingJob(data));
            TempData["Message"] = $"Data processing job enqueued with ID: {jobId}";
            return RedirectToAction("Index");
        }

        // Continuation job - executes after parent job completes
        [HttpPost]
        public IActionResult EnqueueContinuationJob()
        {
        var parentJobId = _backgroundJobClient.Enqueue<IJobService>(
           x => x.SimpleJob("Parent job"));

            var continuationJobId = _backgroundJobClient.ContinueJobWith<IJobService>(
          parentJobId,
      x => x.SendEmailJob("admin@example.com", "Parent job completed!"));

            TempData["Message"] = $"Parent job ID: {parentJobId}, Continuation job ID: {continuationJobId}";
          return RedirectToAction("Index");
 }

      // Batch jobs - multiple jobs at once
 [HttpPost]
        public IActionResult EnqueueBatchJobs()
        {
      var jobIds = new List<string>();
   
  for (int i = 1; i <= 5; i++)
            {
         var jobId = _backgroundJobClient.Enqueue<IJobService>(
         x => x.SimpleJob($"Batch job #{i}"));
     jobIds.Add(jobId);
         }

    TempData["Message"] = $"Enqueued {jobIds.Count} batch jobs. Check dashboard for details.";
      return RedirectToAction("Index");
      }

   public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
   {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
