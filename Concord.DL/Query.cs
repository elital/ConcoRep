using System;
using System.Linq;
using System.Net.Sockets;
using Concord.Entities;

namespace Concord.DL
{
    public class Query
    {
        private readonly string _queryNameStatement = "";


        public Word GetOrCreateWord(string text, bool increaseRepetition)
        {
            return OracleDataLayer.Instance.Select<Word>(
                (reader) =>
                    {
                        while (reader.Read())
                        {
                            //reader["MYFIELD"];
                        }
                        return new Word();
                    }
                , "select...");
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
    }
}