

using System;
using System.IO;
using System.Reflection;
using DreamPoeBot.Loki.Common;
using log4net;
using System.Runtime.Loader;


namespace DevReload
{
    public class Util
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();

        private static CollectibleAssemblyLoadContext context;

        public static Assembly LoadAssembly(
            string path
        )
        {
            if (context != null) {
                UnloadAssembly();
            }

            byte[] contents = File.ReadAllBytes(path);
            System.IO.Stream ms = new System.IO.MemoryStream(contents);
            context = new CollectibleAssemblyLoadContext();
            return context.LoadFromStream(ms);
        }

        public static void UnloadAssembly()
        {
            if (context == null)
                return;

            context.Unload();
            context = null;
        }
    }

    public class CollectibleAssemblyLoadContext 
    : AssemblyLoadContext
{
    public CollectibleAssemblyLoadContext() : base(isCollectible: true)
    { }
 
    protected override Assembly Load(AssemblyName assemblyName)
    {
        return null;
    }
}
}