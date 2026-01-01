# Master Architecture Plan – پروژه Notely

این سند یک **نقشه‌راه معماری نهایی (Architecture Blueprint)** است؛ نه آموزش، نه کد نویسی. هدف این فایل این است که دقیقاً بدانی:

* هر لایه چه مسئولیتی دارد
* چه فایل‌ها و پوشه‌هایی باید وجود داشته باشند
* چه چیزی «درست» است و چه چیزی «اشتباه»
* برای پروژه فعلی تو چه انتخابی منطقی، ساده و performant است

این نسخه بر اساس:

* Microsoft .NET Architecture Guidance
* Jason Taylor – Clean Architecture
* Steve Smith (Ardalis)
* Jimmy Bogard (CQRS / MediatR)
* Blazor Server Best Practices (2024–2025)

---

## 1. تصمیمات معماری قطعی (Non‑Negotiable)

### 1.1 نوع معماری کلی

**Clean Architecture + CQRS (MediatR)**

* Business Logic کاملاً مستقل از UI
* Infrastructure فقط implementation است
* UI فقط orchestrator است

> این تصمیم درست است و تغییر نمی‌کند.

---

### 1.2 انتخاب تکنولوژی Web (خیلی مهم)

برای پروژه فعلی تو:

✅ **Blazor Server فقط**

❌ Blazor WebAssembly
❌ Blazor Hybrid

**چرا؟**

* اپلیکیشن Note‑Taking است، نه PWA
* Offline نیاز ندارد
* Security مهم‌تر از Client power است
* سادگی و Maintainability اولویت است

> Hybrid فقط complexity اضافه می‌کند و الان هیچ ارزشی ندارد.

---

## 2. ساختار نهایی Solution (Final Shape)

```
Notely/
├── Notely.sln
│
├── src/
│   ├── Core/
│   │   ├── Notely.Domain/
│   │   └── Notely.Application/
│   │
│   ├── Infrastructure/
│   │   └── Notely.Infrastructure/
│   │
│   ├── Presentation/
│   │   └── Notely.Web/        ← Blazor Server
│   │
│   └── Shared/
│       └── Notely.Shared/
│
└── tests/
```

**قانون طلایی:**

> UI هیچ وقت به Infrastructure reference مستقیم ندارد.

---

## 3. Domain Layer – دقیقاً چه چیزی اینجا باشد؟

### 3.1 مسئولیت Domain

Domain فقط و فقط:

* Business Rules
* Invariants
* رفتار (Behavior)

Domain نباید بداند:

* Database چیست
* EF Core چیست
* MediatR چیست
* UI چیست

---

### 3.2 ساختار Domain (استاندارد حرفه‌ای)

```
Notely.Domain/
├── Entities/
├── ValueObjects/
├── Events/
├── Enums/
├── Exceptions/
├── Specifications/
└── Common/
```

**توضیح خیلی مهم:**

* Entity = رفتار دارد
* ValueObject = Validation + Meaning
* Domain Event = واکنش به تغییرات مهم

اگر Domain فقط property دارد → Domain ضعیف است.

---

## 4. Application Layer – مغز سیستم

### 4.1 مسئولیت Application

Application:

* Use caseها را اجرا می‌کند
* CQRS را مدیریت می‌کند
* Transaction boundary است

Application نباید:

* UI logic داشته باشد
* Database implementation بداند

---

### 4.2 ساختار Application (Best Practice)

```
Notely.Application/
├── Common/
│   ├── Behaviors/
│   ├── Exceptions/
│   └── Models/
│
├── Contracts/
│   ├── Persistence/
│   ├── Services/
│   └── Infrastructure/
│
├── Features/
│   ├── Notes/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   └── EventHandlers/
│   ├── Categories/
│   └── Tags/
│
└── DependencyInjection.cs
```

**قانون:**

> Feature‑based همیشه بهتر از Layer‑based داخل Application است.

---

## 5. Infrastructure Layer – فقط Implementation

### 5.1 مسئولیت Infrastructure

* EF Core
* Repository implementation
* External services
* Cache
* File system

Infrastructure هیچ Business Rule ندارد.

---

### 5.2 ساختار Infrastructure

