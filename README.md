# Library Management System

A modern library management system built with Clean Architecture principles using .NET 9, Entity Framework Core, and a
React frontend.

## 🏗️ Architecture

This project follows **Clean Architecture** (Hexagonal Architecture) principles with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                       │
│  ┌─────────────────────┐  ┌─────────────────────────────────┐│
│  │   React Frontend    │  │      ASP.NET Core API          ││
│  │   (Port: 5173)      │  │      (Port: 7280)              ││
│  └─────────────────────┘  └─────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                    Application Layer                        │
│  ┌─────────────────────────────────────────────────────────┐│
│  │   CQRS + MediatR + FluentValidation                    ││
│  │   • Commands (Create, Update, Delete)                  ││
│  │   • Queries (GetAll, GetById)                          ││
│  │   • DTOs & Validation                                  ││
│  └─────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                      Domain Layer                           │
│  ┌─────────────────────────────────────────────────────────┐│
│  │   • Entities (Book, Patron)                            ││
│  │   • Value Objects (BookId, PatronId, Money)            ││
│  │   • Domain Events                                      ││
│  │   • Business Rules & Invariants                        ││
│  │   • Repository Interfaces                              ││
│  └─────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                      │
│  ┌─────────────────────────────────────────────────────────┐│
│  │   • Entity Framework Core                              ││
│  │   • SQL Server Database                                ││
│  │   • Repository Implementations                         ││
│  │   • Data Persistence                                   ││
│  └─────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
```

### Key Architectural Patterns

- **Clean Architecture**: Clear separation of concerns with dependency inversion
- **CQRS (Command Query Responsibility Segregation)**: Separate models for reading and writing
- **MediatR**: Request/response handling and cross-cutting concerns
- **Repository Pattern**: Data access abstraction
- **Domain-Driven Design**: Rich domain models with business logic
- **Event-Driven Architecture**: Domain events for side effects

## 🚀 Features

### 📚 Book Management

- **Create Books**: Add new books with title, author, ISBN, and restricted access flag
- **Update Books**: Modify existing book information
- **Delete Books**: Remove books from the system
- **Search Books**: Find books by various criteria
- **Book Status Tracking**: Available, On Hold, Checked Out

### 👥 Patron Management

- **Register Patrons**: Create new library members
- **Patron Types**: Regular patrons and Researchers (with special privileges)
- **Update Profiles**: Modify patron information
- **Account Status**: Active/inactive patron management
- **Fee Management**: Track and manage outstanding fees

### 📖 Library Operations

- **Place Holds**: Reserve books for future checkout
- **Check Out Books**: Borrow books with due dates
- **Return Books**: Process book returns
- **Hold Management**: Automatic hold expiry handling
- **Overdue Tracking**: Calculate overdue fees ($0.50/day)
- **Restricted Access**: Special books for researchers only

### 🔐 Business Rules

- Only available books can be placed on hold
- Books on hold can only be checked out by the holder
- Automatic 2-week checkout period
- Overdue fee calculation
- Researchers can access restricted books
- Email uniqueness validation

## 🛠️ Technology Stack

### Backend (.NET 9)

- **Framework**: ASP.NET Core 9.0
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Architecture**: Clean Architecture with CQRS
- **Validation**: FluentValidation
- **API Documentation**: Swagger/OpenAPI
- **Containerization**: Docker

### Frontend (React + TypeScript)

- **Framework**: React 18 with TypeScript
- **Build Tool**: Vite
- **Styling**: Tailwind CSS
- **UI Components**: shadcn/ui
- **HTTP Client**: Axios/Fetch API
- **Port**: 5173 (development)

### Database

- **SQL Server**: Running in Docker container
- **Port**: 1433
- **Features**: Entity Framework migrations, relationships, constraints

## 🐳 Docker Setup

### Services Overview

The application runs in a multi-container Docker setup:

```yaml
services:
  # SQL Server Database
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: library-management-db
    ports: ["1433:1433"]
    
  # .NET API
  library-api:
    build: .
    container_name: library-management-api
    ports: ["5000:8080", "5001:8081"]
    depends_on: [sqlserver]
```

### Container Details

| Service | Container Name | Ports | Description |
|---------|---------------|-------|-------------|
| **SQL Server** | `library-management-db` | `1433:1433` | Microsoft SQL Server 2022 Express |
| **API** | `library-management-api` | `5000:8080`, `5001:8081` | .NET 9 Web API |
| **Frontend** | *(separate)* | `5173:5173` | React development server |

### Environment Variables

```bash
# Database
ACCEPT_EULA=Y
SA_PASSWORD=Password123
MSSQL_PID=Express

# API
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LibraryManagementDB;User=sa;Password=YourStrong@Password123;TrustServerCertificate=true;
```

## 🚀 Getting Started

### Prerequisites

- Docker & Docker Compose
- .NET 9 SDK (for local development)
- Node.js 18+ (for frontend development)

### Option 1: Docker Compose (Recommended)

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd library-management
   ```

2. **Start all services**
   ```bash
   docker-compose up -d
   ```

3. **Access the applications**
    - API: http://localhost:5000
    - Swagger UI: http://localhost:5000/swagger
    - Database: localhost:1433 (sa/Password123)

### Option 2: Local Development

1. **Start SQL Server**
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
   ```

2. **Run Database Migrations**
   ```bash
   cd LiBraryManagement.Api
   dotnet ef database update
   ```

3. **Start the API**
   ```bash
   cd LiBraryManagement.Api
   dotnet run
   ```

4. **Start the Frontend**
   ```bash
   cd library-management-frontend
   npm install
   npm run dev
   ```

## 📡 API Endpoints

### Books

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/books` | Get all books |
| `GET` | `/api/books/{id}` | Get book by ID |
| `POST` | `/api/books` | Create new book |
| `PUT` | `/api/books/{id}` | Update book |
| `DELETE` | `/api/books/{id}` | Delete book |
| `POST` | `/api/books/{id}/hold` | Place book on hold |
| `POST` | `/api/books/{id}/checkout` | Check out book |
| `POST` | `/api/books/{id}/return` | Return book |

### Patrons

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/patrons` | Get all patrons |
| `GET` | `/api/patrons/{id}` | Get patron by ID |
| `POST` | `/api/patrons` | Create new patron |
| `PUT` | `/api/patrons/{id}` | Update patron |
| `DELETE` | `/api/patrons/{id}` | Deactivate patron |
| `GET` | `/api/patrons/{id}/holds` | Get patron's holds |

## 🗄️ Database Schema

### Key Entities

```sql
-- Books Table
Books: Id, BookId, Title, Author, ISBN, Status, IsRestrictedAccess, 
       CurrentHolderId, CurrentBorrowerId, HoldExpiryDate, 
       CheckoutDate, DueDate, CreatedAt, UpdatedAt

-- Patrons Table  
Patrons: Id, PatronId, Name, Email, Type, OutstandingFees_Amount,
         OutstandingFees_Currency, IsActive, CreatedAt, UpdatedAt
```

### Relationships

- Books can have zero or one current holder (PatronId)
- Books can have zero or one current borrower (PatronId)
- Patrons can have multiple books on hold or checked out

## 🔧 Configuration

### CORS Settings

The API is configured to accept requests from  frontend port:

- `http://localhost:5173` (Vite default)

### Database Connection

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=LibraryManagementDB;User=sa;Password=Password123;TrustServerCertificate=true;"
  }
}
```


## 📞 Support

For support and questions, please open an issue in the GitHub repository.

---

**Built with ❤️ using Clean Architecture principles**