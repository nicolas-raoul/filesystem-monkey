using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace filesystem_monkey
{
    /// <summary>
    /// Run monkey tests within a folder.
    /// </summary>
    class FilesystemMonkey
    {
        /// <summary>
        /// Folder within which to perform filesystem operations.
        /// </summary>
        private String root;

        /// <summary>
        /// Incremented unique identifier for files and folders.
        /// </summary>
        private int id = 1;

        private Random random = new Random();


        /// <summary>
        /// Constructor.
        /// </summary>
        public FilesystemMonkey(string root)
        {
            this.root = root;
        }
        

        /// <summary>
        /// Run the monkey.
        /// </summary>
        public void run()
        {
            while (true)
            {
                try
                {
                    if (random.Next(100) < 10)
                    {
                        // Create file.
                        CreateFile(root, 30);
                    }
                    if (random.Next(100) < 10)
                    {
                        // Create directory.
                        ++ id;
                        string path1 = Path.Combine(root, "dir" + id);
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(path1);
                        }
                    }
                    if (random.Next(100) < 10)
                    {
                        // Delete file.
                        string file = RandomFile();
                        if (file != null)
                        {
                            File.Delete(Path.Combine(root, file));
                        }
                    }
                    if (random.Next(100) < 10)
                    {
                        // Delete directory.
                        string folder = RandomFolder();
                        if (folder != null)
                        {
                            Directory.Delete(Path.Combine(root, folder), true); // Recursive.
                        }
                    }
                    if (random.Next(100) < 10)
                    {
                        // Move file.
                        string file = RandomFile();
                        string destination = RandomFolder();
                        if (file != null && destination != null)
                        {
                            String destinationFile = Path.Combine(destination, Path.GetFileName(file));
                            File.Move(file, destinationFile);
                        }
                    }
                    if (random.Next(100) < 10)
                    {
                        // Move folder.
                        string folder = RandomFolder();
                        string destination = RandomFolder();
                        if (folder != null && destination != null)
                        {
                            String destinationFolder = Path.Combine(destination, Path.GetFileName(folder));
                            Directory.Move(folder, destinationFolder);
                        }
                    }
                    // TODO Renames.

                    // Show statistics.
                    int numberOfFiles = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories).Length;
                    int numberOfFolders = Directory.GetDirectories(root, "*", SearchOption.AllDirectories).Length;
                    Console.WriteLine("Files: " + numberOfFiles + " , Folders: " + numberOfFolders);

                    // Wait.
                    System.Threading.Thread.Sleep(random.Next(20*1000)); // Max 20 seconds.
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception on testing side, ignoring " + e);
                }
            }
        }

        
        /// <summary>
        /// Get a random existing file.
        /// </summary>
        public string RandomFile()
        {
            string[] files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                return null;
            }
            else
            {
                string result =  files[random.Next(files.Length)];

                // Avoid the filesystem-monkey.exe executable.
                if (result.Equals(System.AppDomain.CurrentDomain.FriendlyName))
                {
                    return null;
                }
                else
                {
                    return result;
                }
            }
        }


        /// <summary>
        /// Get a random existing folder.
        /// </summary>
        public string RandomFolder()
        {
            string[] folders = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);

            if (folders.Length == 0)
            {
                return null;
            }
            else
            {
                return folders[random.Next(folders.Length)];
            }
        }


        /// <summary>
        /// Create a file in this folder with a new unique name and random content.
        /// </summary>
        public void CreateFile(string path, int maxSizeInKb)
        {
            string filename = GetNextFileName();
            int sizeInKb = 1 + random.Next(maxSizeInKb);
            ++ id;
            byte[] data = new byte[1024];

            try
            {
                using (FileStream stream = File.OpenWrite(Path.Combine(path, filename)))
                {
                    // Write random data
                    for (int i = 0; i < sizeInKb; i++)
                    {
                        random.NextBytes(data);
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception on testing side, ignoring " + ex);
            }
        }


        /// <summary>
        /// Get next available unique file name.
        /// </summary>
        public string GetNextFileName()
        {
            string filename = "file_" + id.ToString() + ".bin";
            return filename;
        }
    }
}
