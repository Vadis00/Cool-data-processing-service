using System.IO;

public class Program
{
    static void Main()
    {
        var watcher = new FileSystemWatcher();
        watcher.Path = @"C:\Users\vgome\OneDrive\Рабочий стол\test";
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        watcher.Filter = "*.*";
        watcher.Changed += OnChanged;
        watcher.EnableRaisingEvents = true;

        Console.ReadLine();
    }

    private static void OnChanged(object source, FileSystemEventArgs e)
    {
        Console.WriteLine("ff");
        //Copies file to another directory.
    }

}