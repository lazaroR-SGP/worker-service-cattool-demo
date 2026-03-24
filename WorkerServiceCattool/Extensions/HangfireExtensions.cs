// //-----------------------------------------------------------------------
// // <copyright file="HangfireExtensions.cs" company="Symco Group Panama">
// //    Copyright 2026 (c) Symco Group Panama. All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------

namespace WorkerServiceCattool.Extensions;

using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection;

public static class HangfireExtensions
{
    public static IHostApplicationBuilder AddHangfirePipeline(
        this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration
                                      .GetConnectionString("HangfireConnection");


        builder.Services.AddHangfire(cfg => cfg
                                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                            .UseSimpleAssemblyNameTypeSerializer()
                                            .UseRecommendedSerializerSettings()
                                            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                                            {
                                                CommandBatchMaxTimeout      = TimeSpan.FromMinutes(5),
                                                SlidingInvisibilityTimeout  = TimeSpan.FromMinutes(5),
                                                QueuePollInterval           = TimeSpan.FromSeconds(15),
                                                UseRecommendedIsolationLevel = true,
                                                DisableGlobalLocks          = true
                                            }));

        // Servidor del orquestador — baja concurrencia, alta prioridad
        builder.Services.AddHangfireServer(opts =>
        {
            opts.ServerName  = $"orchestrator@{Environment.MachineName}";
            opts.WorkerCount = 2;
            opts.Queues      = ["orchestrator"];
        });

        // Servidor de workers — mayor concurrencia
        builder.Services.AddHangfireServer(opts =>
        {
            opts.ServerName  = $"workers@{Environment.MachineName}";
            opts.WorkerCount = 5;
            opts.Queues      = ["workers", "default"];
        });

        return builder;
    }
}
