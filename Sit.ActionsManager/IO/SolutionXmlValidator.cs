using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Sit.ActionsManager.IO
{
    internal class SolutionXmlValidator
    {
        private XDocument _xml;
        public const string SchemaName = "http://www.stofferit.nl/Schema/ActionManager_v1";
        private const string XsdResourcePath = "Sit.ActionsManager.Xsd.solution.xsd";

        public SolutionXmlValidator(XDocument xml)
        {
            _xml = xml;
        }

        public void Validate()
        {
            var set = new XmlSchemaSet();

            set.Add(SchemaName, new XmlTextReader(OpenXsdStream()));

            var builder = new StringBuilder();

            _xml.Validate(set, (o, e) =>
                                   {
                                       builder.AppendLine(
                                           String.Format("Xsd validation error on line {0} and column {1}: ",
                                                         e.Exception.LineNumber, e.Exception.LinePosition));
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