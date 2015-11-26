using System;
using System.Collections.Generic;
using Concord.Entities;

namespace Concord.Dal.WordEntity
{
    public class WordCreator
    {
        #region Properties

        public string Text { get; set; }
        public int? Repetitions { get; set; }

        #endregion

        #region Field names

        private const string IdText = "ID";
        private const string WordText = "WORD";
        private const string RepetitionText = "REPETITION";

        #endregion

        #region Statements

        private readonly string _insertIntoWordsStatement = $"insert into WORDS(ID, WORD, REPETITION) " +
                                                            $"values(:{IdText}, :{WordText}, :{RepetitionText})";

        private readonly string _increaseWordRepetitionStatement = $"update WORDS W " +
                                                                   $"set W.REPETITION = W.REPETITION + 1 " +
                                                                   $"where W.ID = :{IdText}";

        #endregion

        // TODO : Singleton or not?
        #region Singleton

        private static WordCreator _instance;
        public static WordCreator Instance
        {
            get { return _instance ?? (_instance = new WordCreator()); }
            set { _instance = value; }
        }

        private WordCreator() { }

        #endregion

        //public Word Create(string text, int repetitions)
        //{
        //    return Create(text, repetitions, true);
        //}

        internal Word Create(string text, int repetitions, bool commit)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(Utils.GetMemberName(() => text));

            // TODO : Singleton or not?
            //ValidateProperties();

            var id = SequenceQuery.Instance.GetWordId();

            OracleDataLayer.Instance.DmlAction(_insertIntoWordsStatement,
                new KeyValuePair<string, object>(IdText, id),
                new KeyValuePair<string, object>(WordText, text),
                new KeyValuePair<string, object>(RepetitionText, repetitions));

            if (commit)
                OracleDataLayer.Instance.Commit();

            return new WordQuery().GetWordById(id);
        }

        internal bool IncreaseRepetition(int id, bool commit)
        {
            var rowcount = OracleDataLayer.Instance.DmlAction(_increaseWordRepetitionStatement, new KeyValuePair<string, object>(IdText, id));

            if (commit)
                OracleDataLayer.Instance.Commit();

            return rowcount == 1;
        }


        // TODO : Singleton or not?
        //private void ValidateProperties()
        //{
        //    if (string.IsNullOrEmpty(Text))
        //        throw new PropertyNullException(Utils.GetMemberName(() => Text));

        //    if (!Repetitions.HasValue)
        //        throw new PropertyNullException(Utils.GetMemberName(() => Repetitions));
        //}
    }
}