using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using DifferenceReturnValueType = System.Tuple<string, object>;

namespace blExternals.blReflection
{
    public class Difference<T1, T2>
    {
        public readonly string[] Props;
        
        private readonly Func<T1, T2, DifferenceReturnValueType[]> _comparisonFunc;
        
        private Func<T1, T2, DifferenceReturnValueType[]> CreateExpression(IEnumerable<string> fields)
        {
            var t1param = ExpressionHelpers.Parameter<T1>("left");
            var t2param = ExpressionHelpers.Parameter<T2>("right");
            
            var NULL = ExpressionHelpers.Constant<DifferenceReturnValueType>(null);
            var TupleCreate = ExpressionHelpers.MethodInfo(() => Tuple.Create<string, object>("", null));
            
            var cmplist2 = fields.Select(f
                => Expression.Condition(t1param.PropertyOrField(f).IsEqual(t2param.PropertyOrField(f)),
                    NULL,
                    Expression.Call(TupleCreate, ExpressionHelpers.Constant(f), ExpressionHelpers.Box(t2param.PropertyOrField(f)))));
            
            var resultArr = Expression.NewArrayInit(typeof(DifferenceReturnValueType), cmplist2);
            var l = Expression.Lambda<Func<T1, T2, DifferenceReturnValueType[] >>(resultArr, t1param, t2param);

            return l.Compile();
        }

        public Difference(string[] props)
        {
            this.Props = props;
            this._comparisonFunc = CreateExpression(props);
        }

        public DifferenceReturnValueType[] Compare(T1 left, T2 right) => _comparisonFunc.Invoke(left, right);
    }
}