using System.Collections.Generic;
using System.Linq;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal.WordEntity
{
    public class WordQuery : QueryBase
    {
        public enum OrderBy
        {
            Word,
            Repetitions
        }

        #region Properties
        
        public string Word { get; set; }
        public int? Repetition { get; set; }
        
        public OrderBy? OrderByField { get; set; }

        #endregion

        #region Fields names

        private const string IdText = "ID";
        private const string WordText = "WORD";
        private const string RepetitionText = "REPETITION";

        #endregion

        #region Statements

        private readonly string _getStatement = $"select * " +
                                                $"from WORDS ";

        private readonly string _getWordByIdStatement = $"select * " +
                                                        $"from   WORDS W " +
                                                        $"where  W.ID = :{IdText}";
        
        #endregion

        public Word GetWordById(int id)
        {
            return OracleDataLayer.Instance.Select(ReadWord, _getWordByIdStatement,
                new KeyValuePair<string, object>(IdText, id));
        }

        public Word SingleOrDefault()
        {
            return Get().SingleOrDefault();
        }

        public IEnumerable<Word> Get()
        {
            var statement = _getStatement;
            var parameters = new List<KeyValuePair<string, object>>();

            AddComparison(ref statement, WordText, Word, parameters);
            AddComparison(ref statement, RepetitionText, Repetition, parameters);

            SetOrderBy(ref statement);

            return OracleDataLayer.Instance.Select(ReadWords, statement, parameters.ToArray());
        }

        //public IEnumerable<Word> GetLike()
        //{
        //    var statement = _getStatement;
        //    var parameters = new List<KeyValuePair<string, object>>();

        //    AddOrLikeComparison(ref statement, WordText, Word, parameters);
        //    AddComparison(ref statement, RepetitionText, Repetition, parameters);

        //    return OracleDataLayer.Instance.Select(ReadWords, statement, parameters.ToArray());
        //}
        
        public Word GetOrCreateWord(string text, bool increaseRepetition)
        {
            var word = new WordQuery {Word = text}.SingleOrDefault();
            
            if (word == null)
            {
                var repetition = increaseRepetition ? 1 : 0;

                word = WordCreator.Instance.Create(text, repetition);
            }
            else if (increaseRepetition)
            {
                WordCreator.Instance.IncreaseRepetition(word.Id);
                word.Repetitions++;
            }

            return word;
        }

        private void SetOrderBy(ref string statement)
        {
            if (!OrderByField.HasValue)
                return;

            statement = OrderByField.Value == OrderBy.Word
                ? $"{statement} order by {WordText} "
                : $"{statement} order by {RepetitionText} desc ";
        }

        private Word ReadWord(OracleDataReader reader)
        {
            return ReadWord(reader, true);
        }

        internal static Word ReadWord(OracleDataReader reader, bool read)
        {
            if (read)
                if (!reader.Read())
                    return null;

            return new Word
            {
                Id = (int)reader[IdText],
                Text = (string)reader[WordText],
                Repetitions = (int)reader[RepetitionText]
            };
        }

        private IEnumerable<Word> ReadWords(OracleDataReader reader)
        {
            var words = new List<Word>();

            Word word;

            while ((word = ReadWord(reader)) != null)
                words.Add(word);

            return words;
        }
    }
}