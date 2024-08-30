using System.Collections.Concurrent;

namespace FileSizeTracker;

public class Program()
{
    public static async Task Main()
    {
        Console.Write("Enter the initial directory: ");
        var directoryName = Console.ReadLine();

        Console.Write("Enter the file format: ");
        var FormatFile = Console.ReadLine();

        var maxDegreeOfParallelism = int.Parse(Environment.GetEnvironmentVariable("MAX_THREADS", EnvironmentVariableTarget.User));

        var fileSizes = new ConcurrentBag<long>();
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        await FileSizeCalculate.FindFileSizeAsync(directoryName, FormatFile, fileSizes, maxDegreeOfParallelism, token);
    }
}