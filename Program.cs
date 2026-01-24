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
            // start with no indentation at level 0 and increase by 2 spaces for each level deeper in the tree
            string indentation = new string(Enumerable.Repeat(' ', indentationLevel * 2).ToArray());

            // loop through each child item and output its tree level with increased indentation
            if (item.Children != null && item.Children.Count() > 0)
            {
                foreach (FileSystemTreeItem child in item.Children)
                {
                    // recursively output each child item with increased indentation level by 1
                    OutputFileSystemTreeLevel(indentationLevel + 1, child);
                }
            }

            //combine the indentation with the current tree items name and type
            Console.WriteLine($"{indentation}{item.Name} ({item.Type}) ({item.Length} bytes)  ({item.CreationTime})");

        }
        static FileSystemTreeItem GetFileSystemTree(DirectoryInfo baseDirectory)
        {
            // Read all subdirectories and files from the current baseDirectory
            // and using will give you a better view of what is happening/ printing 
            // could have also made it into a string but this way is more visual

            DirectoryInfo[] subdirectories;
            FileInfo[] files;
            try
            {
                subdirectories = baseDirectory.GetDirectories();
                files = baseDirectory.GetFiles();
            }
            // catch exceptions for unauthorized access and IO issues
            catch (UnauthorizedAccessException)
            {
                //Skip directories i don't have permission to read
                //try to read directory but if not possible return empty directory and skip it in the tree
                // ImmutableArray makes sure that the array cannot be modified after creation
                return new FileSystemTreeItem(baseDirectory.Name, FileSystemTreeItemType.Directory, ImmutableArray<FileSystemTreeItem>.Empty, 0);
            }
            catch (IOException)
            {
                //Skip directories that cause IO issues
                return new FileSystemTreeItem(baseDirectory.Name, FileSystemTreeItemType.Directory, ImmutableArray<FileSystemTreeItem>.Empty, 0);
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
                children.Add(new FileSystemTreeItem(file.Name, FileSystemTreeItemType.File, (long)file.Length));
            }

            return new FileSystemTreeItem(baseDirectory.Name, FileSystemTreeItemType.Directory, children.ToImmutableArray(), 0);
        }

        static string GetBaseDirectoryPath()
        {
            string path;
            do
            {
                // clear the console for better readability every time the user is prompted for input
                Console.Clear(); //Clear the console window
                Console.Write("Please enter a valid directory path: ");
                path = Console.ReadLine();

                //when the user input is not a valid path doesn't exist, continue to prompt for a valid directory path
            } while (!Directory.Exists(path));
            return path;
        }
    }
}


