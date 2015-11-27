using AutoMapper;
using Concord.App.Models;
using Concord.Entities;

namespace Concord.App.Mapping
{
    public class RelationMapping
    {
        public static void MapRelation()
        {
            Mapper.CreateMap<Relation, RelationModel>()
                //.ForMember(d => d.Name, a => a.MapFrom(s => s.Name))
                .ForMember(d => d.Pairs, a => a.MapFrom(s => s.Pairs));

            Mapper.CreateMap<Pair, PairModel>()
                //.ForMember(d => d.Id, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.FirstWord, a => a.MapFrom(s => s.FirstWord))
                .ForMember(d => d.SecondWord, a => a.MapFrom(s => s.SecondWord));
        } 
    }
}