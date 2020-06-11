using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace blExternals.blReflection
{
    // NOTES:
    //
    // 1) Sealed params are that initialized once!
    //
    public abstract class BaseFactory<TInterface>
        : IFactory<TInterface>
    {
        private readonly Dictionary<int, Func<object[], object>> _generators 
            = new Dictionary<int, Func<object[], object>>();

        protected abstract Type _genericType { get; set; }
        
        protected virtual List<Tuple<int, Type>> _sealedGenericParameters { get; set; }
            = new List<Tuple<int, Type>>
            {
            };
        
        private Type[] _genericTypeArhuments;
        
        protected BaseFactory()
        {
            if (!_genericType?.IsGenericType ?? false)
            {
                throw new ArgumentException($"{_genericType.Name} is not generic type or null!");
            }
            _genericTypeArhuments = _genericType.GetGenericArguments();
        }
        
        public virtual TInterface Create(params object[] parameters)
        {
            var types = ResolveGenericParameters(parameters);
            var hash = Utilities.GetHashCode(types);
        
            if (_generators.TryGetValue(hash, out var generator))
            {
                return (TInterface)generator(parameters);
            }
            generator = ExpressionHelpers
                .CreateGenericConstructor(FindConstructor(types, 
                    parameters.Select(p => p.GetType()).ToArray()));
        
            _generators[hash] = generator;
            
            return Populate((TInterface)generator(parameters));
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public void ClearCache()
        {
            _generators.Clear();
        }

        /// <summary>
        /// Searches constructor by passed constructor parameters.
        /// </summary>
        /// <param name="parameters">Parameters that passed to constructor.</param>
        /// <returns>Constructor info.</returns>
        protected virtual ConstructorInfo FindConstructor(Type[] arguments, Type[] parameters)
        {
            return _genericType.MakeGenericType(arguments).GetConstructor(parameters);
        }

        /// <summary>
        /// Dynamic parameters resolving. Use sealed params by default.
        /// </summary>
        /// <param name="parameters">Parameters that passed to constructor.</param>
        /// <returns>Types that specify generic parameters at runtime.</returns>
        protected virtual Type[] ResolveGenericParameters(params object[] parameters)
        {
            _sealedGenericParameters.Sort((left, right) 
                => left.Item1.CompareTo(right.Item1));

            var typeArray = new Type[_genericTypeArhuments.Length];

            for(var i = 0; i < _sealedGenericParameters.Count; ++i)
            {
                typeArray[_sealedGenericParameters[i].Item1] 
                    = _sealedGenericParameters[i].Item2;
            }
            return typeArray;
        }
        
        /// <summary>
        /// Populate object after creating.
        /// </summary>
        /// <param name="obj">Created object.</param>
        /// <returns>Created object.</returns>
        protected virtual TInterface Populate(TInterface obj)
        {
            return obj;
        }
    }
}