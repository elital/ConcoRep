using System.Collections.Generic;
using Concord.Dal.WordEntity;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal.RelationEntity
{
    public class RelationQuery
    {
        #region Fields names

        private const string IdText = "ID";
        private const string NameText = "NAME";
        private const string FirstWordIdText = "FIRST_WORD_ID";
        private const string SecondWordIdText = "SECOND_WORD_ID";

        #endregion

        #region Statements

        private readonly string _getStatement = "select * " +
                                                "from RELATIONS R " +
                                                "order by R.NAME";

        #endregion
        
        public IEnumerable<Relation> Get()
        {
            return OracleDataLayer.Instance.Select(ReadRelations, _getStatement);
        }

        private Relation ReadRelationPair(OracleDataReader reader, out Pair pair)
        {
            pair = null;

            if (!reader.Read())
                return null;

            pair = new Pair
                {
                    Id = (int) reader[IdText],
                    FirstWord = new WordQuery().GetWordById((int) reader[FirstWordIdText]),
                    SecondWord = new WordQuery().GetWordById((int) reader[SecondWordIdText])
                };

            return new Relation
                {
                    Name = (string) reader[NameText]
                };
        }

        private IEnumerable<Relation> ReadRelations(OracleDataReader reader)
        {
            var relations = new List<Relation>();
            Relation relation;
            Relation lastRelation = null;
            Pair pair;

            while ((relation = ReadRelationPair(reader, out pair)) != null)
            {
                if (lastRelation == null)
                    lastRelation = relation;

                if (lastRelation.Name != relation.Name)
                {
                    relations.Add(lastRelation);
                    lastRelation = relation;
                }

                lastRelation.Pairs.Add(pair);
            }

            if (lastRelation != null)
                relations.Add(lastRelation);

            return relations;
        }
    }
}