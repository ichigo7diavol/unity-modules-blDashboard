using System;
using blExternals.blBind;
using UnityEngine.Experimental.UIElements;

namespace blExternals.blEditorFramework.UIElements
{
    public class BaseBindableField<TValue> 
        : BaseField<TValue>
    {
        private IBind _bind;
            
        public BaseBindableField(IBind bind)
        {
            if (bind.ValueType != typeof(TValue))
            {
                throw new ArgumentException($"{bind.ValueType} != {typeof(TValue)}");
            }
            _bind = bind;
        }
    }
}