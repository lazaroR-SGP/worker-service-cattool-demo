using NLog;
using WorkerServiceCattool;
using WorkerServiceCattool.Extensions;

var logger = LogManager.Setup()

                       //.SetupExtensions(ext => { ext.RegisterLayoutRenderer<SensitiveDataMaskingLayoutRenderer>("maskSensitive"); })
                       .LoadConfigurationFromFile("nlog.config")
                       .GetCurrentClassLogger();

logger.Debug("init main");

try
{
    HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder(args);
    var builder = hostApplicationBuilder;

    builder.AddLogging();

    //builder.AddPersistence();
    builder.AddHangfirePipeline();
    //builder.AddBatchPipeline();
    //builder.AddScheduler();
    //builder.AddObservability();

    builder.Services.AddHostedService<Worker>();

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    // NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
