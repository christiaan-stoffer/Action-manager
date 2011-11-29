
namespace Sit.ActionsManager.Proxy
{
    public class Action : Item
    {
        public string ProviderIdentifier { get; set; }

        public string CategoryIdentifier { get; set; }
       
        public ActionGroup Prerequisites { get; set; }
    }
}
