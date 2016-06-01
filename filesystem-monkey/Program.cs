using System;
using System.IO;

namespace filesystem_monkey
{
    /// <summary>
    /// Run monkey tests within a folder.
    /// Command-line argument: Folder within which to perform filesystem operations.
    /// Author: Nicolas Raoul
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Find out the folder.
            String folder;
            if (args.Length > 0)
            {
                folder = args[0];
            }
            else
            {
                Console.WriteLine("No folder given as command-line argument, so I will generate files in the current folder, is it OK? Press CTRL-C to exit.");
                Console.ReadKey();
                folder = Directory.GetCurrentDirectory();
            }

            // Run the monkey.
            new FilesystemMonkey(folder).run();
        }
    }
}
