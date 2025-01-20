using DreamPoeBot.Loki.Bot;

namespace DevReload.Proxy
{
    public abstract class BotProxy : BaseProxy<IBot>, IBot
    {
        public void Start() => _innerInstance.Start();

        public void Stop() => _innerInstance.Stop();

        public void Tick() => _innerInstance.Tick();
    }
}
