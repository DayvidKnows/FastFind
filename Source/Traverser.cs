using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FastFind
{
    internal class Traverser
    {
        public EventHandler OnFileFound;
        public EventHandler OnDirectoryFound;

        private readonly List<string> fileExtensions = new List<string>();

        public Traverser()
        {
        }

        public Traverser(params string[] fileExtensions)
        {
            this.fileExtensions = fileExtensions.Select(ext => string.Concat(".", ext)).ToList();
        }

        public void Traverse(string path)
        {
            FindDirectoriesAndFiles(path);
        }

        private void FindDirectoriesAndFiles(string path)
        {
            if (!Directory.Exists(path))
            {
                // maybe logging
                return;
            }

            var eventArgs = new DirectoryFoundEventArgs
            {
                Path = path
            };

            OnDirectoryFound?.Invoke(this, eventArgs);

            var directoryPaths = Directory.EnumerateDirectories(path);

            Parallel.ForEach(directoryPaths, directoryPath =>
            {
                FindDirectoriesAndFiles(directoryPath);
            });

            FindFiles(path);
        }

        private void FindFiles(string path)
        {
            var filePaths = Directory.EnumerateFiles(path);

            Parallel.ForEach(filePaths, filePath =>
            {
                FileFound(filePath);
            });
        }

        private void FileFound(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath);
            if (fileExtensions.Count > 0 && !fileExtensions.Contains(fileExtension))
            {
                return;
            }

            var eventArgs = new FileFoundEventArgs
            {
                Path = filePath
            };

            OnFileFound?.Invoke(this, eventArgs);
        }
    }

    internal class FileFoundEventArgs : EventArgs
    {
        public string Path { get; set; }
    }

    internal class DirectoryFoundEventArgs : EventArgs
    {
        public string Path { get; set; }
    }
}