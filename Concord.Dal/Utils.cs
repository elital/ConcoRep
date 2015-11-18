using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Concord.Dal
{
    public static class Utils
    {
        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            return ((MemberExpression) memberExpression.Body).Member.Name;
        }
    }

    [Serializable]
    public class PropertyNullException : Exception
    {

        public string PropertyName { get; set; }

        public PropertyNullException(string propertyName)
        {
            PropertyName = propertyName;
        }

        public PropertyNullException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

        public PropertyNullException(string propertyName, string message, Exception inner) : base(message, inner)
        {
            PropertyName = propertyName;
        }

        protected PropertyNullException(string propertyName, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PropertyName = propertyName;
        }
    }
}