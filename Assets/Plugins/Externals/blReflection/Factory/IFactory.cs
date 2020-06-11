using System;
using UnityEditor;

namespace blExternals.blReflection
{
    // TODO: переименовать, т.к. это не просто фабрика!
    public interface IFactory<out TInterface>
    {
        TInterface Create(params object[] parameters);
        void ClearCache();
    }
}