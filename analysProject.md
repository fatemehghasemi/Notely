# 🎯 راهنمای جامع و کامل بازسازی پروژه Notely - Clean Architecture

> **این سند شامل BEST PRACTICES برای تمام لایه‌ها + راهنمای کامل Blazor Architecture است**

---

# 📑 فهرست مطالب

1. [معماری کلی و تصمیمات معماری](#1-معماری-کلی-و-تصمیمات-معماری)
2. [Domain Layer - بهترین شیوه‌ها](#2-domain-layer---بهترین-شیوهها)
3. [Application Layer - CQRS کامل](#3-application-layer---cqrs-کامل)
4. [Infrastructure Layer - پیاده‌سازی حرفه‌ای](#4-infrastructure-layer---پیادهسازی-حرفهای)
5. [Shared Layer - DTOs و Contracts](#5-shared-layer---dtos-و-contracts)
6. [Web Layer - راهنمای جامع Blazor](#6-web-layer---راهنمای-جامع-blazor)
7. [API Layer - RESTful Best Practices](#7-api-layer---restful-best-practices)
8. [Configuration & Deployment](#8-configuration--deployment)
9. [Testing Strategy](#9-testing-strategy)
10. [Performance & Optimization](#10-performance--optimization)

---

# 1. معماری کلی و تصمیمات معماری

## 1.1 ساختار نهایی پروژه (Production-Ready)

```
Notely/
├── 📄 Notely.sln
├── 📄 README.md
├── 📄 .editorconfig
├── 📄 .gitignore
├── 📄 Directory.Build.props          # Global properties
├── 📄 Directory.Packages.props       # Central Package Management
│
├── 📁 src/
│   ├── 📁 Core/                      # ✅ Business Logic Layer
│   │   ├── 📁 Notely.Domain/
│   │   └── 📁 Notely.Application/
│   │
│   ├── 📁 Infrastructure/            # ✅ External Dependencies
│   │   ├── 📁 Notely.Infrastructure/
│   │   └── 📁 Notely.Persistence/    # جداسازی Persistence
│   │
│   ├── 📁 Presentation/              # ✅ UI & API Layer
│   │   ├── 📁 Notely.Web/           # Blazor Server
│   │   └── 📁 Notely.Api/           # REST API (جداگانه)
│   │
│   └── 📁 Shared/                    # ✅ Cross-cutting
│       ├── 📁 Notely.Shared.Abstractions/
│       └── 📁 Notely.Shared.Contracts/
│
├── 📁 tests/                         # ✅ Test Projects
│   ├── 📁 Notely.Domain.UnitTests/
│   ├── 📁 Notely.Application.UnitTests/
│   ├── 📁 Notely.Infrastructure.IntegrationTests/
│   ├── 📁 Notely.Api.IntegrationTests/
│   └── 📁 Notely.ArchitectureTests/  # معماری tests
│
├── 📁 docs/                          # Documentation
│   ├── 📄 ARCHITECTURE.md
│   ├── 📄 API.md
│   └── 📄 DEPLOYMENT.md
│
└── 📁 scripts/                       # Build & Deploy scripts
    ├── 📄 build.ps1
    └── 📄 deploy.ps1
```

## 1.2 تصمیمات کلیدی معماری

### ❓ Blazor Server vs WebAssembly vs Hybrid?

**پاسخ: برای پروژه شما → Blazor Server**

```
چرا Blazor Server؟
✅ Access مستقیم به Application Layer (بدون نیاز به HTTP calls)
✅ Performance بهتر (کد روی سرور اجرا می‌شود)
✅ Security بهتر (Business logic روی سرور)
✅ کد ساده‌تر و maintainable
✅ Database access مستقیم

❌ Blazor WebAssembly فقط برای:
   - Progressive Web Apps (PWA)
   - Offline-first applications
   - Client-heavy processing

⚠️ Blazor Hybrid = پیچیدگی اضافی بدون فایده برای این پروژه
```

### ❓ HttpClient vs MediatR vs Service Layer در Web؟

**پاسخ: MediatR مستقیم (بدون HttpClient)**

```csharp
❌ اشتباه: Blazor Server → HttpClient → API → MediatR → Database
   (Overhead غیرضروری)

✅ درست: Blazor Server → MediatR مستقیم → Database
   (Simple & Fast)

// در Blazor Server Components:
@inject IMediator Mediator

@code {
    private async Task LoadNotes()
    {
        var query = new GetAllNotesQuery();
        var result = await Mediator.Send(query);
        
        if (result.IsSuccess)
        {
            notes = result.Data;
        }
    }
}
```

### ❓ چه زمانی API جداگانه نیاز است؟

```
✅ API جداگانه برای:
   - Mobile apps
   - External integrations
   - Third-party consumers
   - Multiple frontends (React, Angular, etc.)

⚠️ برای پروژه فعلی:
   - اگر فقط Blazor داری → API لازم نیست
   - اگر آینده Mobile/React می‌خواهی → API جدا بساز
```

**توصیه من: ابتدا فقط Blazor Server، بعداً در صورت نیاز API اضافه کن**

---

# 2. Domain Layer - بهترین شیوه‌ها

## 2.1 ساختار کامل Domain

```
Notely.Domain/
├── 📁 Entities/
│   ├── 📄 Note.cs
│   ├── 📄 Category.cs
│   ├── 📄 Tag.cs
│   └── 📄 NoteTag.cs
│
├── 📁 ValueObjects/              # ⭐ NEW - بهبود Domain Model
│   ├── 📄 NoteContent.cs
│   ├── 📄 NoteTitle.cs
│   └── 📄 EmailAddress.cs
│
├── 📁 Enums/
│   ├── 📄 NoteStatus.cs
│   └── 📄 NotePriority.cs
│
├── 📁 Events/                    # ⭐ NEW - Domain Events
│   ├── 📄 NoteCreatedEvent.cs
│   ├── 📄 NoteUpdatedEvent.cs
│   └── 📄 NoteDeletedEvent.cs
│
├── 📁 Exceptions/                # ⭐ NEW - Domain Exceptions
│   ├── 📄 DomainException.cs
│   ├── 📄 NoteNotFoundException.cs
│   └── 📄 InvalidNoteException.cs
│
├── 📁 Common/
│   ├── 📄 BaseEntity.cs
│   ├── 📄 IAuditableEntity.cs
│   ├── 📄 ISoftDeletable.cs
│   └── 📄 IDomainEvent.cs
│
└── 📁 Specifications/            # ⭐ NEW - Specification Pattern
    ├── 📄 NoteSpecification.cs
    └── 📄 ISpecification.cs
```

## 2.2 Rich Domain Model با Best Practices

### BaseEntity با بهترین شیوه‌ها

```csharp
// Common/BaseEntity.cs
public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public TId Id { get; protected set; } = default!;
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    public string? CreatedBy { get; private set; }
    public string? UpdatedBy { get; private set; }

    // Domain Events
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    // Audit methods
    protected void SetCreatedInfo(string? createdBy = null)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    protected void SetUpdatedInfo(string? updatedBy = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    // Equality
    public bool Equals(BaseEntity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is BaseEntity<TId> entity && Equals(entity);
    }

    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }

    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        return !Equals(left, right);
    }
}

// Common/IAuditableEntity.cs
public interface IAuditableEntity
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    string? CreatedBy { get; }
    string? UpdatedBy { get; }
}

// Common/ISoftDeletable.cs
public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    string? DeletedBy { get; }
    void Delete(string? deletedBy = null);
    void Restore();
}

// Common/IDomainEvent.cs
public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}

public abstract record DomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
```

### Value Objects (Rich Domain)

```csharp
// ValueObjects/NoteTitle.cs
public sealed class NoteTitle : ValueObject
{
    public string Value { get; }

    private NoteTitle(string value)
    {
        Value = value;
    }

    public static Result<NoteTitle> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<NoteTitle>.Failure("عنوان نمی‌تواند خالی باشد");

        if (value.Length > 200)
            return Result<NoteTitle>.Failure("عنوان نمی‌تواند بیشتر از 200 کاراکتر باشد");

        if (value.Length < 3)
            return Result<NoteTitle>.Failure("عنوان نمی‌تواند کمتر از 3 کاراکتر باشد");

        return Result<NoteTitle>.Success(new NoteTitle(value));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(NoteTitle title) => title.Value;
}

// ValueObjects/ValueObject.cs (Base Class)
public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;
        if (GetType() != other.GetType()) return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject valueObject && Equals(valueObject);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) => HashCode.Combine(current, obj));
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }
}

// ValueObjects/NoteContent.cs
public sealed class NoteContent : ValueObject
{
    public string Value { get; }

    private NoteContent(string value)
    {
        Value = value;
    }

    public static Result<NoteContent> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<NoteContent>.Failure("محتوا نمی‌تواند خالی باشد");

        if (value.Length > 10000)
            return Result<NoteContent>.Failure("محتوا نمی‌تواند بیشتر از 10000 کاراکتر باشد");

        return Result<NoteContent>.Success(new NoteContent(value));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(NoteContent content) => content.Value;
}
```

### Rich Entity با Business Logic

```csharp
// Entities/Note.cs - نسخه بهبود یافته
public sealed class Note : BaseEntity<Guid>, IAuditableEntity, ISoftDeletable
{
    // Private setters for encapsulation
    public NoteTitle Title { get; private set; } = default!;
    public NoteContent Content { get; private set; } = default!;
    public NoteStatus Status { get; private set; }
    public NotePriority Priority { get; private set; }
    
    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    private readonly List<NoteTag> _noteTags = new();
    public IReadOnlyCollection<NoteTag> NoteTags => _noteTags.AsReadOnly();

    // Soft Delete
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }

    // Private constructor for EF Core
    private Note() { }

    // Factory Method (Best Practice)
    public static Result<Note> Create(
        string title, 
        string content, 
        Guid? categoryId = null,
        string? createdBy = null)
    {
        var titleResult = NoteTitle.Create(title);
        if (titleResult.IsFailure)
            return Result<Note>.Failure(titleResult.Error!);

        var contentResult = NoteContent.Create(content);
        if (contentResult.IsFailure)
            return Result<Note>.Failure(contentResult.Error!);

        var note = new Note
        {
            Id = Guid.NewGuid(),
            Title = titleResult.Data!,
            Content = contentResult.Data!,
            Status = NoteStatus.Draft,
            Priority = NotePriority.Normal,
            CategoryId = categoryId
        };

        note.SetCreatedInfo(createdBy);
        note.AddDomainEvent(new NoteCreatedEvent(note.Id, note.Title.Value));

        return Result<Note>.Success(note);
    }

    // Business Methods
    public Result UpdateTitle(string newTitle, string? updatedBy = null)
    {
        var titleResult = NoteTitle.Create(newTitle);
        if (titleResult.IsFailure)
            return Result.Failure(titleResult.Error!);

        Title = titleResult.Data!;
        SetUpdatedInfo(updatedBy);
        AddDomainEvent(new NoteUpdatedEvent(Id, "Title"));

        return Result.Success();
    }

    public Result UpdateContent(string newContent, string? updatedBy = null)
    {
        var contentResult = NoteContent.Create(newContent);
        if (contentResult.IsFailure)
            return Result.Failure(contentResult.Error!);

        Content = contentResult.Data!;
        SetUpdatedInfo(updatedBy);
        AddDomainEvent(new NoteUpdatedEvent(Id, "Content"));

        return Result.Success();
    }

    public void Publish(string? updatedBy = null)
    {
        if (Status == NoteStatus.Published)
            return;

        Status = NoteStatus.Published;
        SetUpdatedInfo(updatedBy);
        AddDomainEvent(new NotePublishedEvent(Id));
    }

    public void Archive(string? updatedBy = null)
    {
        Status = NoteStatus.Archived;
        SetUpdatedInfo(updatedBy);
        AddDomainEvent(new NoteArchivedEvent(Id));
    }

    public void SetPriority(NotePriority priority, string? updatedBy = null)
    {
        if (Priority == priority)
            return;

        Priority = priority;
        SetUpdatedInfo(updatedBy);
    }

    public void ChangeCategory(Guid? categoryId, string? updatedBy = null)
    {
        CategoryId = categoryId;
        SetUpdatedInfo(updatedBy);
    }

    // Tag Management
    public void AddTag(Guid tagId)
    {
        if (_noteTags.Any(nt => nt.TagId == tagId))
            return;

        _noteTags.Add(NoteTag.Create(Id, tagId));
    }

    public void RemoveTag(Guid tagId)
    {
        var noteTag = _noteTags.FirstOrDefault(nt => nt.TagId == tagId);
        if (noteTag != null)
            _noteTags.Remove(noteTag);
    }

    public void ClearTags()
    {
        _noteTags.Clear();
    }

    // Soft Delete Implementation
    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        AddDomainEvent(new NoteDeletedEvent(Id));
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}

// Enums/NoteStatus.cs
public enum NoteStatus
{
    Draft = 0,
    Published = 1,
    Archived = 2
}

// Enums/NotePriority.cs
public enum NotePriority
{
    Low = 0,
    Normal = 1,
    High = 2,
    Urgent = 3
}
```

### Domain Events

```csharp
// Events/NoteCreatedEvent.cs
public sealed record NoteCreatedEvent(Guid NoteId, string Title) : DomainEvent;

// Events/NoteUpdatedEvent.cs
public sealed record NoteUpdatedEvent(Guid NoteId, string UpdatedField) : DomainEvent;

// Events/NoteDeletedEvent.cs
public sealed record NoteDeletedEvent(Guid NoteId) : DomainEvent;

// Events/NotePublishedEvent.cs
public sealed record NotePublishedEvent(Guid NoteId) : DomainEvent;

// Events/NoteArchivedEvent.cs
public sealed record NoteArchivedEvent(Guid NoteId) : DomainEvent;
```

### Domain Exceptions

```csharp
// Exceptions/DomainException.cs
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

// Exceptions/NoteNotFoundException.cs
public sealed class NoteNotFoundException : DomainException
{
    public Guid NoteId { get; }

    public NoteNotFoundException(Guid noteId) 
        : base($"یادداشت با شناسه {noteId} یافت نشد")
    {
        NoteId = noteId;
    }
}

// Exceptions/InvalidNoteException.cs
public sealed class InvalidNoteException : DomainException
{
    public InvalidNoteException(string message) : base(message)
    {
    }
}
```

### Specification Pattern (Advanced Query)

```csharp
// Specifications/ISpecification.cs
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    int? Take { get; }
    int? Skip { get; }
    bool IsPagingEnabled { get; }
}

// Specifications/BaseSpecification.cs
public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; private set; } = default!;
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int? Take { get; private set; }
    public int? Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected BaseSpecification()
    {
    }

    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}

// Specifications/NoteSpecifications.cs
public sealed class ActiveNotesSpecification : BaseSpecification<Note>
{
    public ActiveNotesSpecification() 
        : base(n => !n.IsDeleted && n.Status != NoteStatus.Archived)
    {
        AddInclude(n => n.Category!);
        AddInclude(n => n.NoteTags);
        ApplyOrderByDescending(n => n.CreatedAt);
    }
}

public sealed class NotesByCategorySpecification : BaseSpecification<Note>
{
    public NotesByCategorySpecification(Guid categoryId) 
        : base(n => !n.IsDeleted && n.CategoryId == categoryId)
    {
        AddInclude(n => n.Category!);
        AddInclude(n => n.NoteTags);
        ApplyOrderByDescending(n => n.CreatedAt);
    }
}

public sealed class NotesByPrioritySpecification : BaseSpecification<Note>
{
    public NotesByPrioritySpecification(NotePriority priority) 
        : base(n => !n.IsDeleted && n.Priority == priority)
    {
        AddInclude(n => n.Category!);
        ApplyOrderByDescending(n => n.CreatedAt);
    }
}

public sealed class PaginatedNotesSpecification : BaseSpecification<Note>
{
    public PaginatedNotesSpecification(int pageNumber, int pageSize) 
        : base(n => !n.IsDeleted)
    {
        AddInclude(n => n.Category!);
        ApplyOrderByDescending(n => n.CreatedAt);
        ApplyPaging((pageNumber - 1) * pageSize, pageSize);
    }
}
```

---

# 3. Application Layer - CQRS کامل

## 3.1 ساختار کامل Application

```
Notely.Application/
├── 📁 Common/
│   ├── 📁 Behaviors/                 # MediatR Pipelines
│   │   ├── 📄 ValidationBehavior.cs
│   │   ├── 📄 LoggingBehavior.cs
│   │   ├── 📄 PerformanceBehavior.cs
│   │   ├── 📄 CachingBehavior.cs
│   │   └── 📄 TransactionBehavior.cs
│   │
│   ├── 📁 Mappings/                  # Mapster Profiles
│   │   └── 📄 NoteProfile.cs
│   │
│   ├── 📁 Exceptions/
│   │   ├── 📄 ValidationException.cs
│   │   └── 📄 NotFoundException.cs
│   │
│   └── 📁 Models/
│       ├── 📄 PaginatedList.cs
│       └── 📄 Result.cs
│
├── 📁 Contracts/                     # ⭐ Interfaces
│   ├── 📁 Persistence/
│   │   ├── 📄 IApplicationDbContext.cs
│   │   ├── 📄 IUnitOfWork.cs
│   │   └── 📄 IRepository.cs
│   │
│   ├── 📁 Services/
│   │   ├── 📄 IDateTime.cs
│   │   ├── 📄 ICurrentUserService.cs
│   │   └── 📄 ICacheService.cs
│   │
│   └── 📁 Infrastructure/
│       ├── 📄 IEmailService.cs
│       └── 📄 INotificationService.cs
│
├── 📁 Features/                      # ⭐ Feature-based
│   ├── 📁 Notes/
│   │   ├── 📁 Commands/
│   │   │   ├── 📁 CreateNote/
│   │   │   │   ├── 📄 CreateNoteCommand.cs
│   │   │   │   ├── 📄 CreateNoteCommandHandler.cs
│   │   │   │   └── 📄 CreateNoteCommandValidator.cs
│   │   │   │
│   │   │   ├── 📁 UpdateNote/
│   │   │   │   ├── 📄 UpdateNoteCommand.cs
│   │   │   │   ├── 📄 UpdateNoteCommandHandler.cs
│   │   │   │   └── 📄 UpdateNoteCommandValidator.cs
│   │   │   │
│   │   │   ├── 📁 DeleteNote/
│   │   │   │   ├── 📄 DeleteNoteCommand.cs
│   │   │   │   ├── 📄 DeleteNoteCommandHandler.cs
│   │   │   │   └── 📄 DeleteNoteCommandValidator.cs
│   │   │   │
│   │   │   ├── 📁 PublishNote/
│   │   │   └── 📁 ArchiveNote/
│   │   │
│   │   ├── 📁 Queries/
│   │   │   ├── 📁 GetAllNotes/
│   │   │   │   ├── 📄 GetAllNotesQuery.cs
│   │   │   │   ├── 📄 GetAllNotesQueryHandler.cs
│   │   │   │   └── 📄 GetAllNotesQueryValidator.cs
│   │   │   │
│   │   │   ├── 📁 GetNoteById/
│   │   │   ├── 📁 GetNotesByCategory/
│   │   │   ├── 📁 GetNotesByPriority/
│   │   │   └── 📁 SearchNotes/
│   │   │
│   │   └── 📁 EventHandlers/        # ⭐ Domain Event Handlers
│   │       ├── 📄 NoteCreatedEventHandler.cs
│   │       └── 📄 NoteDeletedEventHandler.cs
│   │
│   ├── 📁 Categories/
│   └── 📁 Tags/
│
└── 📄 DependencyInjection.cs
```

## 3.2 MediatR Behaviors (Pipeline)

### Validation Behavior (بهبود یافته)

```csharp
// Common/Behaviors/ValidationBehavior.cs
public sealed class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning(
                "Validation failed for {RequestType}. Errors: {