using System;
using System.Reflection;
using blExternals.blReflection;

namespace blExternals.blBind
{
    public class Bind<TModel, TValue> : IBind
    {
        [Flags]
        public enum BindingTypes
        {
            None = 0,
            
            Read = 1,
            Write = 2,
            ReadWrite = 3,
        }

        private Action<TModel, TValue> _setter;
        private Func<TModel, TValue> _getter;

        private TModel _model;
        
        private Type _valueType = typeof(TValue);
        private Type _modelType = typeof(TModel);
        
        private BindingTypes _bindingType;
        
        public TValue Value
        {
            get => _getter(_model); 
            set => _setter(_model, value);
        }

        public TValue SafeValue
        {
            get => IsReadable ? _getter(_model) : default;
            
            set
            {
                if (IsWritable) _setter(_model, value);
            }
        }
        
        public object BoxedValue
        {
            get => _getter(_model);
            set => _setter(_model, (TValue)value);
        }
        
        public object SafeBoxedValue
        {
            get => IsReadable ? _getter(_model) : default;
            set
            {
                if (IsWritable) _setter(_model, (TValue)value);
            }
        }
        
        public BindingTypes BindingType => _bindingType;

        public bool IsReadable => (_bindingType & BindingTypes.Read) != 0;
        public bool IsWritable => (_bindingType & BindingTypes.Write) != 0;

        public Type ValueType => _valueType;
        public Type ModelType => _modelType;

        public Bind(TModel model, MemberInfo info)
        {
            _model = model;

            _getter = info.CreateGetter<TModel, TValue>();
            _setter = info.CreateSetter<TModel, TValue>();

            _bindingType |= (_getter != null) ? BindingTypes.Read : BindingTypes.None;
            _bindingType |= (_setter != null) ? BindingTypes.Write : BindingTypes.None;
        }
        
        public TNewValue GetValue<TNewValue>()
        {
            if (Value is TNewValue value)
            {
                return value;
            }
            return default;
        }

        public void SetValue<TNewValue>(TNewValue value)
        {
            if (value is TValue castedValue)
            {
                Value = castedValue;
            }
        }
    }
}