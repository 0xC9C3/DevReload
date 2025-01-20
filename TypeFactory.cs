using System;
using System.Reflection;
using System.Reflection.Emit;
using DevReload.Proxy;
using DreamPoeBot.Loki.Bot;

// https://stackoverflow.com/a/3862241
namespace DevReload
{
    public static class TypeFactory
    {
        public static Type CreateType(string typeName, Type interfaceType, Type proxyType)
        {
            var an = new AssemblyName("ProxyAssembly" + typeName);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType("ProxyType" + typeName.Replace(" ", ""),
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    proxyType,
                    [interfaceType]
            );

            

            return tb.CreateType();
        }
    }
}