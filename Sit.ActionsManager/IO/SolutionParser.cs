using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Sit.ActionsManager.Externals;
using StructureMap;

namespace Sit.ActionsManager.IO
{
    public class SolutionParser
    {
        private readonly string _solutionFilePath;
        private readonly IFileSystem _fileSystem;

        private XDocument _loadedXmlDocument;
        private const string SchemaName = "http://www.stofferit.nl/Schema/ActionManager_v1";

        private const string XsdResourcePath = "Sit.ActionsManager.Xsd.solution.xsd";

        public SolutionParser(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException();
            }

            _fileSystem = ObjectFactory.GetInstance<IFileSystem>();

            if (!_fileSystem.DoesFileExist(path))
            {
                throw new FileNotFoundException(string.Format("The path '{0}' points to a non-existing file.", path));
            }

            _solutionFilePath = path;
        }

        public void Parse()
        {
            using (var stream = _fileSystem.OpenStream(_solutionFilePath))
            {
                _loadedXmlDocument = XDocument.Load(stream);
            }

            Validate();
        }

        private void Validate()
        {
            var set = new XmlSchemaSet();

            set.Add(SchemaName, new XmlTextReader(OpenXsdStream()));

            var builder = new StringBuilder();

            _loadedXmlDocument.Validate(set, (o,e)=>
                                                 {
                                                     builder.AppendLine(string.Format("Xsd validation error on line {0} and column {1}: ", e.Exception.LineNumber, e.Exception.LinePosition));
                                                     builder.AppendLine(e.Message);

                                                     Debug.Write(e.Message);
                                                 }, true);

            if (builder.Length > 0)
            {
                throw new XmlSchemaValidationException(builder.ToString());
            }
        }

        private Stream OpenXsdStream()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(XsdResourcePath);
        }
    }
}