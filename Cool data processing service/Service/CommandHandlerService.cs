
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
                         _worker.Start();
                        break;
                    case "stop":
                         _worker.Start();
                        break;
                    case "exit":
                        return;
                }
            }
        }
    }
}
