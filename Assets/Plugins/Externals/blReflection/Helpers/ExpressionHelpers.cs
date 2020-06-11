using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace blExternals.blReflection
{
    public static class ExpressionHelpers
    {
        public static ParameterExpression Variable<T>(string name) 
            => Expression.Variable(typeof(T), name);
        
        public static ParameterExpression Parameter<T>(string name) 
            => Expression.Parameter(typeof(T), name);
        
        public static MemberExpression PropertyOrField(this Expression exp, string fieldName) 
            => Expression.Field(exp, fieldName);

        public static MemberExpression Accessor(this Expression exp, MemberInfo info)
        {
            switch (info)
            {
                case PropertyInfo propertyInfo:
                    return exp.Accessor(propertyInfo);
                
                case FieldInfo fieldInfo:
                    return exp.Accessor(fieldInfo);
                
                default:
                    return null;
            }
        }

        public static MemberExpression Accessor(this Expression exp, PropertyInfo info) 
            => Expression.Property(exp, info);
        
        public static MemberExpression Accessor(this Expression exp, FieldInfo info) 
            => Expression.Field(exp, info);
        
        public static Expression IsEqual(this Expression left, Expression right) 
            => Expression.Equal(left, right);
        
        public static Expression Constant<T>(T value) 
            => Expression.Constant(value, typeof(T));
        
        public static MethodInfo MethodInfo(Expression<Action> exp) 
            => (exp.Body as MethodCallExpression)?.Method;
        
        public static Expression Box(Expression e)
            => Expression.Convert(e, typeof(object));

        public static Action<TModel, TValue> CreateSetter<TModel, TValue>(this MemberInfo info)
        {
            if (!(info.IsAccessor() && info.IsAccessorTo<TValue>() && info.IsAccessorWritable()))
            {
                return null;
            }
            var modelParameter = Expression.Parameter(info.DeclaringType, "model");
            var valueParameter = Parameter<TValue>("value");
            
            var memberExpression = (Expression) modelParameter.Accessor(info);
            
            return Expression.Lambda<Action<TModel, TValue>>(Expression.Assign(memberExpression, 
                    valueParameter), modelParameter, valueParameter)
                .Compile(false);
        }
        
        public static Func<TModel, TValue> CreateGetter<TModel, TValue>(this MemberInfo info)
        {
            if (!(info.IsAccessor() && info.IsAccessorTo<TValue>() && info.IsAccessorReadable()))
            {
                return null;
            }
            var modelParameter = Expression.Parameter(info.DeclaringType, "model");
            var memberExpression = (Expression) modelParameter.Accessor(info);
            
            return Expression.Lambda<Func<TModel, TValue>>(memberExpression, modelParameter)
                .Compile(false);
        }
        
        public static Func<TInterface> CreateGenericConstructor<TInterface,TGeneric>(
            params Type[] parameters)
        {
            var genericType = typeof(TGeneric).MakeGenericType(parameters);
            
            var func = (Func<TInterface>)Expression.Lambda(Expression.New(genericType))
                .Compile(false);
            
            return func;
        }
        
        public static Func<object[], object> CreateGenericConstructor(ConstructorInfo constructor)
        {
            if (constructor == null)
            {
                throw new ArgumentNullException(nameof(constructor));
            }
            CreateParamsExpressions(constructor, out ParameterExpression argsExp, out Expression[] paramsExps);

            var newExp = Expression.New(constructor, paramsExps);
            var resultExp = Expression.Convert(newExp, typeof(object));
            var lambdaExp = Expression.Lambda(resultExp, argsExp);
            var lambda = lambdaExp.Compile();
            
            return (Func<object[], object>)lambda;
        }
        
        private static void CreateParamsExpressions(MethodBase method, 
            out ParameterExpression argsExp, out Expression[] paramsExps)
        {
            var parameters = method.GetParameters()
                .Select(p => p.ParameterType).ToList();

            argsExp = Expression.Parameter(typeof(object[]), "args");
            paramsExps = new Expression[parameters.Count];

            for (var i = 0; i < parameters.Count; i++)
            {
                var constExp = Expression.Constant(i, typeof(int));
                var argExp = Expression.ArrayIndex(argsExp, constExp);
                paramsExps[i] = Expression.Convert(argExp, parameters[i]);
            }
        }
    }
}