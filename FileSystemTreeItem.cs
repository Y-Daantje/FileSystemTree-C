
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
        private readonly long length;
        private readonly string creationTime;

        public FileSystemTreeItem(string name, FileSystemTreeItemType type, IEnumerable<FileSystemTreeItem> children, long length)
        {
            this.children = children;
            this.name = name;
            this.type = type;
            this.length = length;
            creationTime = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public FileSystemTreeItem(string name, FileSystemTreeItemType type, long length)
            : this(name, type, Array.Empty<FileSystemTreeItem>(), length)
        { }

        // public properties to access the private fields
        // Children property returns the child items
        // Type property returns whether it's a file or directory
        // Name property returns the name of the item
        public IEnumerable<FileSystemTreeItem> Children { get { return children; } }
        public FileSystemTreeItemType Type { get { return type; } }
        public string Name { get { return name; } }
        public long Length { get { return length; } }
        public string CreationTime { get { return creationTime; } }

    }

    public enum FileSystemTreeItemType
    {
        Directory,
        File
    }

}
