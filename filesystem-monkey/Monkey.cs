using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace filesystem_monkey
{
    /// <summary>
    /// Run monkey tests within a folder.
    /// </summary>
    class Monkey
    {
        private FileSystem fs;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Monkey(string root)
        {
            this.fs = new FileSystem(root);
        }
        

        /// <summary>
        /// Run the monkey.
        /// </summary>
        public void run()
        {            Random random = new Random();

            int targetFilesNumber = 10;
            int targetFoldersNumber = 10;            while (true)
            {
                try
                {
                    if (random.Next(100) < 3 * (3 + targetFilesNumber - fs.NumberOfFiles()))
                    {
                        fs.CreateFile(30);
                    }
                    if (random.Next(100) < 3 * (3 + targetFoldersNumber - fs.NumberOfFolders()))
                    {
                        fs.CreateFolder();
                    }
                    if (random.Next(100) < 3 * (3 + fs.NumberOfFiles() - targetFilesNumber))
                    {
                        fs.DeleteFile();
                    }
                    if (random.Next(100) < 3 * (3 + fs.NumberOfFolders() - targetFoldersNumber))
                    {
                        fs.DeleteFolder();
                    }
                    if (random.Next(100) < 10)
                    {
                        fs.MoveFile();
                    }
                    if (random.Next(100) < 10)
                    {
                        fs.MoveFolder();
                    }
                    if (random.Next(100) < 10)
                    {
                        fs.RenameFile();
                    }
                    if (random.Next(100) < 10)
                    {
                        fs.RenameFolder();
                    }

                    Console.WriteLine("Files: " + fs.NumberOfFiles() + " , Folders: " + fs.NumberOfFolders());

                    // Wait.
                    System.Threading.Thread.Sleep(random.Next(10*1000)); // Max 10 seconds.
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception on testing side, ignoring " + e);
                }
            }
        }
    }
}
