using System.Collections.Generic;
using Concord.Dal.WordEntity;
using Concord.Entities;

namespace Concord.Dal.RelationEntity
{
    public class RelationCreator
    {
        #region Fields names

        private const string IdText = "ID";
        private const string RelationNameText = "GROUP_NAME";
        private const string FirstWordIdText = "FIRST_WORD_ID";
        private const string SecondWordIdText = "SECOND_WORD_ID";

        #endregion

        #region Statements

        private readonly string _createNewPair = $"insert into RELATIONS(ID, NAME, FIRST_WORD_ID, SECOND_WORD_ID) " +
                                                 $"values(:{IdText}, :{RelationNameText}, :{FirstWordIdText}, :{SecondWordIdText})";

        #endregion

        // TODO : Singleton or not?
        #region Singleton

        private static RelationCreator _instance;
        public static RelationCreator Instance => _instance ?? (_instance = new RelationCreator());

        private RelationCreator() { }

        #endregion

        private int CreateRelationPair(string relationName, int firstWordId, int secondWordId, bool commit)
        {
            var id = SequenceQuery.Instance.GetRelationPairId();

            OracleDataLayer.Instance.DmlAction(_createNewPair,
                new KeyValuePair<string, object>(IdText, id),
                new KeyValuePair<string, object>(RelationNameText, relationName),
                new KeyValuePair<string, object>(FirstWordIdText, firstWordId),
                new KeyValuePair<string, object>(SecondWordIdText, secondWordId));

            if (commit)
                OracleDataLayer.Instance.Commit();

            return id;
        }

        public Pair CreateRelationPair(string relationName, string firstWord, string secondWord)
        {
            if (string.IsNullOrEmpty(firstWord) || string.IsNullOrEmpty(secondWord))
                return null;

            var first = new WordQuery().GetOrCreateWord(firstWord, false, false);
            var second = new WordQuery().GetOrCreateWord(secondWord, false, false);

            return new Pair
                {
                    Id = CreateRelationPair(relationName, first.Id, second.Id, true),
                    FirstWord = first,
                    SecondWord = second
                };
        }
    }
}