using System;
using System.IO;
using System.Xml.Linq;
using Sit.ActionsManager.Externals;
using Sit.ActionsManager.Proxy;
using StructureMap;

namespace Sit.ActionsManager.IO
{
    public class SolutionXmlSession
    {
        private readonly string _solutionFilePath;
        private readonly IFileSystem _fileSystem;
        private XDocument _loadedXmlDocument;

        public SolutionXmlSession(string path)
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

        public ObjectContainer Parse()
        {
            using (var stream = _fileSystem.OpenStream(_solutionFilePath))
            {
                _loadedXmlDocument = XDocument.Load(stream);
            }

            var validator = new SolutionXmlValidator(_loadedXmlDocument);
            validator.Validate();

            var reader = new SolutionXmlParser(_loadedXmlDocument);
            return reader.GetObjectContainer();
        }
    }
}