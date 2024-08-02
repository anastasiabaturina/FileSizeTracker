using System.Collections.Concurrent;

namespace FileSizeTracker;

public class Program()
{
    public static void Main()
    {
        Console.Write("Enter the initial directory: ");
        var directoryName = Console.ReadLine();

        Console.Write("Enter the file format: ");
        var FormatFile = Console.ReadLine();

        var maxDegreeOfParallelism = int.Parse(Environment.GetEnvironmentVariable("MAX_THREADS", EnvironmentVariableTarget.User));

        var fileSizes = new ConcurrentBag<long>();
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        FileSizeCalculate.FindFileSizeAsync(directoryName, FormatFile, fileSizes, maxDegreeOfParallelism, token);
    }
}