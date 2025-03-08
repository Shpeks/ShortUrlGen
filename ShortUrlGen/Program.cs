using Microsoft.EntityFrameworkCore;
using ShortUrlGen;
using Quartz;
using Quartz.Impl;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
var conf = builder.Configuration;

// Add services to the container.
service.AddEndpointsApiExplorer();
service.AddSwaggerGen();
service.AddControllers();

service.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(conf.GetConnectionString("DefaultConnection")));

service.AddQuartz();

service.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

// Options Quartz
var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
await scheduler.Start();

//  Options Job and Trigger
IJobDetail job = JobBuilder.Create<RemoveShortLinkJob>()
    .WithIdentity("RemoveShortLinkJob", "Group1")
    .Build();

ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("RemoveShortLinkTrigger", "Group1")
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(60)
        .RepeatForever())
    .Build();

// Start Job and Trigger
await scheduler.ScheduleJob(job, trigger);

// Endpoint
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers(); 
app.MapControllerRoute(
    name: "redirect",
    pattern: "{shortUrl}",
    defaults: new { controller = "Url", action = "RedirectToLongUrl" });
app.Run();
