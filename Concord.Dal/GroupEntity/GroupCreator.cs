using System.Collections.Generic;
using Concord.Dal.WordEntity;
using Concord.Entities;

namespace Concord.Dal.GroupEntity
{
    public class GroupCreator
    {
        #region Fields names

        private const string IdText = "ID";
        private const string GroupNameText = "GROUP_NAME";
        private const string WordIdText = "WORD_ID";

        #endregion

        #region Statements

        private readonly string _createNewWord = $"insert into WORD_GROUPS(ID, GROUP_NAME, WORD_ID) " +
                                                 $"values(:{IdText}, :{GroupNameText}, :{WordIdText})";

        #endregion

        // TODO : Singleton or not?
        #region Singleton

        private static GroupCreator _instance;
        public static GroupCreator Instance => _instance ?? (_instance = new GroupCreator());

        private GroupCreator() { }

        #endregion

        public Word CreateGroupWord(string groupName, int wordId)
        {
            OracleDataLayer.Instance.DmlAction(_createNewWord,
                new KeyValuePair<string, object>(IdText, SequenceQuery.Instance.GetGroupWordId()),
                new KeyValuePair<string, object>(GroupNameText, groupName),
                new KeyValuePair<string, object>(WordIdText, wordId));

            return new WordQuery().GetWordById(wordId);
        }

        public Word CreateGroupWord(string groupName, string word)
        {
            return CreateGroupWord(groupName, new WordQuery().GetOrCreateWord(word, false).Id);
        }
    }
}