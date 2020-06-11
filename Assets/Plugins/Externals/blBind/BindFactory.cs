using System;
using System.Reflection;
using blExternals.blReflection;

namespace blExternals.blBind
{
    public class BindFactory : BaseFactory<IBind>
    {
        protected override Type _genericType { get; set; } = typeof(Bind<,>);

        protected override Type[] ResolveGenericParameters(params object[] parameters)
        {
            var types = base.ResolveGenericParameters(parameters);

            types[0] = parameters[0].GetType();
            types[1] = ((MemberInfo) parameters[1]).AccessorValueType();
            
            return types;
        }
    }
}