using Cool_data_processing_service.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

public class Program
{
    static void Main()
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
            .BuildServiceProvider();


        //do the actual work here
        var commandHandler = serviceProvider.GetService<CommandHandlerService>();
        commandHandler?.Worker();

    }
}