using Mapster;
using Notely.Core.Application.Responses.Categories;
using Notely.Core.Application.Responses.Notes;
using Notely.Core.Application.Responses.Tags;
using Notely.Core.Domain.Entities;
using Shared.DTOs;

namespace Notely.Core.Application.Common.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Category, CategoryItem>.NewConfig()
            .Map(dest => dest.NotesCount, src => src.Notes != null ? src.Notes.Count : 0);

        TypeAdapterConfig<Note, NoteItem>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Title : null)
            .Map(dest => dest.Tags, src => src.NoteTags != null ? src.NoteTags.Select(nt => nt.Tag.Title).ToList() : new List<string>());

        TypeAdapterConfig<Note, GetNoteByIdResponse>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Title : null)
            .Map(dest => dest.Tags, src => src.NoteTags != null ? src.NoteTags.Select(nt => nt.Tag.Title).ToList() : new List<string>());

        TypeAdapterConfig<Category, GetCategoryByIdResponse>.NewConfig()
            .Map(dest => dest.NotesCount, src => src.Notes != null ? src.Notes.Count : 0)
            .Map(dest => dest.Notes, src => src.Notes != null ? src.Notes.Select(n => new CategoryNoteItem
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content.Length > 100 ? n.Content.Substring(0, 100) + "..." : n.Content,
                CreatedAt = n.CreatedAt
            }).ToList() : new List<CategoryNoteItem>());

        TypeAdapterConfig<Tag, GetTagByIdResponse>.NewConfig()
            .Map(dest => dest.NotesCount, src => src.NoteTags != null ? src.NoteTags.Count : 0);

        TypeAdapterConfig<GetNoteByIdResponse, NoteDto>.NewConfig();
        TypeAdapterConfig<CategoryItem, CategoryDto>.NewConfig();
        TypeAdapterConfig<NoteItem, NoteDto>.NewConfig();
    }
}
