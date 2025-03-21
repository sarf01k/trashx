using System.CommandLine;

namespace Trashx
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var extensionsOption = new Option<string>(
                ["-e", "--extensions"],
                "The file extension(s) to be deleted.");

            var directoryArgument = new Argument<DirectoryInfo>(
                "directory",
                description: "The directory where files should be deleted.")
            {
                Arity = ArgumentArity.ExactlyOne
            };

            var recursiveOption = new Option<bool>(
                ["-r", "--recursive"],
                "Include subdirectories when deleting files.");

            var rootCommand = new RootCommand("CLI tool to delete files by extension")
            {
                extensionsOption,
                recursiveOption,
                directoryArgument
            };
            rootCommand.Name = "trashx";

            rootCommand.SetHandler((string extensions, bool recursive, DirectoryInfo directory) =>
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
                                                .Select(e => e.Trim().TrimStart('.'))
                                                .Where(e => !string.IsNullOrEmpty(e))
                                                .ToList();

                Console.WriteLine("\nFILE".PadRight(100) + "SIZE");
                Console.WriteLine("----".PadRight(100) + "----");

                var filesToDelete = GetFilesToDelete(extensionsList, directory, recursive);

                if (filesToDelete.Count == 0)
                {
                    Console.WriteLine("\n⚠ No files with the specified extensions were found.");
                    return;
                }

                foreach (var file in filesToDelete)
                {
                    string fileName = file.FullName.PadRight(100);
                    double fileSize = file.Length;
                    Console.WriteLine(fileName + ConvertSizeFromBytes(fileSize));
                }

                Console.Write("\nProceed to delete files? [y / n]: ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (input == "y")
                {
                    foreach (var file in filesToDelete)
                    {
                        file.Delete();
                    }
                    Console.WriteLine("\n🗑️ Files deleted successfully.");
                }
                else
                {
                    Console.WriteLine("\n❌ Deletion cancelled.");
                }
            }, extensionsOption, recursiveOption, directoryArgument);

            return await rootCommand.InvokeAsync(args);
        }

        static List<FileInfo> GetFilesToDelete(List<string> extensionsList, DirectoryInfo directory, bool recursive)
        {
            var filesToDelete = new List<FileInfo>();

            foreach (var file in directory.EnumerateFiles())
            {
                string extension = Path.GetExtension(file.Name).TrimStart('.');
                if (extensionsList.Contains(extension))
                {
                    filesToDelete.Add(file);
                }
            }

            if (recursive)
            {
                foreach (var subDir in directory.EnumerateDirectories())
                {
                    filesToDelete.AddRange(GetFilesToDelete(extensionsList, subDir, recursive));
                }
            }

            return filesToDelete;
        }

        static string ConvertSizeFromBytes(double sizeInBytes)
        {
            double sizeKB = sizeInBytes / 1024.0;
            double sizeMB = sizeKB / 1024.0;
            double sizeGB = sizeMB / 1024.0;

            if (sizeGB >= 1)
                return $"{sizeGB:F2} GB";
            else if (sizeMB >= 1)
                return $"{sizeMB:F2} MB";
            else if (sizeKB >= 1)
                return $"{sizeKB:F2} KB";
            else
                return $"{sizeInBytes} bytes";
        }
    }
}