using System.Collections.Generic;
using System.Linq;
using Concord.Dal.WordEntity;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal.GroupEntity
{
    public class GroupQuery : QueryBase
    {
        #region Properties

        public string Name { get; set; }
        public string Word { get; set; }

        #endregion

        #region Fields names

        private const string IdText = "ID";
        private const string GroupNameText = "GROUP_NAME";
        private const string WordText = "WORD";
        
        #endregion

        #region Statements

        private readonly string _getStatement = $"select * " +
                                                $"from   WORD_GROUPS WG " +
                                                $"     , WORDS       W " +
                                                $"where  WG.WORD_ID = W.ID ";

        private readonly string _orderByClause = " order by WG.GROUP_NAME, WG.ID ";

        #endregion

        public Group SingleOrDefault()
        {
            return Get().SingleOrDefault();
        }

        public IEnumerable<Group> Get()
        {
            var statement = _getStatement;
            var parameters = new List<KeyValuePair<string, object>>();

            AddComparison(ref statement, GroupNameText, Name, parameters);
            AddComparison(ref statement, WordText, Word, parameters);

            statement = $"{statement} {_orderByClause}";

            return OracleDataLayer.Instance.Select(ReadGroups, statement, parameters.ToArray());
        }

        private Group ReadGroupWord(OracleDataReader reader, out Word word)
        {
            word = null;

            if (!reader.Read())
                return null;

            word = WordQuery.ReadWord(reader, false);

            return new Group
                {
                    Name = (string) reader[GroupNameText]
                };
        }

        private IEnumerable<Group> ReadGroups(OracleDataReader reader)
        {
            var groups = new List<Group>();
            Group group;
            Group lastGroup = null;
            Word word;

            while ((group = ReadGroupWord(reader, out word)) != null)
            {
                if (lastGroup == null)
                    lastGroup = group;

                if (lastGroup.Name != group.Name)
                {
                    groups.Add(lastGroup);
                    lastGroup = group;
                }

                lastGroup.Words.Add(word);
            }

            if (lastGroup != null)
                groups.Add(lastGroup);

            return groups;
        }
    }
}