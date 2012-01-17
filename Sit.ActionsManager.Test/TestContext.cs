using Moq;
using Sit.ActionsManager.Externals;

namespace Sit.ActionsManager.Test
{
    public class TestContext
    {
        public static TestContext Current;

        private readonly Mock<IFileSystem> _fileSystemMoq;

        public TestContext()
        {
            _fileSystemMoq = new Mock<IFileSystem>();

            // Default behaviours
            _fileSystemMoq.Setup(fs=>fs.DoesFileExist(It.IsAny<string>())).Returns(true);
        }

        public Mock<IFileSystem> FileSystemMoq
        {
            get { return _fileSystemMoq; }
        }
    }
}