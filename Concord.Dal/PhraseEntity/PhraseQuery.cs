using System.Collections.Generic;
using Concord.Dal.WordEntity;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal.PhraseEntity
{
    public class PhraseQuery : QueryBase
    {
        #region Properties

        public int? PhraseNumber { get; set; }

        #endregion

        #region Fields names
        
        private const string IdText = "ID";
        private const string PhraseNumberText = "PHRASE_NUMBER";
        private const string WordSequenceText = "WORD_SEQUENCE";
        private const string WordIdText = "WORD_ID";

        #endregion

        #region Statements

        private readonly string _getAll = "select * " +
                                          "from   PHRASES P ";
        private readonly string _orderBy = " order by P.PHRASE_NUMBER, P.WORD_SEQUENCE ";

        #endregion

        public IEnumerable<Phrase> Get()
        {
            var statement = _getAll;
            var parameters = new List<KeyValuePair<string, object>>();
            AddComparison(ref statement, PhraseNumberText, PhraseNumber, parameters);
            statement = $"{statement} {_orderBy}";

            return OracleDataLayer.Instance.Select(ReadPhrases, statement, parameters.ToArray());
        }

        private Phrase ReadPhraseWord(OracleDataReader reader, out PhraseWord word)
        {
            word = null;

            if (!reader.Read())
                return null;

            word = new PhraseWord
                {
                    Id = (int) reader[IdText],
                    WordSequence = (int) reader[WordSequenceText],
                    Word = new WordQuery().GetWordById((int) reader[WordIdText])
                };

            return new Phrase
                {
                    PhraseNumber = (int) reader[PhraseNumberText]
                };
        }

        private IEnumerable<Phrase> ReadPhrases(OracleDataReader reader)
        {
            var phrases = new List<Phrase>();
            Phrase phrase;
            Phrase lastPhrase = null;
            PhraseWord word;

            while ((phrase = ReadPhraseWord(reader, out word)) != null)
            {
                if (lastPhrase == null)
                    lastPhrase = phrase;

                if (lastPhrase.PhraseNumber != phrase.PhraseNumber)
                {
                    phrases.Add(lastPhrase);
                    lastPhrase = phrase;
                }

                lastPhrase.Words.Add(word);
            }

            if (lastPhrase != null)
                phrases.Add(lastPhrase);

            return phrases;
        }
    }
}