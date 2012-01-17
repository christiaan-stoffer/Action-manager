using System.Collections;
using System.Collections.Generic;

namespace Sit.ActionsManager.Proxy
{
    public class Settings : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly Dictionary<string, string> _internal;

        public Settings(Dictionary<string, string> settings)
        {
            _internal = settings;
        }

        public bool ContainsKey(string key)
        {
            return _internal.ContainsKey(key);
        }

        public bool ContainsValue(string value)
        {
            return _internal.ContainsValue(value);
        }

        public Dictionary<string, string>.ValueCollection Values
        {
            get { return _internal.Values; }
        }

        public Dictionary<string, string>.KeyCollection Keys
        {
            get { return _internal.Keys; }
        }

        public string this[string key]
        {
            get { return _internal[key]; }
            set { _internal[key] = value; }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _internal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}