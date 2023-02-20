
namespace Cool_data_processing_service.Service
{
    public class CommandHandlerService
    {
        private readonly Worker _worker;
        public CommandHandlerService(Worker worker)
        {
            _worker = worker;
        }
        public async Task Worker()
        {
            string command;

            Console.WriteLine("Commands:");
            Console.WriteLine("start - Start listening to directories");
            Console.WriteLine("stop - Stop listening to the directory");
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
                        string msg;
                        if(!_worker.ConfigurationСheck(out msg))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(msg);
                            Console.ResetColor();
                        }
                        else
                        {
                            _worker.Start();
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Listening to updates...");
                            Console.ResetColor();
                        }
                        break;
                    case "stop":
                         _worker.Stop();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Listening is stopped!");
                        Console.ResetColor();
                        break;
                    case "exit":
                        _worker.Stop();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Listening is stopped!");
                        Console.ResetColor();
                        await Task.Delay(3000);
                        return;
                }
            }
        }
    }
}
