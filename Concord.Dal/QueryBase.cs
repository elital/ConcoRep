using System.Collections.Generic;

namespace Concord.Dal
{
    public abstract class QueryBase
    {
        private string _fieldComparison = " {0} = :{0} ";
        //private string _fieldLikeComparison = " {0} like %:{1}% ";

        protected void AddComparison(ref string statement, string comparisonField, object value, List<KeyValuePair<string, object>> parameters)
        {
            if ((value == null) ||
                (value is string && string.IsNullOrEmpty((string) value)))
                return;

            var cond = statement.ToUpper().Contains("WHERE") ? " and " : " where ";
            statement = $"{statement} {cond} {string.Format(_fieldComparison, comparisonField)}";
            parameters.Add(new KeyValuePair<string, object>(comparisonField, value));
        }

        //protected void AddOrLikeComparison(ref string statement, string comparisonField, string value, List<KeyValuePair<string, object>> parameters)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return;

        //    var cond = statement.ToUpper().Contains("WHERE") ? " or " : " where ";
        //    var parts = value.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        //    var i = 1;

        //    foreach (var part in parts)
        //    {
        //        var bindField = $"{comparisonField}{i}";
        //        var comparison = string.Format(_fieldLikeComparison, comparisonField, bindField);
        //        statement = $"{statement} {cond} {comparison}";
        //        parameters.Add(new KeyValuePair<string, object>(comparisonField, part));
        //        cond = " or ";
        //        i++;
        //    }
        //}
    }
}