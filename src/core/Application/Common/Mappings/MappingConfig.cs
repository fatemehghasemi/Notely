using Mapster;
using Notely.Core.Application.Responses.Categories;
using Notely.Core.Application.Responses.Notes;
using Notely.Core.Domain.Entities;
using Shared.DTOs;

namespace Notely.Core.Application.Common.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CategoryItem, CategoryDto>.NewConfig();
        TypeAdapterConfig<NoteItem, NoteDto>.NewConfig();
        TypeAdapterConfig<GetNoteByIdResponse, NoteDto>.NewConfig();

        TypeAdapterConfig<Category, CategoryItem>.NewConfig()
            .Map(dest => dest.NotesCount, src => src.Notes != null ? src.Notes.Count : 0);

        TypeAdapterConfig<Note, NoteItem>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Title : null)
            .Map(dest => dest.Tags, src => src.NoteTags != null ? src.NoteTags.Select(nt => nt.Tag.Title).ToList() : new List<string>());

        TypeAdapterConfig<Note, GetNoteByIdResponse>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Title : null)
            .Map(dest => dest.Tags, src => src.NoteTags != null ? src.NoteTags.Select(nt => nt.Tag.Title).ToList() : new List<string>());

        TypeAdapterConfig<Note, CreateNoteResponse>.NewConfig();
        
        TypeAdapterConfig<Note, UpdateNoteResponse>.NewConfig()
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt ?? src.CreatedAt);

        TypeAdapterConfig<Category, CreateCategoryResponse>.NewConfig();
        
        TypeAdapterConfig<Category, UpdateCategoryResponse>.NewConfig();

        TypeAdapterConfig<Category, GetCategoryByIdResponse>.NewConfig()
            .Map(dest => dest.NotesCount, src => src.Notes != null ? src.Notes.Count : 0)
            .Map(dest => dest.Notes, src => src.Notes != null ? src.Notes.Select(n => new CategoryNoteItem
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content.Length > 100 ? n.Content.Substring(0, 100) + "..." : n.Content,
                CreatedAt = n.CreatedAt
            }).ToList() : new List<CategoryNoteItem>());

        TypeAdapterConfig<GetCategoryByIdResponse, CategoryDto>.NewConfig();

        TypeAdapterConfig<CreateNoteDto, Note>.NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
            .Ignore(dest => dest.UpdatedAt)
            .Ignore(dest => dest.Category)
            .Ignore(dest => dest.NoteTags)
            .Ignore(dest => dest.Color);

        TypeAdapterConfig<UpdateNoteDto, Note>.NewConfig()
            .Map(dest => dest.UpdatedAt, src => DateTime.UtcNow)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.Category)
            .Ignore(dest => dest.NoteTags)
            .Ignore(dest => dest.Color);

        TypeAdapterConfig<CreateCategoryDto, Category>.NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
            .Ignore(dest => dest.UpdatedAt)
            .Ignore(dest => dest.Notes);

        TypeAdapterConfig<UpdateCategoryDto, Category>.NewConfig()
            .Map(dest => dest.UpdatedAt, src => DateTime.UtcNow)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.Notes);
    }
}
