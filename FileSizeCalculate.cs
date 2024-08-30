using System.Collections.Concurrent;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
namespace FileSizeTracker;

public class FileSizeCalculate
{
    public static async Task FindFileSizeAsync(string directoryPath, string formatFile, ConcurrentBag<long> fileSizes, int maxDegreeOfParallelism, CancellationToken token)
    {
        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine($"The directory '{directoryPath}' does not exist.");
            return;
        }

        var files = Directory.GetFiles(directoryPath, $"*.{formatFile}", SearchOption.AllDirectories);

        if (files == null)
        {
            Console.WriteLine("There are no files with this format");
            return;
        }

        Parallel.ForEach(files, new ParallelOptions { 
            MaxDegreeOfParallelism = maxDegreeOfParallelism, 
            CancellationToken = token 
        }, file =>
        {
            var fileSize = new FileInfo(file).Length;
            fileSizes.Add(fileSize);
        });

        await using (var fstream = new FileStream("FileSize.txt", FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true))
        {
            var text = $"The size of all files: {fileSizes.Sum() / 1024} KB";
            var buffer = Encoding.Default.GetBytes(text);
            fstream.Write(buffer, 0, buffer.Length);
        }
    }
}