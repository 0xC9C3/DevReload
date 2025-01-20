using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DreamPoeBot.Framework.Helpers;
using DreamPoeBot.Loki.Bot;

namespace DevReload.Proxy
{
    public class Store
    {
        private static Dictionary<string, IBase> innerInstances = [];
        private static Assembly assembly;

        public static T GetInnerInstanceByType<T>(Type type)
        {
            Type targetType = typeof(T);
            string keyName = $"{targetType.Name}_{type.Name}";

            var inst = innerInstances.First(i => i.Key == keyName);

            if (inst.Value == null)
            {
                DevReloadPlugin.Log.Error($"[DevReload] No instance for ${type} found");
            }

            return (T)inst.Value;
        }

        public static void InitializeAll()
        {
            innerInstances.ForEach((_, b) => b.Initialize());
        }

        public static void DeinitializeAll()
        {
            innerInstances.ForEach((_, b) => b.Deinitialize());
        }

        public static void LoadAssembly()
        {
            assembly = Util.LoadAssembly(Settings.Instance.TargetPath);

            DevReloadPlugin.Log.Debug($"[DevReload] Assembly loaded, looking for interfaces");

            (Type, Type)[] targetInterfaces = [
                (typeof(IBot), typeof(BotProxy)),
                (typeof(IPlayerMover), typeof(MoverProxy)),
                (typeof(IPlugin), typeof(PluginProxy)),
                (typeof(IRoutine), typeof(RoutineProxy)),
            ];

            foreach(var (targetType, proxyType) in targetInterfaces) {
                Type[] BotTypes = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(targetType)).ToArray();
                foreach (var b in BotTypes)
                {
                    DevReloadPlugin.Log.Debug($"[DevReload] found {targetType.Name} {b.Name}");
                    Type newProxyType = TypeFactory.CreateType(b.Name, proxyType);
                    IBot botInstance = (IBot)Activator.CreateInstance(b);
                    string keyName = $"{targetType.Name}_{newProxyType.Name}";
                    innerInstances.Remove(keyName);
                    innerInstances.Add(keyName, botInstance);
                }
            }
        }
    }
}
