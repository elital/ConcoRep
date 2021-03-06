﻿using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal
{
    public class SequenceQuery
    {
        private const string SongSequenceName = "SONGS_S";
        private const string SongWordSequenceName = "SONG_WORDS_S";
        private const string WordSequenceName = "WORDS_S";
        private const string GroupWordSequenceName = "WORD_GROUPS_S";
        private const string RelationPairSequenceName = "RELATIONS_S";
        private const string PhraseWordSequenceName = "PHRASES_S";
        private const string PhraseNumberSequenceName = "PHRASE_NUMBER_S";

        private const string SequenceValueColumn = "VAL";
        private readonly string _getNextSequenceIdStatement = "select {0}.NEXTVAL {1} from dual";

        #region Singleton

        private static SequenceQuery _instance;
        public static SequenceQuery Instance
        {
            get { return _instance ?? (_instance = new SequenceQuery()); }
            set { _instance = value; }
        }

        private SequenceQuery() { }

        #endregion

        public int GetSongId()
        {
            return GetNextId(SongSequenceName);
        }

        public int GetSongWordId()
        {
            return GetNextId(SongWordSequenceName);
        }

        public int GetWordId()
        {
            return GetNextId(WordSequenceName);
        }

        public int GetGroupWordId()
        {
            return GetNextId(GroupWordSequenceName);
        }

        public int GetRelationPairId()
        {
            return GetNextId(RelationPairSequenceName);
        }

        public int GetPhraseWordId()
        {
            return GetNextId(PhraseWordSequenceName);
        }

        public int GetPhraseNumber()
        {
            return GetNextId(PhraseNumberSequenceName);
        }

        private int GetNextId(string sequenceName)
        {
            return OracleDataLayer.Instance.Select(ReadSequenceValue, string.Format(_getNextSequenceIdStatement, sequenceName, SequenceValueColumn));
        }

        private int ReadSequenceValue(OracleDataReader reader)
        {
            if (!reader.Read())
                return 0;

            return (int)(decimal)reader[SequenceValueColumn];
        }
    }
}