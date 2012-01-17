using System.Collections.Generic;

namespace Sit.ActionsManager.Proxy
{
    public class Category : Item
    {
        public IEnumerable<string> ActionIdentifiers
        {
            get; internal set;
        }

        public IEnumerable<Category> Categories
        {
            get; internal set;
        } 
    }
}