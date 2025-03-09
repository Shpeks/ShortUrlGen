using Base62;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using ShortUrlGen;
using ShortUrlGen.Data;
using ShortUrlGen.Interfaces;
using ShortUrlGen.Repository;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
var conf = builder.Configuration;

// Add services to the container.
service.AddEndpointsApiExplorer();
service.AddSwaggerGen();
service.AddControllers();
service.AddSingleton<Base62Converter>();

service.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(conf.GetConnectionString("DefaultConnection")));

service.AddScoped<IMappingRepository, MappingRepository>();

service.AddTransient<RemoveShortLinkJob>();

service.AddQuartz(q => 
{
    var jobkey = new JobKey("SampleJob");

    q.AddJob<RemoveShortLinkJob>(opts => opts.WithIdentity(jobkey));
    q.AddTrigger(opts => opts
        .ForJob(jobkey)
        .WithIdentity("SampleJob-trigger")
        .WithSimpleSchedule(x => x          //.WithCronSchedule("0 0/1 * * * ?")); 60 sec~
            .WithIntervalInSeconds(60)
            .RepeatForever()));
});

service.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

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
