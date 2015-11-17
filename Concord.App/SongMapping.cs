using AutoMapper;
using Concord.App.Models;
using Concord.Entities;

namespace Concord.App
{
    public static class SongMapping
    {
        public static void MapSong()
        {
            Mapper.CreateMap<SongModel, Song>()
                //.ForMember(s => s.Id, a => a.MapFrom(d => d.Id))
                //.ForMember(s => s.Title, a => a.MapFrom(d => d.Title))
                //.ForMember(s => s.Author, a => a.MapFrom(d => d.Author))
                //.ForMember(s => s.PublishDate, a => a.MapFrom(d => d.PublishDate))
                .ForMember(s => s.AlbumName, a => a.MapFrom(d => d.Album))
                .ForMember(s => s.SongText, a => a.MapFrom(d => d.Text));

            Mapper.CreateMap<Song, SongModel>()
                //.ForMember(s => s.Id, a => a.MapFrom(d => d.Id))
                //.ForMember(s => s.Title, a => a.MapFrom(d => d.Title))
                //.ForMember(s => s.Author, a => a.MapFrom(d => d.Author))
                //.ForMember(s => s.PublishDate, a => a.MapFrom(d => d.PublishDate))
                .ForMember(s => s.Album, a => a.MapFrom(d => d.AlbumName))
                .ForMember(s => s.Text, a => a.MapFrom(d => d.SongText));
        }
    }
}