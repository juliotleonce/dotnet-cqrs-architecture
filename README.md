# .Net CQRS Architecture

## 🧪 Architecture Experimentation Project

This repository is a dedicated space for **architectural experimentation** and exploring modern software design patterns within the .NET ecosystem. It is not intended for production use but serves as a playground for implementing and validating various architectural styles and pragmatic engineering decisions.

The project simulates a Wallet System (Fintech) to provide a non-trivial domain where complex business rules and transaction integrity are paramount.

---

## 🛠 Tech Stack

- **Language:** C# 14
- **Framework:** .NET 10 (ASP.NET Core)
- **Database:** PostgreSQL (via Entity Framework Core)
- **Documentation:** Swagger
- **Caching:** Redis (Distributed Caching)
- **Messaging/Internal Dispatch:** MediatR (CQRS and Domain Events)
- **Security:** JWT Bearer Authentication, BCrypt for hashing
- **External Integrations:** Vonage/Orange SMS (Abstraction-based)
- **Containerization:** Docker & Docker Compose

---

## 🏛 Architectural Patterns & Implementation

The project follows a **Hexagonal Architecture** (Ports & Adapters) approach combined with **Domain-Driven Design (DDD)** principles.

### 1. Pragmatic Hexagonal Architecture
While Hexagonal Architecture (Ports & Adapters) often suggests strict physical separation via multiple projects/assemblies, this project takes a **pragmatic single-project approach**:
- **Folder-based Separation:** Layers are separated by folders rather than projects to reduce boilerplate and complexity during experimentation.
- **Dependency Flow:** Even within a single project, we strive to maintain the core rule: dependencies point inwards toward the Domain.
- **Layers:**
    - **Domain:** Pure business logic, entities, and domain exceptions. No dependencies on external libraries.
    - **Application:** Orchestrates business flow using Commands/Queries.
    - **Infrastructure:** Implementation of "Adapters" (Repositories, External Services, DB Context).
    - **Presentation:** Entry points (Controllers, Middlewares).

### 2. CQRS with MediatR
We use the Command Query Responsibility Segregation pattern. 
- Commands and Handlers are separated into distinct folders within the Application layer.
- **Pragmatic Decision:** MediatR is used as the in-process bus to decouple the "What needs to be done" from the "How it is executed".

### 3. Pragmatic Domain Entities (Aggregate Roots)
While DDD purism suggests that the Domain layer should be entirely free of infrastructure concerns (like EF Core attributes), this project adopts a **pragmatic approach to entities**:
- **Inline Mapping:** We use Data Annotations (`[MaxLength]`, `[Index]`) and sometimes EF Core specific attributes directly within Domain entities.
- **Reasoning:** This reduces the need for extensive Fluent API configurations in the Infrastructure layer, keeping the "experimentation" focused on logic rather than boilerplate mapping code.
- **Base Class:** `AggregatRoot.cs` provides a base for entities to collect domain events during business operations.
- **Domain Events:** Instead of triggering side effects immediately (like sending an SMS), the Domain layer raises events (e.g., `UserCreatedEvent`). These are dispatched only after a successful database transaction.

### 4. Pragmatic Unit of Work (UoW)
The `UnitOfWork` implementation in `Infrastructure/Database` is a key experimentation point:
- It wraps `DbContext.SaveChangesAsync()`.
- **Automatic Event Dispatch:** It scans the ChangeTracker for `AggregatRoot` entities, extracts their collected events, and dispatches them via MediatR *only after* the database changes are committed. This ensures consistency between the state and the side effects.

### 5. Event Flow: A DDD Example
To illustrate how these concepts work together, here is the lifecycle of a **User Sign-up** operation:
1.  **Application Layer:** `SignUpHandler` receives a `SignUpCommand`.
2.  **Domain Layer:** It calls `WalletUser.Create(...)`. The entity validates business rules and registers a `UserCreatedEvent` internally using `AddDomainEvent()`.
3.  **Infrastructure Layer:** The handler calls `unitOfWork.CommitAndDispatchEventsAsync()`.
    - `DbContext.SaveChangesAsync()` is called to persist the user.
    - If successful, the `UnitOfWork` scans the ChangeTracker for events.
    - `UserCreatedEvent` is published via **MediatR**.
4.  **Side Effects (Application Layer):** `AccountVerificationRequiredEventHandler` catches the event and triggers a new command to send an OTP via SMS.

This flow ensures that side effects (like sending SMS) only happen if the database transaction is successful, maintaining eventual consistency.

### 6. Ports & Adapters for External Services
External services like SMS are hidden behind interfaces (`ISmsService`). This allows:
- Easy swapping between providers (Vonage, Orange).
- Simple `MockSmsService` for local development without consuming credits.

---

## 📁 Project Structure

```text
├── Application/         # Orchestration (Commands, Handlers, EventHandlers)
├── Domain/              # Business Logic (Entities, Events, Ports/Interfaces)
├── Infrastructure/      # Technical Details (DB, Repositories, Security, External APIs)
├── Presentation/        # Web API (Controllers, Middleware)
├── Migrations/          # EF Core Database Migrations
├── Program.cs           # Composition Root / Dependency Injection
├── compose.yaml         # Docker orchestration (DB, Redis)
└── .env.example         # Environment variables template
```

---

## 🚀 Testing the project

### Requirements
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) or Docker Engine
- A tool like `Postman` or `curl` (or use the built-in Swagger UI)

### Setup & Run

1. **Clone the repository**
   ```bash
   git clone <repo-url>
   cd dotnet-cqrs-architecture
   ```

2. **Configure Environment**
   Copy `.local.example.env` to `.env` and fill in the values.
   ```bash
   cp .local.example.env .env
   ```

3. **Start Infrastructure (PostgreSQL & Redis)**
   ```bash
   docker-compose up -d
   ```

4. **Apply Migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the API**
   ```bash
   dotnet run
   ```
   The API will be available at `http://localhost:5000` (or as configured). Swagger UI is accessible at `/swagger`.

---

## 📄 License

This project is open-source and intended for educational/experimental purposes. Check the `LICENSE` file for details (MIT suggested).
