using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace VSI.EDGEAXConnector.Common
{
    public static class ObjectExtensions
    {
        public static string SerializeToJson(this object o)
        {
            JsonSerializer serializer =
                JsonSerializer.Create(
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Error,
                        Formatting = Formatting.None,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    });

            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, o);
                return sw.ToString();
            }
        }

        public static string SerializeToJson(this object o, int depth)
        {
            JsonSerializer serializer =
                JsonSerializer.Create(
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Error,
                        Formatting = Formatting.None,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        DateTimeZoneHandling = DateTimeZoneHandling.Local,
                        MaxDepth = depth
                    });

            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, o);
                return sw.ToString();
            }
        }

        public static TResult EvalDeepNull<TArg, TResult>(this TArg arg, Expression<Func<TArg, TResult>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (ReferenceEquals(arg, null))
            {
                return default(TResult);
            }

            var stack = new Stack<MemberExpression>();
            var expr = expression.Body as MemberExpression;
            while (expr != null)
            {
                stack.Push(expr);
                expr = expr.Expression as MemberExpression;
            }

            if (stack.Count == 0 || !(stack.Peek().Expression is ParameterExpression))
            {
                throw new Exception(String.Format("The expression '{0}' contains unsupported constructs.", expression));
            }

            object a = arg;
            while (stack.Count > 0)
            {
                expr = stack.Pop();
                var p = expr.Expression as ParameterExpression;
                if (p == null)
                {
                    p = Expression.Parameter(a.GetType(), "x");
                    expr = expr.Update(p);
                }
                LambdaExpression lambda = Expression.Lambda(expr, p);
                Delegate t = lambda.Compile();
                a = t.DynamicInvoke(a);
                if (ReferenceEquals(a, null))
                {
                    return default(TResult);
                }
            }

            return (TResult)a;
        }
    }
}
