using System;
using System.Linq;
using System.Net.Sockets;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal
{
    public class Query
    {
        private const string WordText = "WORD";
        private const string IdText = "ID";
        private const string RepetitionText = "REPETITION";
        //private const string SequenceNameText = "SequenceName";
        //private readonly string _getSequenceNextVal = $"select :{SequenceNameText}.nextval VAL from dual";
        private readonly string _getWordByWordTextStatement = $"select * from WORDS W where W.WORD = :{WordText}";
        private readonly string _insertIntoWordsStatement =
            $"insert into WORDS(ID, WORD, REPETITION) values(WORDS_S.NEXTVAL, :{WordText}, :{RepetitionText})";
        private readonly string _increaseWordRepetitionStatement =
            $"update WORDS W set W.REPETITION = W.REPETITION + 1 where W.ID = :{IdText}";

        #region Singleton

        private static Query _instance;
        public static Query Instance
        {
            get { return _instance ?? (_instance = new Query()); }
            set { _instance = value; }
        }

        private Query()
        {
            
        }

        #endregion
        
        public Word GetOrCreateWord(string text, bool increaseRepetition)
        {
            var word = OracleDataLayer.Instance.Select(ReadWord, _getWordByWordTextStatement, new[] {WordText, text});

            if (word == null)
            {
                var repetition = increaseRepetition ? 1 : 0;

                OracleDataLayer.Instance.DmlAction(_insertIntoWordsStatement, new[] {WordText, text},
                    new[] {RepetitionText, repetition.ToString()});
            }
            else if (increaseRepetition)
            {
                // TODO : increase repetition (update)
                OracleDataLayer.Instance.DmlAction(_increaseWordRepetitionStatement, new[] {IdText, word.Id.ToString()});
            }

            return word;
        }

        private void BuildSongWords(Song song)
        {
            if (string.IsNullOrEmpty(song.SongText) || song.SongWords.Any())
                return;

            var lines = song.SongText.Split(new[] {Environment.NewLine}, 2, StringSplitOptions.None);
            var currentLine = 0;

            foreach (var line in lines)
            {
                currentLine++;
                var columns = line.Split(' ');
                var currentColumn = 1;

                foreach (var currentWord in columns)
                {
                    // Get or Create
                    var word = new Word();
                    
                    // Create after song / together
                    var songWord = new SongWord {Id = 0, Word = word, Line = currentLine, Column = currentColumn++};
                    
                    song.SongWords.Add(songWord);
                }
            }
        }

        private Word ReadWord(OracleDataReader reader)
        {
            if (!reader.Read())
                return null;

            return new Word
            {
                Id = (int) reader[IdText],
                Text = (string) reader[WordText],
                Repetitions = (int) reader[RepetitionText]
            };
        }
    }
}