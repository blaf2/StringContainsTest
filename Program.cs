

using StringContainsTest;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            Console.WriteLine("Please specify a txt file");
            return;
        }

        var fileContents = File.ReadAllText(args[0]);

        var allItems = new List<TestItem>();

        foreach (var text in fileContents.Split(", "))
        {
            allItems.Add(new TestItem { Id = Guid.NewGuid(), Name = text });
        }

        var lookup = new StringContainsHelper<TestItem>(allItems);

        string? search = null;
        while (search == null) { 
            Console.WriteLine("Please specify a search phrase:");
            search = Console.ReadLine();
        }

        var results = lookup.ContainsSearch(search);
        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
        Console.ReadLine();
    }
}