using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace blExternals.blReflection
{
    public static class ReflectionHelpers
    {
        public static readonly BindingFlags AllBindingFlags = (BindingFlags) int.MaxValue;
        
        public static MemberInfo[] GetMembers<TCustomAttribute>(this Type type,
            BindingFlags flags = (BindingFlags)int.MaxValue, 
            MemberTypes memberTypes = MemberTypes.Property | MemberTypes.Field, 
            bool isRecursive = false) 
            
            where TCustomAttribute : Attribute
        {
            var props = type
                .GetMembers(flags)
                .Where(p => p.GetCustomAttribute<TCustomAttribute>(true) != null 
                    && (p.MemberType & memberTypes) > 0);

            return (type.BaseType != null && isRecursive) 
                ? props.Union(type.BaseType?.GetMembers<TCustomAttribute>(flags)).ToArray() 
                : props.ToArray();
        }

        public static PropertyInfo[] GetProps<T>(BindingFlags flags = (BindingFlags)int.MaxValue) 
            => typeof(T)
                .GetProperties(flags)
                .ToArray();

        public static string[] PropNames<T>() 
            => GetProps<T>()
                .Select(p => p.Name).ToArray();

        public static string[] WritablePropNames<T>() 
            => GetProps<T>()
                .Where(p => p.CanWrite)
                .Select(p => p.Name).ToArray();
        
        public static IEnumerable<Tuple<Type, string[]>> CollectProps<T>() 
            => GetProps<T>()
                .GroupBy(prop => prop.PropertyType)
                .Select(g => Tuple.Create(g.Key, g.Select(pi => pi.Name).ToArray()));
        
        public static Type MakeGenericType<TType>(Type[] args)
        {
            var type = typeof(TType);
            
            if (!type.IsGenericType)
            {
                return null;
            }
            try
            {
                return type.MakeGenericType(args);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);                
            }
            return null;
        }

        public static bool IsAccessor(this MemberInfo info) 
            => (info.MemberType & (MemberTypes.Property | MemberTypes.Field)) != 0;

        public static Type AccessorValueType(this MemberInfo info)
        {
            return (info as PropertyInfo)?.PropertyType
                            ?? (info as FieldInfo)?.FieldType;
        }

        public static bool IsAccessorTo<TValue>(this MemberInfo info)
        {
            var valueType = (info as PropertyInfo)?.PropertyType 
                            ?? (info as FieldInfo)?.FieldType;

            return (valueType != null) 
                   && (valueType.IsAssignableFrom(typeof(TValue)));
        }
        
        public static bool IsAccessorReadable(this MemberInfo info) 
            => (info as PropertyInfo)?.CanRead ?? (info is FieldInfo);

        public static bool IsAccessorWritable(this MemberInfo info) 
            => (info as PropertyInfo)?.CanWrite ?? (info is FieldInfo);
        
        /// <summary>
        /// Use after CollectProps!
        /// </summary>>
        public static FieldExtractor<T1, T2> GetExtractorFor<T1,T2>(
            IEnumerable<Tuple<Type, string[]>> propsCollection)
        {
            var proplist = propsCollection.First(el => el.Item1 == typeof(T2));
            
            return proplist == null 
                ? null : new FieldExtractor<T1, T2>(proplist.Item2);
        }
        
        ///<summary>
        ///Extract everything to jagged arrays.
        ///Then transpose it.
        ///</summary>>
        public static object[][] ExtractToObjectArrays<T1>(
            FieldExtractor<T1, object> extractor, ICollection<T1> entries) 
        {
            var ecount = entries.Count;
            var extracted = new object[entries.Count][];
            
            for (var i=0; i < entries.Count; i++) 
            {
                extracted[i] = extractor.Extract(entries.ElementAt(i));
            }
            object[][] jagged = new object[extractor.Props.Length][];
            
            for (var i = 0; i < extractor.Props.Length; i++) 
            {
                jagged[i] = new object[ecount];
            }

            for (var i = 0; i < entries.Count; i++) 
            {
                for (var j=0; j < extractor.Props.Count(); j++) 
                {
                    jagged[j][i] = extracted[i][j];
                }
            }
            return jagged;
        }
    }
}