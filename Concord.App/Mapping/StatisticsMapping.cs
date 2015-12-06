using AutoMapper;
using Concord.App.Models;
using Concord.Entities;

namespace Concord.App.Mapping
{
    public class StatisticsMapping
    {
        public static void MapStatistics()
        {
            Mapper.CreateMap<SongStatistics, SongStatisticsModel>();
            //.ForMember(d => d.WordsAmount, a => a.MapFrom(s => s.WordsAmount))
            //.ForMember(d => d.LongestWord, a => a.MapFrom(s => s.LongestWord))
            //.ForMember(d => d.LongestWordLength, a => a.MapFrom(s => s.LongestWordLength))
            //.ForMember(d => d.ShortestWord, a => a.MapFrom(s => s.ShortestWord))
            //.ForMember(d => d.ShortestWordLength, a => a.MapFrom(s => s.ShortestWordLength))
            //.ForMember(d => d.MostRepeatedWord, a => a.MapFrom(s => s.MostRepeatedWord))
            //.ForMember(d => d.MostRepeatedWordRepetitions, a => a.MapFrom(s => s.MostRepeatedWordRepetitions));

            Mapper.CreateMap<SystemStatistics, SystemStatisticsModel>();
                //.ForMember(d => d.TotalSongsAmount, a => a.MapFrom(s => s.TotalSongsAmount))
                //.ForMember(d => d.TotalSystemSongsWordsAmount, a => a.MapFrom(s => s.TotalSystemSongsWordsAmount))
                //.ForMember(d => d.SystemDifferentSongsWordsAmount, a => a.MapFrom(s => s.SystemDifferentSongsWordsAmount))
                //.ForMember(d => d.LongestSongName, a => a.MapFrom(s => s.LongestSongName))
                //.ForMember(d => d.LongestSongWordsAmount, a => a.MapFrom(s => s.LongestSongWordsAmount))
                //.ForMember(d => d.ShortestSongName, a => a.MapFrom(s => s.ShortestSongName))
                //.ForMember(d => d.ShortestSongWordsAmount, a => a.MapFrom(s => s.ShortestSongWordsAmount))
                //.ForMember(d => d.MostRepeatedWord, a => a.MapFrom(s => s.MostRepeatedWord))
                //.ForMember(d => d.MostRepeatedWordRepetition, a => a.MapFrom(s => s.MostRepeatedWordRepetition))
                //.ForMember(d => d.LongestWord, a => a.MapFrom(s => s.LongestWord))
                //.ForMember(d => d.LongestWordLength, a => a.MapFrom(s => s.LongestWordLength))
                //.ForMember(d => d.ShortestWord, a => a.MapFrom(s => s.ShortestWord))
                //.ForMember(d => d.ShortestWordLength, a => a.MapFrom(s => s.ShortestWordLength));
        } 
    }
}