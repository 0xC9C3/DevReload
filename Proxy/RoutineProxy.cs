using DreamPoeBot.Loki.Bot;

namespace DevReload.Proxy
{
    public abstract class RoutineProxy : BaseProxy<IRoutine>, IRoutine
    {
        public void Start() => _innerInstance.Start();

        public void Stop() => _innerInstance.Stop();

        public void Tick() => _innerInstance.Tick();
    }
}
