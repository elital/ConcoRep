using System.Collections.Generic;
using Concord.App.Models;

namespace Concord.App.HiddenTabsData
{
    public class ResultData
    {
        private static ResultData _instance;
        public static ResultData Instance
        {
            get { return _instance ?? (_instance = new ResultData()); }
            private set { _instance = value; }
        }

        public List<ContextModel> Contexts { get; set; }
        public int SongId { get; set; }

        private ResultData()
        {
            Contexts = new List<ContextModel>();
            SongId = 0;
        }
    }
}