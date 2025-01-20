using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Common;

namespace DevReload.Proxy
{
    public abstract class BaseProxy: IConfigurable, IMessageHandler, IAuthored, IBase, ILogicProvider, IStartStopEvents, ITickEvents
    {

        protected object _innerInstance {
            get {
                return Store.GetInnerInstanceByType(this.GetType());
            }
        }

        protected M getInnerInstance<M>() {
            return (M)(object)_innerInstance;
        }

        protected bool innerInstanceImplements<M>() {
            return _innerInstance.GetType().GetInterfaces().FirstOrDefault(i => i == typeof(M), null) != null;
        }

        public UserControl Control => getInnerInstance<IConfigurable>().Control;
        public JsonSettings Settings => getInnerInstance<IConfigurable>().Settings;
        public MessageResult Message(Message message) => getInnerInstance<IMessageHandler>().Message(message);

        public string Author => getInnerInstance<IAuthored>().Author;

        public string Description => getInnerInstance<IAuthored>().Description;

        public string Name => "Proxied " + getInnerInstance<IAuthored>().Name;

        public string Version => getInnerInstance<IAuthored>().Version;

        public PathfindingCommand CurrentCommand => getInnerInstance<IPlayerMover>().CurrentCommand;

        public void Deinitialize() => getInnerInstance<IBase>().Deinitialize();

        public void Initialize() => getInnerInstance<IBase>().Initialize();

        public Task<LogicResult> Logic(Logic logic) => getInnerInstance<ILogicProvider>().Logic(logic);

        public void Start()
        {
            if (!innerInstanceImplements<IStartStopEvents>()) {
                return;
            }

            getInnerInstance<IStartStopEvents>().Start();
        }

        public void Stop()
        {
            if (!innerInstanceImplements<IStartStopEvents>()) {
                return;
            }

            getInnerInstance<IStartStopEvents>().Start();
        }

        public void Tick()
        {
            if (!innerInstanceImplements<ITickEvents>()) {
                return;
            }

            getInnerInstance<ITickEvents>().Tick();
        }

        public bool MoveTowards(Vector2i position, params dynamic[] user)
        {
            if (!innerInstanceImplements<IPlayerMover>()) {
                return false;
            }

            return getInnerInstance<IPlayerMover>().MoveTowards(position, user);
        }

        public void Disable()
        {
            if (!innerInstanceImplements<IPlugin>()) {
                return;
            }

            getInnerInstance<IPlugin>().Disable();
        }

        public void Enable()
        {
            if (!innerInstanceImplements<IPlugin>()) {
                return;
            }

            getInnerInstance<IPlugin>().Enable();
        }
    }
}
