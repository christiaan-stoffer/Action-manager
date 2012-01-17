using System.IO;
using System.Xml.Schema;
using Moq;
using NUnit.Framework;
using Sit.ActionsManager.IO;

namespace Sit.ActionsManager.Test
{
    [TestFixture]
    public class SolutionParserFixture : FixtureBase
    {
        private Stream CreateStreamForValidDocument()
        {
            return new StreamReader(@"TestResources\validdocument.xml", true).BaseStream;
        }

        private Stream CreateStreamForInvalidDocument()
        {
            return new StreamReader(@"TestResources\invaliddocument.xml", true).BaseStream;
        }

        [Test]
        [ExpectedException(typeof (XmlSchemaValidationException))]
        public void ParseInvalidDocument()
        {
            TestContext.Current.FileSystemMoq.Setup(fs => fs.OpenStream(It.IsAny<string>())).Returns(
                CreateStreamForInvalidDocument);

            var parser = new SolutionXmlSession("mypath");

            parser.Parse();
        }

        [Test]
        public void ParseValidDocument()
        {
            TestContext.Current.FileSystemMoq.Setup(fs => fs.OpenStream(It.IsAny<string>())).Returns(
                CreateStreamForValidDocument);

            var parser = new SolutionXmlSession("mypath");

            var container = parser.Parse();

            Assert.IsTrue(container.Solution.Name == "Mijn mooie oplossing");
        }
    }
}