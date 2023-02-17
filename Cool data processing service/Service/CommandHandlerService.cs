
namespace Cool_data_processing_service.Service
{
    public class CommandHandlerService
    {
        private readonly FileWatcherService _fileWatcherService;
        public CommandHandlerService(FileWatcherService fileWatcherService)
        {
            _fileWatcherService = fileWatcherService;
        }
        public void Worker()
        {
            string command;

            Console.WriteLine("Commands:");
            Console.WriteLine("start - Start listening to directories");
            Console.WriteLine("start - Stop listening to the directory");
            Console.WriteLine("exit - Finish and exit");
            Console.WriteLine();
            Console.WriteLine("Enter the command..");

            while (true)
            {
                Console.Write("Command: ");
                command = Console.ReadLine().ToLower();

                switch (command)
                {
                    case "start":
                        _fileWatcherService.ListenUpdate();
                        break;
                    case "stop":
                        _fileWatcherService.Stoplistening();
                        break;
                    case "exit":
                        return;
                }
            }
        }
    }
}