```
Notely.Infrastructure/
├── Persistence/
│   ├── DbContext
│   ├── Configurations
│   └── Migrations
│
├── Repositories/
├── Caching/
├── Identity/
└── DependencyInjection.cs
```

**نکته مهم:**

* Queryها لزوماً Repository نمی‌خواهند
* Command side Repository OK

---

## 6. Shared Layer – چرا و چه زمانی؟

Shared فقط برای:

* DTO مشترک
* Result / ApiResponse
* Constants

اگر چیزی فقط مخصوص UI یا Application است → Shared نکن.

---

## 7. Web Layer (Blazor Server) – راهنمای کامل

### 7.1 مسئولیت Web Layer

Web Layer:

* Rendering UI
* Orchestration
* State management سبک

Web Layer نباید:

* Business logic داشته باشد
* Validation business انجام دهد

---

### 7.2 ساختار استاندارد Blazor Server

```
Notely.Web/
├── Components/
│   ├── Layout/
│   ├── Shared/
│   └── Features/
│       └── Notes/
│           ├── NoteList.razor
│           ├── NoteEditor.razor
│           └── NoteCard.razor
│
├── Pages/
│   ├── Notes/
│   └── Home.razor
│
├── Services/          ← خیلی مهم
│   ├── AppServices/
│   ├── State/
│   └── Notifications/
│
├── wwwroot/
└── Program.cs
```

---

### 7.3 Blazor باید با چی حرف بزند؟

#### انتخاب درست برای پروژه تو:

✅ **Application Service (Facade)**

```
Blazor Component
   ↓
AppService
   ↓
MediatR
```

❌ HttpClient (برای Blazor Server)
❌ MediatR مستقیم در UI

**چرا؟**

* تست‌پذیری بالا
* UI ساده
* تغییر معماری راحت

---

### 7.4 State Management در Blazor

حداقل لازم:

* Feature State (مثلاً NotesState)
* No global chaos

بدون Redux و پیچیدگی.

---

## 8. Performance Rules (اجباری)

* Queries: AsNoTracking
* Projection قبل از ToList
* Pagination اجباری
* Cache برای Read-heavy queries

UI:

* Virtualize lists
* Avoid re-render loops

---

## 9. برنامه اجرای تغییرات (Step Plan)

### Phase 1 – Web Layer Stabilization (اولین و مهم‌ترین مرحله)

> هدف این مرحله: **UI ساده، قابل فهم، تست‌پذیر و بدون وابستگی اشتباه**

#### 9.1 تصمیمات قطعی Web Layer

* ✔ فقط **Blazor Server**
* ❌ حذف Blazor WebAssembly Client
* ❌ عدم استفاده از HttpClient
* ❌ عدم استفاده مستقیم از MediatR در Component
* ✔ استفاده از **Application Facade (AppService)**

---

#### 9.2 ساختار نهایی Web Layer

```
Notely.Web/
├── Components/
│   ├── Layout/
│   ├── Shared/
│   └── Features/
│       └── Notes/
│           ├── NoteList.razor
│           ├── NoteEditor.razor
│           └── NoteCard.razor
│
├── Pages/
│   ├── Notes/
│   │   ├── Index.razor
│   │   ├── Create.razor
│   │   └── Edit.razor
│   └── Home.razor
│
├── Services/
│   ├── AppServices/        # Facade به Application
│   │   └── INotesAppService.cs
│   │   └── NotesAppService.cs
│   ├── State/              # UI State
│   │   └── NotesState.cs
│   └── Notifications/
│       └── ToastService.cs
│
├── wwwroot/
├── Program.cs
└── DependencyInjection.cs
```

---

#### 9.3 مسئولیت هر بخش (خیلی مهم)

* **Pages**

  * فقط routing و orchestration
  * هیچ logic سنگین

* **Components**

  * Reusable UI
  * Stateless یا state خیلی محدود

* **Services/AppServices**

  * تنها نقطه ارتباط UI با Application
  * MediatR فقط اینجا استفاده می‌شود

* **Services/State**

  * نگه‌داری state صفحه
  * جلوگیری از re-fetch بی‌دلیل

---

#### 9.4 قوانینی که نباید شکسته شوند

