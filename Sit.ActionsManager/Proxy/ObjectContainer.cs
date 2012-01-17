using System.Collections.Generic;

namespace Sit.ActionsManager.Proxy
{
    public class ObjectContainer
    {
        public string DefaultModuleIdentifier { get; set; }

        public Solution Solution { get; set; }

        public Settings Settings { get; set; }

        public Dictionary<string, Action> Actions { get; set; }

        public Dictionary<string, Module> Modules { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}