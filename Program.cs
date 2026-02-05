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

            OutputFileSystemTree(fileSystemTree);
        }

        //item (the current file or folder)
        //indentation (the current level of indentation)
        //isLast (whether the current item is the last child of its parent)
        //isRoot (whether the current item is the root of the tree) the beginning of the tree
        static void OutputFileSystemTree(FileSystemTreeItem item, string indentation = "", bool isLast = true, bool isRoot = true)
        {
            List<FileSystemTreeItem> children = item.Children?.ToList() ?? new List<FileSystemTreeItem>();
            // Output the current item with appropriate indentation and connector
            string connector;
            if (isRoot)
            {
                // for the root item, no connector is needed
                connector = "";
            }
            else
            {
                // for the middle or last children use different connectors
                connector = isLast ? "└── " : "├── ";
            }
            // Recursively output each child item
            for (int i = 0; i < children.Count; i++)
            {
                // Determine if the child is the last in the list
                bool childIsLast = i == children.Count - 1;
                // Update indentation for child items
                string childIndentation;
                if (isRoot)
                {
                    childIndentation = "";
                }
                else
                {
                    childIndentation = indentation + (isLast ? "    " : "│   ");
                }
                OutputFileSystemTree(children[i], childIndentation, childIsLast, false);
            }
            Console.WriteLine(indentation + connector + $"{item.Name,15} " + $"{item.Type,7} " + $"({item.Length,5} bytes) " + $"{item.CreationTime,12}");
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
            catch (Exception)
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
                children.Add(new FileSystemTreeItem(file.Name, FileSystemTreeItemType.File, file.Length));
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
                path = Console.ReadLine() ?? string.Empty;

                //when the user input is not a valid path doesn't exist, continue to prompt for a valid directory path
            } while (!Directory.Exists(path));
            return path;
        }
    }
}


