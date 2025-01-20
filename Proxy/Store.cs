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

        public static object GetInnerInstanceByType(Type type)
        {
            string keyName = $"{type.Name}";

            var inst = innerInstances.First(i => i.Key == keyName);

            if (inst.Value == null)
            {
                DevReloadPlugin.Log.Error($"[DevReload] No instance for ${type} found");
            }

            return inst.Value;
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
                (typeof(IBot), typeof(BaseProxy)),
                (typeof(IPlayerMover), typeof(BaseProxyMover)),
                (typeof(IPlugin), typeof(BaseProxyPlugin)),
                (typeof(IRoutine), typeof(BaseProxy)),
            ];

            foreach(var (targetType, targetProxy) in targetInterfaces) {
                Type[] types = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(targetType)).ToArray();
                foreach (var b in types)
                {
                    DevReloadPlugin.Log.Debug($"[DevReload] found {targetType.Name} {b.Name}");
                    Type newProxyType = TypeFactory.CreateType(b.Name, targetType, targetProxy);
                    IBase botInstance = (IBase)Activator.CreateInstance(b);
                    string keyName = $"{newProxyType.Name}";
                    innerInstances.Remove(keyName);
                    innerInstances.Add(keyName, botInstance);
                }
            }
        }
    }
}
