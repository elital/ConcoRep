using System;
using System.Collections.Generic;

namespace Concord.Dal
{
    public abstract class QueryBase
    {
        private string _fieldComparison = " {0} = :{0} ";
        private string _fieldLikeComparison = " {0} like :{1} ";
        private string _likeOperator = "%";
        private string _containsLikeFunctionName = "ET_UTILS.CONTAINS_LIKE_PHRASE";
        private string _trueResult = "ET_UTILS.GET_TRUE_RESULT";
        
        protected void AddComparison(ref string statement, string comparisonField, object value, List<KeyValuePair<string, object>> parameters)
        {
            if ((value == null) ||
                (value is string && string.IsNullOrEmpty((string) value)))
                return;

            var cond = statement.ToUpper().Contains("WHERE") ? " and " : " where ";
            statement = $"{statement} {cond} {string.Format(_fieldComparison, comparisonField)}";
            parameters.Add(new KeyValuePair<string, object>(comparisonField, value));
        }
        
        protected void AddContainsLikePhraseComparison(ref string statement, string phrase, List<KeyValuePair<string, object>> parameters)
        {
            var cond = statement.ToUpper().Contains("WHERE") ? " and " : " where ";
            var bindFieldName = "PHRASE";
            statement = $"{statement} {cond} {_containsLikeFunctionName}(ID, :{bindFieldName}) = {_trueResult} ";
            parameters.Add(new KeyValuePair<string, object>(bindFieldName, phrase));
        }
        
        protected void AddOrLikeComparison(ref string statement, string comparisonField, string value, List<KeyValuePair<string, object>> parameters)
        {
            AddLikeComparison(ref statement, comparisonField, value, parameters, "or");
        }

        protected void AddAndLikeComparison(ref string statement, string comparisonField, string value, List<KeyValuePair<string, object>> parameters)
        {
            AddLikeComparison(ref statement, comparisonField, value, parameters, "and");
        }

        private void AddLikeComparison(ref string statement, string comparisonField, string value, List<KeyValuePair<string, object>> parameters, string additionType)
        {
            if (string.IsNullOrEmpty(value))
                return;

            var cond = statement.ToUpper().Contains("WHERE") ? " and " : " where ";
            var parts = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var i = 1;

            statement = $"{statement} {cond} ( ";

            foreach (var part in parts)
            {
                var bindField = $"{comparisonField}{i}";
                var comparison = string.Format(_fieldLikeComparison, comparisonField, bindField);
                statement = i == 1
                    ? $"{statement} {comparison}"
                    : $"{statement} {additionType} {comparison}";
                parameters.Add(new KeyValuePair<string, object>(bindField, $"{_likeOperator}{part}{_likeOperator}"));
                i++;
            }

            statement = $"{statement} ) ";
        }
    }
}