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
        } 
    }
}