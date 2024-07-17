using System.Net.Http.Json;

using HttpClient hc = new() { BaseAddress = new("https://localhost:7168") };
while (true)
{
    Console.Write("Question: ");

    await foreach (var msg in hc.GetFromJsonAsAsyncEnumerable<string>($"/copilot?question={Console.ReadLine()}"))
        Console.Write(msg);

    Console.WriteLine();
}