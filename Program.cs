using System;
using System.Diagnostics;
using System.Threading.Tasks;
class Program
{
    static async Task Main()
    {
        Console.WriteLine("Введите путь к папке с файлами, в которых нужно посчитать количество пробелов:");
        string filePath = Console.ReadLine();

        Stopwatch stopWatcher_SpacesCounter = new Stopwatch();
        stopWatcher_SpacesCounter.Start();

        int totalSpaces = await CountSpacesInFilesAsync(filePath);

        stopWatcher_SpacesCounter.Stop();
        Console.WriteLine("Общее количество пробелов со всех файлов - " + totalSpaces);
        Console.WriteLine("Время на подсчет пробелов - " + stopWatcher_SpacesCounter.Elapsed.ToString());
    }

    static async Task<int> CountSpacesInFilesAsync(string folderPath)
    {
        int totalSpaces = 0;
        //getting list of files from the selected folder
        string[] files = Directory.GetFiles(folderPath);
        //creating and run tasks for every file
        Task<int>[] tasks = new Task<int>[files.Length];
        for (int i = 0; i < files.Length; i++)
        { 
            tasks[i] = CountSpacesAsync(files[i]);
        }
        //waiting when all tasks will be finished and summarize counts of spaces
        await Task.WhenAll(tasks);
        foreach (var task in tasks)
        {
            totalSpaces += await task;
        }
        return totalSpaces;
    }

    static async Task<int> CountSpacesAsync(string filePath)
    {
        int spaces = 0;
        //reading the content of file async
        using (StreamReader reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                spaces += line.Split(' ').Length - 1;
            }
        }
        return spaces;
    }
}

