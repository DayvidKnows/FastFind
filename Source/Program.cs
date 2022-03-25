using System;
using System.Threading;

namespace FastFind
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            const string path = @"C:\Users\Daniel\Desktop\gecko-dev";

            int fileCount = 0;
            int directoryCount = 0;

            var traverser = new Traverser("js");

            traverser.OnFileFound += (s, o) => Interlocked.Increment(ref fileCount);
            traverser.OnDirectoryFound += (s, o) => Interlocked.Increment(ref directoryCount);

            traverser.Traverse(path);

            Console.WriteLine($"FileCount: {fileCount} | DirectoryCount: {directoryCount}");

            // setup directory traverser
            // setup queue
            // setup blackboard
        }
    }
}