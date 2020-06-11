using System;

namespace blExternals.blBind
{
    public interface IBind
    {
        Type ValueType { get; }
        Type ModelType { get; }

        object BoxedValue { get; set; }
        object SafeBoxedValue { get; set; }

        TNewValue GetValue<TNewValue>();
        void SetValue<TNewValue>(TNewValue value);
    }
}