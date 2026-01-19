using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace FileSystemTree
{
    class FileProgram
    {
        static void Main(string[] args)
        {
            string baseDirectoryPath = GetBaseDirectoryPath();
            DirectoryInfo baseDirectory = new DirectoryInfo(baseDirectoryPath);

            FileSystemTreeItem fileSystemTree = GetFileSystemTree(baseDirectory);

            OutputFileSystemTreeLevel(fileSystemTree, 0);
        }

        static void OutputFileSystemTreeLevel(int indentationLevel, FileSystemTreeItem item)
        {
            
        }
        static FileSystemTreeItem GetFileSystemTree()
        {
            
        }
        static string GetBaseDirectoryPath()
        {
         
        }
    }
}