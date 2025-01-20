using DreamPoeBot.Loki.Bot;

namespace DevReload.Proxy
{
    public abstract class PluginProxy : BaseProxy<IPlugin>, IPlugin
    {
        public void Disable() => _innerInstance.Disable();

        public void Enable() => _innerInstance.Enable();
    }
}
