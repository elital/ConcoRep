using AutoMapper;
using Concord.App.Models;
using Concord.Entities;

namespace Concord.App.Mapping
{
    public class GroupMapping
    {
        public static void MapGroup()
        {
            Mapper.CreateMap<GroupModel, Group>()
                //.ForMember(d => d.Name, a => a.MapFrom(s => s.Name))
                .ForMember(d => d.Words, a => a.MapFrom(s => s.Words));

            Mapper.CreateMap<Group, GroupModel>()
                //.ForMember(d => d.Name, a => a.MapFrom(s => s.Name))
                .ForMember(d => d.Words, a => a.MapFrom(s => s.Words));
        }
    }
}