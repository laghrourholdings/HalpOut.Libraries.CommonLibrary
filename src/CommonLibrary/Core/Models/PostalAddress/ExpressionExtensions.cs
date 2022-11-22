using System.Linq.Expressions;

namespace CommonLibrary.Core.Models.PostalAddress
{
    public static class ExpressionExtensions
    {
        public static string PropertyName<T>(this Expression<Func<T, object>> property)
        {
            var member = property.Body as MemberExpression;
            if (member != null)
                return member.Member.Name;

            var unaryExpression = property.Body as UnaryExpression;
            if (unaryExpression != null)
                member = unaryExpression.Operand as MemberExpression;

            return member != null ? member.Member.Name : string.Empty;
        }

        public static string PropertyName<T>(this T instance, Expression<Func<T, object>> property)
        {
            return property.PropertyName();
        }
    }
}