* Blazor Component نباید Command یا Query بشناسد
* Blazor نباید Validation business انجام دهد
* هیچ EF یا DbContext در Web
* هیچ Repository در Web

---

#### 9.5 خروجی قابل قبول Phase 1

در پایان Phase 1 باید:

* Web Layer فقط Blazor Server باشد
* ساختار فولدرها دقیقاً مشخص باشد
* AppService interfaces تعریف شده باشند (بدون پیاده‌سازی کامل)
* هیچ کدی به Application دست نزده باشد

---

### Phase 2

* Rich Domain

### Phase 3

* Application CQRS Cleanup

---

## 10. نتیجه نهایی

این ساختار:

* ساده است
* سریع است
* تست‌پذیر است
* scalable است
* دقیقاً مطابق پروژه‌های حرفه‌ای است

---

**قدم بعدی:**
در پیام بعدی، فقط یکی را انتخاب کن:

1. فقط Web Layer را نهایی کنیم
2. فقط Domain را migrate کنیم
3. فقط Application CQRS را تمیز کنیم

قدم‌به‌قدم جلو می‌رویم، بدون شلوغ‌کاری.


عالی 👌
پس **Phase 1 (مرحله اول)** رو رسماً شروع کردیم و من دقیقاً همون کاری رو که خواستی انجام دادم.

---

## الان چه اتفاقی افتاد؟

من داخل فایل **`Notely – Master Architecture Plan`** بخش **Phase 1 – Web Layer Stabilization** رو اضافه و نهایی کردم.
این بخش الان:

* ❌ آموزش Blazor نیست
* ❌ کدنویسی نیست
* ✅ **نقشه اجرایی دقیق** است

یعنی دقیقاً می‌دانی:

* چه پوشه‌هایی باید داشته باشی
* چه فایل‌هایی باید ساخته شوند
* هر فایل چه مسئولیتی دارد
* چه چیزهایی «ممنوع» هستند

---

## خلاصه Phase 1 به زبان خیلی ساده

### 🎯 هدف Phase 1

> Blazor تو باید **احمق ولی خوشحال** باشد 😄
> یعنی:

* فقط UI
* فقط orchestration
* هیچ فهمی از CQRS، EF، Repository، Validation

---

### 🔴 بزرگ‌ترین اصلاح این مرحله

**قبل:**

```
Blazor → MediatR → Application
```

**بعد (درست):**

```
Blazor → AppService → MediatR → Application
```

Blazor فقط AppService را می‌شناسد.
همین.

---

### 📁 ساختار نهایی Web Layer (قطعی)

این ساختار الان **استاندارد حرفه‌ای** است و دیگر تغییر نمی‌کند:

```
Notely.Web/
├── Components/
├── Pages/
├── Services/
│   ├── AppServices/
│   ├── State/
│   └── Notifications/
├── wwwroot/
└── Program.cs
```

اگر جایی از پروژه‌ات خلاف این بود → Technical Debt محسوب می‌شود.

---

## الان تو دقیقاً چه کاری باید بکنی؟ (بدون کدنویسی)

### ✅ Step 1 (الان)

فقط این کارها را انجام بده:

1. **Blazor WASM Client را حذف کن**
2. فولدرهای زیر را **خالی** بساز:

   * `Services/AppServices`
   * `Services/State`
3. ساختار فولدرها را با فایل مقایسه کن

❌ هیچ Handler
❌ هیچ Command
❌ هیچ Query
❌ هیچ MediatR در UI

---

## قدم بعدی چی می‌شود؟

در پیام بعدی یکی از این‌ها را بگو:

### گزینه A (من پیشنهاد می‌کنم):

👉 **AppService دقیقاً چیست و چه متدهایی باید داشته باشد؟**
(بدون پیاده‌سازی، فقط Interface design)

### گزینه B:

👉 **State در Blazor دقیقاً چطور باید باشد؟**
(چه stateهایی، کجا نگه داریم، چه زمانی refresh شود)

### گزینه C:

👉 **Program.cs و DI Web Layer را نهایی کنیم**

فقط یکی را انتخاب کن.
ما داریم پروژه را **مهندسی می‌کنیم، نه حدس می‌زنیم**.
