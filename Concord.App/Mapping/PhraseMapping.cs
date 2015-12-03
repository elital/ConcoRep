using AutoMapper;
using Concord.App.Models;
using Concord.Entities;

namespace Concord.App.Mapping
{
    public class PhraseMapping
    {
        public static void MapPhrase()
        {
            Mapper.CreateMap<Phrase, PhraseModel>()
                .ForMember(d => d.PhraseNumber, a => a.MapFrom(s => s.PhraseNumber))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Text));
        }
    }
}