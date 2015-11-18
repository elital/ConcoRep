using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal
{
    public class SequenceQuery
    {
        private const string SequenceValueColumn = "VAL";
        private readonly string _getNextSongIdStatement = $"select SONGS_S.NEXTVAL {SequenceValueColumn} from dual";
        private readonly string _getNextSongWordIdStatement = $"select SONG_WORDS_S.NEXTVAL {SequenceValueColumn} from dual";
        private readonly string _getNextWordIdStatement = $"select WORDS_S.NEXTVAL {SequenceValueColumn} from dual";

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
            return OracleDataLayer.Instance.Select(ReadSequenceValue, _getNextSongIdStatement);
            //return GetNextId(_getNextSongIdStatement);
        }

        //private int GetNextId(string statement)
        //{
        //    return OracleDataLayer.Instance.Select(ReadSequenceValue, statement);
        //}

        public int GetSongWordId()
        {
            return OracleDataLayer.Instance.Select(ReadSequenceValue, _getNextSongWordIdStatement);
            //return GetNextId(_getNextSongWordIdStatement);
        }

        public int GetWordId()
        {
            return OracleDataLayer.Instance.Select(ReadSequenceValue, _getNextWordIdStatement);
            //return GetNextId(_getNextWordIdStatement);
        }

        private int ReadSequenceValue(OracleDataReader reader)
        {
            if (!reader.Read())
                return 0;

            return (int)(decimal)reader[SequenceValueColumn];
        }
    }
}