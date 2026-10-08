# Neoxis Authentication App

A professional authentication web application built with **ASP.NET Core 8**, **ASP.NET Core Identity**, **Entity Framework Core**, and **PostgreSQL (Supabase)**.

The application provides user registration, email/password login, **Sign in with Google**, **Sign in with Microsoft**, logout, protected dashboard, and profile functionality. The UI is integrated with the **CoolAdmin** Bootstrap admin dashboard template.

---

## Features

- **User Registration** — Full Name, Email, Password, Confirm Password
- **Email/Password Login** — with Remember Me support
- **Sign in with Google** — OAuth via ASP.NET Core external authentication
- **Sign in with Microsoft** — OAuth via ASP.NET Core external authentication
- **Account Linking** — External OAuth logins are linked to existing Identity accounts automatically
- **Protected Dashboard & Profile** — Server-side authorization; unauthenticated users are redirected to login
- **User-Specific Dashboard** — Dynamically displays user name, email, account status, and registration date
- **Secure Password Hashing** — Uses standard ASP.NET Core Identity PBKDF2 hashing; passwords are never stored in plain text
- **CSRF Protection** — Anti-forgery tokens on all sensitive POST forms
- **Database Persistence** — Supabase PostgreSQL via Entity Framework Core with automatic migrations on startup
- **Docker Support** — Multi-stage Dockerfile for containerized deployment
- **Cloud Deployment** — Continuous deployment via Render

---

## Technologies Used

| Technology | Purpose |
|---|---|
| **ASP.NET Core 8 MVC** | Web backend framework |
| **ASP.NET Core Identity** | Authentication, password hashing, and user management |
| **Entity Framework Core 8** | ORM and database migrations |
| **Npgsql EF Core Provider** | PostgreSQL database provider |
| **Microsoft.AspNetCore.Authentication.Google** | Google OAuth authentication handler |
| **Microsoft.AspNetCore.Authentication.MicrosoftAccount** | Microsoft OAuth authentication handler |
| **Supabase** | Managed cloud PostgreSQL database |
| **CoolAdmin (Colorlib)** | Bootstrap 5 admin dashboard UI template |
| **Docker** | Containerization for deployment |
| **Render** | Cloud hosting web service |

---

## Project Structure

```text
NeoxisAuthApp/
│
├── Controllers/
│   ├── AccountController.cs    # Login, Register, Logout, External OAuth callbacks
│   ├── DashboardController.cs  # Protected Dashboard & Profile
│   └── HomeController.cs       # Root redirection
├── Data/
│   └── ApplicationDbContext.cs # EF Core DbContext for Identity
├── Migrations/                 # EF Core database migrations
├── Models/
│   └── ApplicationUser.cs      # Custom Identity user model (FullName, CreatedAt)
├── ViewModels/                 # Login & Register view models
├── Views/
│   ├── Account/                # Login, Register, AccessDenied
│   ├── Dashboard/              # Protected Index & Profile views
│   └── Shared/                 # _Layout, _AuthLayout
├── wwwroot/                    # CoolAdmin assets (CSS, JS, images, fonts, vendors)
├── Dockerfile                  # Multi-stage Docker build file
├── Program.cs                  # Service configuration, Identity, OAuth, Middleware
└── appsettings.json            # Configuration and connection strings
```

---

## Required Environment Variables (Production / Render)

| Variable | Description | Where to set |
|---|---|---|
| `DATABASE_URL` | Supabase PostgreSQL connection URI | Render → Environment |
| `GOOGLE_CLIENT_ID` | Google OAuth Client ID | Render → Environment |
| `GOOGLE_CLIENT_SECRET` | Google OAuth Client Secret | Render → Environment |
| `MICROSOFT_CLIENT_ID` | Microsoft Entra/Azure App Client ID | Render → Environment |
| `MICROSOFT_CLIENT_SECRET` | Microsoft Entra/Azure App Client Secret | Render → Environment |

> **Security Note:** Never commit database credentials or OAuth secrets into GitHub. In Render, provide them under the **Environment** settings. The application boots and operates normally even if OAuth credentials are not yet set.

---

## OAuth Callback URLs

Configure these exact redirect URIs in each respective provider console:

### Google (Google Cloud Console → Credentials → OAuth 2.0 Client)
- **Local:** `http://localhost:5000/signin-google`
- **Production:** `https://your-app-name.onrender.com/signin-google`

### Microsoft (Azure Portal → App Registrations → Authentication)
- **Local:** `http://localhost:5000/signin-microsoft`
- **Production:** `https://your-app-name.onrender.com/signin-microsoft`

---

## Running Locally

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL or Supabase instance
- Git

### Steps
1. **Clone the repository:**
   ```bash
   git clone https://github.com/lisaajose/sign_up.git
   cd sign_up
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Configure local database:**
   Update the `DefaultConnection` string in `appsettings.json`, then apply migrations:
   ```bash
   dotnet ef database update
   ```

4. **Run the app:**
   ```bash
   dotnet run --urls "http://localhost:5000"
   ```

---

## Deployment Architecture

```text
GitHub (sign_up repo)
      ↓
Render (Docker Web Service)
      ↓
ASP.NET Core 8 Web App
      ↓
Supabase (Cloud PostgreSQL)
```

1. Code pushed to the `main` branch triggers an automatic build on **Render**.
2. Render builds the Docker image and launches the application container.
3. On startup, `Program.cs` automatically applies any pending database migrations to Supabase.

---

## Author

**Lisa Ann Jose**  
GitHub: [https://github.com/lisaajose](https://github.com/lisaajose)  
Repository: [https://github.com/lisaajose/sign_up](https://github.com/lisaajose/sign_up)

---

## License & Attribution

UI template: **CoolAdmin** by [Colorlib](https://colorlib.com).  
Copyright © Colorlib. All rights reserved.
