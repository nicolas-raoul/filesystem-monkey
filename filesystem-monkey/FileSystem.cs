using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace filesystem_monkey
{
    /// <summary>
    /// Represents a filesystem.
    /// The current implementation should work on most desktop computer filesystems,
    /// but more exotic implementations such as FTP or CMIS should be possible.
    /// </summary>
    class FileSystem
    {
        /// <summary>
        /// Folder within which to perform filesystem operations.
        /// </summary>
        private String root;


        /// <summary>
        /// Source of randomness.
        /// </summary>
        private Random random = new Random();


        /// <summary>
        /// Constructor.
        /// </summary>
        public FileSystem(string root)
        {
            this.root = root;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////// Public methods /////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        
        /// <summary>
        /// Create a file in a random folder with a new unique name and random content.
        /// </summary>
        public void CreateFile(int maxSizeInKb)
        {
            string filename = RandomUnicodeString(30);
            string path = Path.Combine(root, filename);

            int sizeInKb = 1 + random.Next(maxSizeInKb);
            byte[] data = new byte[1024];

            try
            {
                using (FileStream stream = File.OpenWrite(path))
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
        /// Create a new folder within a random folder.
        /// </summary>
        public void CreateFolder()
        {
            string folderName = RandomUnicodeString(30);
            string path = Path.Combine(root, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


        /// <summary>
        /// Delete a random file.
        /// </summary>
        public void DeleteFile()
        {
            string file = RandomFile();
            if (file != null)
            {
                File.Delete(Path.Combine(root, file));
            }
        }


        /// <summary>
        /// Delete a random folder.
        /// </summary>
        public void DeleteFolder()
        {
            string folder = RandomFolder(false);
            if (folder != null)
            {
                Directory.Delete(Path.Combine(root, folder), true); // Recursive.
            }
        }


        /// <summary>
        /// Move a random file to a random folder.
        /// </summary>
        public void MoveFile()
        {
            string file = RandomFile();
            string destination = RandomFolder(true);
            if (file != null && destination != null)
            {
                String destinationFile = Path.Combine(destination, Path.GetFileName(file));
                File.Move(file, destinationFile);
            }
        }


        /// <summary>
        /// Move a random folder to within another random folder.
        /// </summary>
        public void MoveFolder()
        {
            string folder = RandomFolder(false);
            string destination = RandomFolder(true);
            if (folder != null && destination != null)
            {
                String destinationFolder = Path.Combine(destination, Path.GetFileName(folder));
                Directory.Move(folder, destinationFolder);
            }
        }


        /// <summary>
        /// Rename a random file with a random name.
        /// </summary>
        public void RenameFile()
        {
            string file = RandomFile();
            if (file != null)
            {
                string newName = RandomUnicodeString(30);
                String destinationFile = Path.Combine(Path.GetDirectoryName(file), newName);
                File.Move(file, destinationFile);
            }
        }


        /// <summary>
        /// Rename a random folder with a random name.
        /// </summary>
        public void RenameFolder()
        {
            string folder = RandomFolder(false);
            if (folder != null)
            {
                string newName = RandomUnicodeString(30);
                String destinationFolder = Path.Combine(Path.GetDirectoryName(folder), newName);
                File.Move(folder, destinationFolder);
            }
        }


        /// <summary>
        /// Number of files.
        /// </summary>
        public int NumberOfFiles()
        {
            return Directory.GetFiles(root, "*.*", SearchOption.AllDirectories).Length;
        }


        /// <summary>
        /// Number of folders.
        /// </summary>
        public int NumberOfFolders()
        {
            return Directory.GetDirectories(root, "*", SearchOption.AllDirectories).Length;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////// Private methods ////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Get a random existing file.
        /// </summary>
        private string RandomFile()
        {
            string[] files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                return null;
            }
            else
            {
                string result = files[random.Next(files.Length)];

                // Avoid the filesystem-monkey.exe executable.
                if (Path.GetFileName(result).Equals(System.AppDomain.CurrentDomain.FriendlyName))
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
        /// <param name="includeRoot">Whether to include or not the root as a possible random folder.</param>
        private string RandomFolder(bool includeRoot)
        {
            string[] folders = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);

            if (folders.Length == 0)
            {
                return null;
            }
            else
            {
                if (includeRoot && random.Next(folders.Length) == 0)
                {
                    return root;
                }
                return folders[random.Next(folders.Length)];
            }
        }


        /// <summary>
        /// Generate a random string.
        /// It might not be a valid filename. But this is monkey testing after all.
        /// </summary>
        private string RandomUnicodeString(int length)
        {
            var stringBuilder = new StringBuilder();
            for (int j = 0; j < length; ++j)
            {
                char character = (char)random.Next(char.MinValue, char.MaxValue);
                
                // TODO Unicode surrogate characters, are not valid on their own,
                // they must appear as a pair (0xD800-0xDBFF first, 0xDC00-0xDFFF second)

                stringBuilder.Append(character);
            }
            byte[] bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
            return Encoding.Unicode.GetString(bytes);
        }
    }
}
