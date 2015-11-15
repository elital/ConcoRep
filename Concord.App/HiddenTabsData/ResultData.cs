using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public SongModel Song { get; set; }

        private ResultData()
        {
            Contexts = new List<ContextModel>();
            Song = new SongModel
                {
                    Id = 1,
                    Title = "asda",
                    Author = "asdasd",
                    Album = "asdasdasdad",
                    PublishDate = DateTime.Now,
                    Text = $"la la la{Environment.NewLine}lalalala"
                };
            
            Contexts.Add(new ContextModel
                {
                    SongTitle = "title la la la",
                    Author = "author baby",
                    Album = "album the sun",
                    ContextLineNumber = 7,
                    ContextColumnNumber = 3,
                    ContextLine1 = "first line",
                    ContextLine2 = "second line",
                    ContextLine3 = "third line"
                });

            Contexts.Add(new ContextModel
                {
                    SongTitle = "title hey hey hey",
                    Author = "author father",
                    Album = "album the end of the galaxy",
                    ContextLineNumber = 2,
                    ContextColumnNumber = 1,
                    ContextLine1 = "one one one",
                    ContextLine2 = "twooooooooooooooooo",
                    ContextLine3 = "three"
                });
        }
    }
}