using System.Xml.Linq;

namespace Sit.ActionsManager.IO
{
    internal static class XElementExtensions
    {
        public static string GetAttributeValue(this XElement element, XName attributeName)
        {
            var attribute = element.Attribute(attributeName);

            var attributeValue = attribute == null ? string.Empty : attribute.Value;

            return attributeValue;
        }

        public static string GetFirstChildElementsText(this XElement element, XName elementName)
        {
            var child = element.Element(elementName);

            return child == null ? string.Empty : child.Value;
        }
    }
}