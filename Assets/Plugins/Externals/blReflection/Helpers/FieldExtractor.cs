// FastExpressionKit
// License: MIT: Copyright (c) 2017 Ville M. Vainio <vivainio@gmail.com>
// See details at https://github.com/vivainio/FastExpressionKit

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace blExternals.blReflection
{
    // Creates extractor, that returns registered fields of object via reflection.
    public class FieldExtractor<T1, TVal>
    {
        public readonly string[] Props;
        
        private readonly Func<T1, TVal[]> _expr;

        public FieldExtractor(string[] props)
        {
            this.Props = props;
            this._expr = CreateExpression(props);
        }

        public TVal[] Extract(T1 obj) => _expr.Invoke(obj);

        public IEnumerable<Tuple<string, TP>> ResultsAsZip<TP>(ICollection<TP> hits)
        {
            var r =  Enumerable.Zip(Props, hits, (p,h) => Tuple.Create(p, h));
            return r;
        }

        /// <summary>
        /// Hits can be any enumerable, as long as it can be zipped with Props!
        /// </summary>
        public Dictionary<string, TP> ResultsAsDict<TP>(ICollection<TP> hits)
        {
            var d = new Dictionary<string, TP>();
            
            for (var i = 0; i < hits.Count; i++)
            {
                d[Props[i]] = hits.ElementAt(i);
            }
            return d;
        }
        
        private Func<T1, TVal[]> CreateExpression(IEnumerable<string> fields)
        {
            var t1param = ExpressionHelpers.Parameter<T1>("obj");
            
            var elist = fields
                .Select(f => t1param.PropertyOrField(f))
                .Cast<Expression>();

            if (typeof(TVal) == typeof(object))
            {
                elist = elist.Select(e => ExpressionHelpers.Box(e));
            }
            var resultArr = Expression.NewArrayInit(typeof(TVal), elist);
            var l = Expression.Lambda<Func<T1, TVal[]>>(resultArr, t1param);
            
            return l.Compile();
        }
    }
}