// //-----------------------------------------------------------------------
// // <copyright file="LoggingExtensions.cs" company="Symco Group Panama">
// //    Copyright 2026 (c) Symco Group Panama. All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------

namespace WorkerServiceCattool.Extensions;

using NLog.Extensions.Logging;

public static class LoggingExtensions
{
    public static IHostApplicationBuilder AddLogging(
        this IHostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddNLog(); // usa el LogManager ya configurado arriba
        return builder;
    }
}
