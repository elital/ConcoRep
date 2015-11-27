using System.Collections.Generic;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal.General
{
    public class ContextQuery : QueryBase
    {
        #region Properties

        public string Word { get; set; }
        public string GroupName { get; set; }
        public string RelationName { get; set; }

        #endregion

        #region Fields names

        private const string MatchWordText = "MATCH_WORD";
        private const string GroupNameText = "GROUP_NAME";
        private const string RelationNameText = "NAME";
        private const string MatchLocationText = "MATCH_LOCATION";
        private const string SongTitleText = "SONG_TITLE";
        private const string SongAuthorText = "SONG_AUTHOR";
        private const string SongAlbumNameText = "SONG_ALBUM_NAME";
        private const string MatchLineText = "MATCH_LINE";
        private const string MatchColumnText = "MATCH_COLUMN";
        private const string ContextLineText = "CONTEXT_LINE";
        private const string ContextWordText = "CONTEXT_WORD";

        #endregion

        #region Statements

        private readonly string _getAll = "select * " +
                                          "from   CONTEXTS ";
        
        private readonly string _getByGroup = "select * " +
                                              "from   CONTEXTS     C " +
                                              "     , WORD_GROUPS  G " +
                                              "where  C.MATCH_WORD_ID = G.WORD_ID ";

        private readonly string _getByRelation = $"select * " +
                                                 $"from   SONG_WORDS   SW1 " +
                                                 $"     , SONG_WORDS   SW2 " +
                                                 $"     , WORDS        W1 " +
                                                 $"     , WORDS        W2 " +
                                                 $"     , RELATIONS    R " +
                                                 $"     , CONTEXTS     C " +
                                                 $"where  C.SONG_ID = SW1.SONG_ID " +
                                                 $"and    SW1.SONG_ID = SW2.SONG_ID " +
                                                 $"and    SW1.WORD_ID = W1.ID " +
                                                 $"and    SW2.WORD_ID = W2.ID " +
                                                 $"and    SW1.WORD_LINE = SW2.WORD_LINE " +
                                                 $"and    SW1.WORD_COLUMN != SW2.WORD_COLUMN " +
                                                 $"and    R.FIRST_WORD_ID = W1.ID " +
                                                 $"and    R.SECOND_WORD_ID = W2.ID " +
                                                 $"and    C.MATCH_LINE = SW1.WORD_LINE " +
                                                 $"and    ((C.MATCH_COLUMN = SW1.WORD_COLUMN and SW1.WORD_COLUMN < SW2.WORD_COLUMN) or " +
                                                 $"        (C.MATCH_COLUMN = SW2.WORD_COLUMN and SW2.WORD_COLUMN < SW1.WORD_COLUMN)) ";

        #endregion

        public IEnumerable<Context> Get()
        {
            string statement;
            var parameters = new List<KeyValuePair<string, object>>();

            if (!string.IsNullOrEmpty(Word))
            {
                statement = _getAll;
                AddComparison(ref statement, MatchWordText, Word, parameters);
            }
            else if (!string.IsNullOrEmpty(GroupName))
            {
                statement = _getByGroup;
                AddComparison(ref statement, GroupNameText, GroupName, parameters);
            }
            else if (!string.IsNullOrEmpty(RelationName))
            {
                statement = _getByRelation;
                AddComparison(ref statement, RelationNameText, RelationName, parameters);
            }
            else
            {
                statement = _getAll;
            }

            return OracleDataLayer.Instance.Select(ReadContexts, statement, parameters.ToArray());
        }
        
        private List<Context> ReadContexts(OracleDataReader reader)
        {
            var contexts = new List<Context>();
            Context currentContext = null;
            string lastMatchLocation = string.Empty;

            while (reader.Read())
            {
                var currentMatchLocation = (string) reader[MatchLocationText];
                
                if (currentContext == null || currentMatchLocation != lastMatchLocation)
                {
                    lastMatchLocation = currentMatchLocation;

                    if (currentContext != null)
                        contexts.Add(currentContext);

                    currentContext = new Context
                        {
                            SongTitle = (string) reader[SongTitleText],
                            Author = (string) reader[SongAuthorText],
                            Album = (string) reader[SongAlbumNameText],
                            MatchLineNumber = (short) reader[MatchLineText],
                            MatchColumnNumber = (short) reader[MatchColumnText]
                        };
                }

                var currentContextLine = (short) reader[ContextLineText];

                if (currentContext.MatchLineNumber > currentContextLine)
                    currentContext.PreContextLine = ReadContextWord(reader, currentContext.PreContextLine);
                else if (currentContext.MatchLineNumber == currentContextLine)
                    currentContext.ContextLine = ReadContextWord(reader, currentContext.ContextLine);
                else if (currentContext.MatchLineNumber < currentContextLine)
                    currentContext.PostContextLine = ReadContextWord(reader, currentContext.PostContextLine);
            }

            if (currentContext != null)
                contexts.Add(currentContext);

            return contexts;
        }

        private string  ReadContextWord(OracleDataReader reader, string text)
        {
            var word = (string) reader[ContextWordText];
            var line = string.IsNullOrEmpty(text) ? string.Empty : $"{text} ";
            return $"{line}{word}";
        }
    }
}