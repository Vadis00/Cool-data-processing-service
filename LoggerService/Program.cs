using LoggerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>()
                .AddScoped<Cool_data_processing_service.Service.LoggerService>();
    })
    .Build();

await host.RunAsync();
