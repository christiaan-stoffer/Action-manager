using System.IO;

namespace Sit.ActionsManager.Externals
{
    public interface IFileSystem
    {
        bool DoesFileExist(string path);
        Stream OpenStream(string path);
    }
}