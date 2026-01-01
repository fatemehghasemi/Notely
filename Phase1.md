خیلی عالی؛ دقیقاً داری کار درستِ یک **Lead Engineer** رو انجام می‌دی 👍
پس من الان نقش «معمار → دستور اجرایی برای AI IDE» رو می‌گیرم.

در این پیام:

* فقط **Phase 1 / Step 1** رو می‌دیم
* متن به‌صورت **Instruction قابل کپی** است
* بدون تحلیل اضافه، بدون آموزش، بدون کد
* مناسب feed کردن به AI Agent داخل IDE

---

## 🧠 Instruction برای AI IDE – Phase 1 / Step 1

**Commit: `chore(web): stabilize blazor web layer structure`**

> Goal: Stabilize Web Layer architecture according to Clean Architecture & Blazor best practices.
> Do NOT implement business logic. Only structural changes.

---

### 1️⃣ Blazor Hosting Model

* Ensure the solution uses **Blazor Server only**
* Remove / exclude **Blazor WebAssembly client project** if present
* Do NOT introduce hybrid or shared UI projects

---

### 2️⃣ Web Project Folder Structure

Create or refactor the Web project to match **exactly** this structure:

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
│   ├── AppServices/
│   │   └── INotesAppService.cs   (interface only, no logic)
│   │   └── NotesAppService.cs    (empty or throw NotImplementedException)
│   │
│   ├── State/
│   │   └── NotesState.cs         (empty state class, no logic yet)
│   │
│   └── Notifications/
│       └── ToastService.cs       (empty placeholder)
│
├── wwwroot/
├── DependencyInjection.cs
└── Program.cs
```

---

### 3️⃣ Strict Architectural Rules (Must Enforce)

* Blazor Pages & Components:

  * MUST NOT reference:

    * MediatR
    * Commands / Queries
    * DbContext
    * Repositories
    * HttpClient
* UI may only depend on:

  * AppServices
  * UI State classes
* No business validation in UI
* No data access logic in Web layer

---

### 4️⃣ AppService Boundary (Important)

* AppServices act as **facade between UI and Application**
* MediatR (if used) must be used **only inside AppServices**
* UI must never know CQRS concepts

---

### 5️⃣ Expected Output of This Step

After completion:

* Web layer compiles
* Folder structure matches exactly
* No functional behavior added
* AppService interfaces exist but are not implemented
* This is a **structural commit only**

---

## ⛔ Explicitly Out of Scope

* No CQRS implementation
* No MediatR wiring
* No business logic
* No EF Core usage
* No performance optimizations yet

---

## ✅ Stop After This Step

Once done:

* Stop
* Do NOT proceed to next phase
* Wait for next instruction

---
