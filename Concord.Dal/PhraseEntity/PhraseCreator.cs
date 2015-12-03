using System;
using System.Collections.Generic;
using System.Linq;
using Concord.Dal.WordEntity;
using Concord.Entities;

namespace Concord.Dal.PhraseEntity
{
    public class PhraseCreator
    {
        #region Fields names

        private const string IdText = "ID";
        private const string PhraseNumberText = "PHRASE_NUMBER";
        private const string WordSequenceText = "WORD_SEQUENCE";
        private const string WordIdText = "WORD_ID";
        
        #endregion

        #region Statements

        private readonly string _createNewPhraseWord = $"insert into PHRASES(ID, PHRASE_NUMBER, WORD_SEQUENCE, WORD_ID) " +
                                                       $"values(:{IdText}, :{PhraseNumberText}, :{WordSequenceText}, :{WordIdText})";

        #endregion

        // TODO : Singleton or not?
        #region Singleton

        private static PhraseCreator _instance;
        public static PhraseCreator Instance => _instance ?? (_instance = new PhraseCreator());

        private PhraseCreator() {}

        #endregion

        public Phrase Create(string phraseText)
        {
            var phraseWords = phraseText.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            if (!phraseWords.Any())
                return null;

            var phraseNumber = SequenceQuery.Instance.GetPhraseNumber();
            var wordSequence = 0;

            foreach (var phraseWord in phraseWords)
            {
                var id = SequenceQuery.Instance.GetPhraseWordId();
                wordSequence++;
                var wordId = new WordQuery().GetOrCreateWord(phraseWord, false, false).Id;

                OracleDataLayer.Instance.DmlAction(_createNewPhraseWord,
                    new KeyValuePair<string, object>(IdText, id),
                    new KeyValuePair<string, object>(PhraseNumberText, phraseNumber),
                    new KeyValuePair<string, object>(WordSequenceText, wordSequence),
                    new KeyValuePair<string, object>(WordIdText, wordId));
            }

            OracleDataLayer.Instance.Commit();

            return new PhraseQuery {PhraseNumber = phraseNumber}.Get().SingleOrDefault();
        }
    }
}