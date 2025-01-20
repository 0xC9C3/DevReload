using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;

namespace DevReload.Proxy
{
    public abstract class MoverProxy : BaseProxy<IPlayerMover>, IPlayerMover
    {
        public PathfindingCommand CurrentCommand => _innerInstance.CurrentCommand;

        public bool MoveTowards(Vector2i position, params dynamic[] user) => _innerInstance.MoveTowards(position, user);

        public void Start() => _innerInstance.Start();

        public void Stop() => _innerInstance.Stop();

        public void Tick() => _innerInstance.Tick();
    }
}
