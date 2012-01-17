using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Sit.ActionsManager.Proxy;

namespace Sit.ActionsManager.IO
{
    internal class SolutionXmlParser
    {
        private readonly ObjectContainer _container;
        private readonly XDocument _xml;
        private readonly XNamespace _ns;
        private readonly XElement _root;

        public SolutionXmlParser(XDocument xml)
        {
            _xml = xml;

            _root = _xml.Root;

            _ns = SolutionXmlValidator.SchemaName;
            _container = new ObjectContainer();
        }

        public ObjectContainer GetObjectContainer()
        {
            ParseSolution();

            ParseConfiguration();

            ParseActions();

            ParseCategories();

            return _container;
        }

        private void ParseCategories()
        {
            XElement categories = _root.Element(_ns+"categories");

            if (categories == null) return;

            _container.Categories = categories.Elements(_ns + "category").Select(Helpers.ParseAsCategory).ToArray();

        }

        private void ParseSolution()
        {
            _container.Solution = Helpers.ParseAsSolution(_root);
        }

        private void ParseActions()
        {
            XElement actionsElement = _root.Element(_ns + "actions");

            if (actionsElement != null)
            {
                _container.Actions = actionsElement.Elements(_ns + "action")
                    .Select(Helpers.ParseAsAction)
                    .ToDictionary(action => action.Identifier);
            }
        }

        private void ParseConfiguration()
        {
            XElement configurationElement = _root.Element(_ns + "configuration");

            if (configurationElement == null) return;

            ParseModules(configurationElement);

            ParseSettings(configurationElement);
        }

        private void ParseSettings(XElement configurationElement)
        {
            XElement settingsElement = configurationElement.Element(_ns + "settings");

            if (settingsElement == null) return;

            Dictionary<string, string> dictionary =
                settingsElement.Elements(_ns + "setting").Select(elm => new { elm.Value, Key = elm.GetAttributeValue("key") })
                    .ToDictionary(itm => itm.Key, itm => itm.Value);

            _container.Settings = new Settings(dictionary);
        }

        private void ParseModules(XElement configurationElement)
        {
            XElement modulesElement = configurationElement.Element(_ns + "modules");

            if (modulesElement == null) return;

            _container.Modules = modulesElement
                .Elements(_ns + "module")
                .Select(Helpers.ParseAsModule)
                .ToDictionary(Helpers.GetKey);

            ParseDefaultModuleIdentifier(modulesElement);
        }

        private void ParseDefaultModuleIdentifier(XElement modulesElement)
        {
            _container.DefaultModuleIdentifier = modulesElement.GetFirstChildElementsText(_ns + "default");
        }

        #region Nested type: Helpers

        private static class Helpers
        {
            private static XNamespace _ns = SolutionXmlValidator.SchemaName;

            public static string GetKey(Item item)
            {
                return item.Identifier;
            }

            public static Module ParseAsModule(XElement element)
            {
                var module = new Module();

                ParseItem(element, module);

                return module;
            }

            public static Action ParseAsAction(XElement element)
            {
                var action = new Action();

                ParseItem(element, action);

                return action;
            }

            public static Solution ParseAsSolution(XElement element)
            {
                var solution = new Solution();

                ParseDescriptable(element, solution);

                return solution;
            }

            public static Category ParseAsCategory(XElement element)
            {
                var category = new Category();

                ParseItem(element, category);

                // Parse children
                category.ActionIdentifiers =
                    element
                    .Elements(_ns+"item")
                    .Elements(_ns+"action")
                    .Select(elm => elm.Value).ToArray();

                // Parse child categories
                category.Categories =
                    element
                        .Elements(_ns+"item")
                        .Elements(_ns + "category")
                        .Select(ParseAsCategory).ToArray();

                return category;
            }

            private static void ParseDescriptable(XElement element, Descriptable descriptable)
            {
                descriptable.Name = element.GetAttributeValue("name");
                descriptable.Description = element.GetFirstChildElementsText(_ns + "description");
            }

            private static void ParseItem(XElement element, Item item)
            {
                ParseDescriptable(element, item);
                item.Identifier = element.GetAttributeValue("id");
            }
        }

        #endregion
    }
}