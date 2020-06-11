using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace blExternals.blReflection
{
    public class FieldCopier<TTarget, TSrc>
    {
        public readonly string[] Props;
        
        private readonly Action<TTarget, TSrc> _assignExpr;
        
        private Action<TTarget, TSrc> CreateExpression(IEnumerable<string> fields)
        {
            var t1param = ExpressionHelpers.Parameter<TTarget>("left");
            var t2param = ExpressionHelpers.Parameter<TSrc>("right");

            var assignList = fields
                .Select(f => Expression.Assign(t1param.PropertyOrField(f), t2param.PropertyOrField(f)));
            
            //var resultArr = Expression.NewArrayInit(typeof(bool), cmplist);
            
            var block = Expression.Block(assignList);
            var l = Expression.Lambda<Action<TTarget, TSrc>>(block, t1param, t2param);
 
            return l.Compile();
        }

        public FieldCopier(string[] props)
        {
            this.Props = props;
            this._assignExpr = CreateExpression(props);
        }

        public void Copy(TTarget left, TSrc right) => _assignExpr.Invoke(left, right);
    }
}