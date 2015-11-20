using AutoMapper;
using Concord.App.Models;
using Concord.Entities;

namespace Concord.App.Mapping
{
    public static class WordMapping
    {
        public static void MapWord()
        {
            Mapper.CreateMap<Word, WordModel>()
                .ForMember(d => d.Id, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Word, a => a.MapFrom(s => s.Text));
                //.ForMember(d => d.Repetitions, a => a.MapFrom(s => s.Repetitions));

            Mapper.CreateMap<WordModel, Word>()
                .ForMember(d => d.Id, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Word));
                //.ForMember(d => d.Repetitions, a => a.MapFrom(s => s.Repetitions));
        }
    }
}