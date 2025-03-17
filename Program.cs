using System.CommandLine;

namespace Trashx
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var directoryArgument = new Argument<DirectoryInfo>(
                "directory",
                description: "The directory where files should be deleted.")
            {
                Arity = ArgumentArity.ExactlyOne
            };

            var extensionsOption = new Option<string>(
                ["-e", "--extensions"],
                "The file extension(s) to be deleted.");

            var rootCommand = new RootCommand("CLI tool to delete files by extension")
            {
                directoryArgument,
                extensionsOption
            };

            rootCommand.SetHandler((directory, extensions) =>
            {
                if (!directory.Exists)
                {
                    Console.WriteLine("Error: The specified directory does not exist.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(extensions))
                {
                    Console.WriteLine("Error: No file extensions provided.");
                    return;
                }

                var extensionsList = extensions.Split(',')
                                                .Select(e => e.Trim())
                                                .Where(e => !string.IsNullOrEmpty(e))
                                                .ToList();

                int deletedCount = 0;
                foreach (var ext in extensionsList)
                {
                    var files = directory.GetFiles($"*.{ext.TrimStart('.')}");

                    foreach (var file in files)
                    {
                        System.Console.WriteLine(file.Length);
                        file.Delete();
                        Console.WriteLine($"Deleted: {file.FullName}");
                        deletedCount++;
                    }
                }

                Console.WriteLine($"\nTotal files deleted: {deletedCount}");
            }, directoryArgument, extensionsOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}