using System;
using System.Collections.Generic;
using blExternals.blBind;
using UnityEngine.Experimental.UIElements;
using blExternals.blReflection;

namespace blExternals.blEditorFramework.UIElements
{
    // TODO: factory generated methods container
    
    public class BaseBindableFieldFactory : BaseFactory<VisualElement>
    {
        private static readonly Dictionary<Type, Func<VisualElement>> _generators 
            = new Dictionary<Type, Func<VisualElement>>();
        
        protected override Type _genericType { get; set; } = typeof(BaseBindableField<>);
        
        protected override Type[] ResolveGenericParameters(params object[] parameters)
        {
            var types = base.ResolveGenericParameters(parameters);

            types[0] = ((IBind)parameters[0]).ValueType;
            
            return types;
        }
    }
}