using AutoMapper;
using Concord.App.Models;
using Concord.Entities;

namespace Concord.App.Mapping
{
    public class ContextMapping
    {
        public static void MapContext()
        {
            Mapper.CreateMap<Context, ContextModel>()
                //.ForMember(d => d.SongTitle, a => a.MapFrom(s => s.SongTitle))
                //.ForMember(d => d.Author, a => a.MapFrom(s => s.Author))
                //.ForMember(d => d.Album, a => a.MapFrom(s => s.Album))
                .ForMember(d => d.ContextLineNumber, a => a.MapFrom(s => s.MatchLineNumber))
                .ForMember(d => d.ContextColumnNumber, a => a.MapFrom(s => s.MatchColumnNumber))
                .ForMember(d => d.ContextLine1, a => a.MapFrom(s => s.PreContextLine))
                .ForMember(d => d.ContextLine2, a => a.MapFrom(s => s.ContextLine))
                .ForMember(d => d.ContextLine3, a => a.MapFrom(s => s.PostContextLine));
        }
    }
}