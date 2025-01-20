using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;

namespace DevReload.Proxy
{
    public abstract class BaseProxy<T> where T : IConfigurable, IMessageHandler, IAuthored, IBase, ILogicProvider
    {

        protected T _innerInstance {
            get {
                return Store.GetInnerInstanceByType<T>(this.GetType());
            }
        }

        public UserControl Control => _innerInstance.Control;
        public JsonSettings Settings => _innerInstance.Settings;
        public MessageResult Message(Message message) => _innerInstance.Message(message);

        public string Author => _innerInstance.Author;

        public string Description => _innerInstance.Description;

        public string Name => "Proxied " + _innerInstance.Name;

        public string Version => _innerInstance.Version;

        public void Deinitialize() => _innerInstance.Deinitialize();

        public void Initialize() => _innerInstance.Initialize();

        public Task<LogicResult> Logic(Logic logic) => _innerInstance.Logic(logic);
    }
}
