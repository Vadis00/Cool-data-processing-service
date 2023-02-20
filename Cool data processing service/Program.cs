using Cool_data_processing_service;
using Cool_data_processing_service.BackgroundService;
using Cool_data_processing_service.Service;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    static void Main(string[] args)
    {
        //setup our DI
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<FileProcessingService>()
            .AddSingleton<FileWatcherService>()
            .AddSingleton<FileSystemWatcher>()
            .AddSingleton<CommandHandlerService>()
            .AddScoped<LoggerService>()
            .AddScoped<DataService>()
            .AddScoped<LogBackgroundService>()
            .AddScoped<Worker>()
            .BuildServiceProvider();


        //do the actual work here
        var commandHandler = serviceProvider.GetService<CommandHandlerService>();
        commandHandler?.Worker();

    }
}