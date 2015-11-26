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

        public Word CreateGroupWord(string groupName, string wordText)
        {
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(wordText))
                return null;

            var word = new WordQuery().GetOrCreateWord(wordText, false, false);
            CreateGroupWord(groupName, word.Id, true);
            return word;
        }

        private void CreateGroupWord(string groupName, int wordId, bool commit)
        {
            OracleDataLayer.Instance.DmlAction(_createNewWord,
                new KeyValuePair<string, object>(IdText, SequenceQuery.Instance.GetGroupWordId()),
                new KeyValuePair<string, object>(GroupNameText, groupName),
                new KeyValuePair<string, object>(WordIdText, wordId));

            if (commit)
                OracleDataLayer.Instance.Commit();
        }
    }
}