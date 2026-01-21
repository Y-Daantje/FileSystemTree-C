using System.Collections.Immutable;


namespace FileSystemTree
{
    class FileProgram
    {
        static void Main(string[] args)
        {
            string baseDirectoryPath = GetBaseDirectoryPath();
            DirectoryInfo baseDirectory = new DirectoryInfo(baseDirectoryPath);

            FileSystemTreeItem fileSystemTree = GetFileSystemTree(baseDirectory);

            OutputFileSystemTreeLevel(0, fileSystemTree);
        }

        static void OutputFileSystemTreeLevel(int indentationLevel, FileSystemTreeItem item)
        {
            // set indentation based on level in tree 2 spaces per level
            string indentation = new string(Enumerable.Repeat(' ', indentationLevel * 2).ToArray());

            //combine the indentation with the current tree items name and type
            Console.WriteLine(indentation + item.Name + " (" + item.Type + ")");

            if (item.Children != null && item.Children.Count() > 0)
            {
                foreach (FileSystemTreeItem child in item.Children)
                {
                    OutputFileSystemTreeLevel(indentationLevel + 1, child);
                }
            }

        }
        static FileSystemTreeItem GetFileSystemTree(DirectoryInfo baseDirectory)
        {
            //Read all subdirectories and files from the current baseDirectory
            DirectoryInfo[] subdirectories;
            FileInfo[] files;
            try
            {
                subdirectories = baseDirectory.GetDirectories();
                files = baseDirectory.GetFiles();
            }
            catch (UnauthorizedAccessException)
            {
                //Skip directories don't have permission to read
                return new FileSystemTreeItem(baseDirectory.Name, FileSystemTreeItemType.Directory, ImmutableArray<FileSystemTreeItem>.Empty);
            }
            catch (IOException)
            {
                //Skip unreadable directories/files
                return new FileSystemTreeItem(baseDirectory.Name, FileSystemTreeItemType.Directory, ImmutableArray<FileSystemTreeItem>.Empty);
            }

            List<FileSystemTreeItem> children = new List<FileSystemTreeItem>();

            //First recursively add all subdirectories with its children to the current tree item
            foreach (DirectoryInfo subdirectory in subdirectories)
            {
                //add all tree items from 
                children.Add(GetFileSystemTree(subdirectory));
            }

            //Lastly add all files of the current tree item
            foreach (FileInfo file in files)
            {
                children.Add(new FileSystemTreeItem(file.Name, FileSystemTreeItemType.File, null));
            }

            return new FileSystemTreeItem(baseDirectory.Name, FileSystemTreeItemType.Directory, children.ToImmutableArray());
        }

        static string GetBaseDirectoryPath()
        {
            string path;
            do
            {
                Console.Clear(); //Clear the console window
                Console.Write("Please enter a valid directory path: ");
                path = Console.ReadLine();

                //While the user input is not a valid path and therefore doesn't exist, continue to prompt for a valid directory path
            } while (!Directory.Exists(path));
            return path;
        }
    }
}



