using System.IO;
using System.Xml.Schema;
using Moq;
using NUnit.Framework;
using Sit.ActionsManager.Externals;
using Sit.ActionsManager.IO;
using StructureMap;
using StructureMap.Pipeline;

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

    public abstract class FixtureBase
    {
        [TestFixtureSetUp]
        public void SetupStructureMap()
        {
            TestContext.Current = new TestContext();

            ObjectFactory
                .Initialize(InitStructureMap);
        }

        private static void InitStructureMap(IInitializationExpression x)
        {
            x.For<IFileSystem>().Use(() => TestContext.Current.FileSystemMoq.Object);
        }
    }

    [TestFixture]
    public class SolutionParserFixture : FixtureBase
    {
        [Test]
        public void ParseValidDocument()
        {
            TestContext.Current.FileSystemMoq.Setup(fs=>fs.OpenStream(It.IsAny<string>())).Returns(CreateStreamForValidDocument);

            var parser = new SolutionParser("mypath");

            parser.Parse();
        }

        [Test]
        [ExpectedException(typeof(XmlSchemaValidationException))]
        public void ParseInvalidDocument()
        {
            TestContext.Current.FileSystemMoq.Setup(fs => fs.OpenStream(It.IsAny<string>())).Returns(CreateStreamForInvalidDocument);

            var parser = new SolutionParser("mypath");

            parser.Parse();
        }

        private Stream CreateStreamForValidDocument()
        {
            return new StreamReader(@"TestResources\validdocument.xml", true).BaseStream;
        }

        private Stream CreateStreamForInvalidDocument()
        {
            return new StreamReader(@"TestResources\invaliddocument.xml", true).BaseStream;
        }
    }
}