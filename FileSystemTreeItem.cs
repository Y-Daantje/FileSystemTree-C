
namespace FileSystemTree
{
    public class FileSystemTreeItem
    {
        // private fields for name, type and children
        //name of the file or directory, type indicates if its a file or directory, children holds the child items(if any)
        //readonly makes sure that these fields cannot be modified after construction
        // This makes the object immutable (read-only after creation).
        private readonly IEnumerable<FileSystemTreeItem> children;
        private readonly FileSystemTreeItemType type;
        private readonly string name;
        private readonly int size;

        public FileSystemTreeItem(string name, FileSystemTreeItemType type, IEnumerable<FileSystemTreeItem> children, int size)
        {
            this.children = children;
            this.name = name;
            this.type = type;
            this.size = size;
        }

        // public properties to access the private fields
        // Children property returns the child items
        // Type property returns whether it's a file or directory
        // Name property returns the name of the item
        public IEnumerable<FileSystemTreeItem> Children { get { return this.children; } }
        public FileSystemTreeItemType Type { get { return this.type; } }
        public string Name { get { return this.name; } }
        public int? Size { get { return this.size; } }



    }

    public enum FileSystemTreeItemType
    {

        Directory,
        File
    }

}