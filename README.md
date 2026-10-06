# An Auth App

A basic authentication web application built with ASP.NET Core 8, ASP.NET Core Identity, Entity Framework Core, and PostgreSQL.

The application provides user registration, login, logout, a protected dashboard, and profile functionality. The UI is based on the CoolAdmin Bootstrap admin dashboard template.

## Features

- User registration
- User login and logout
- Remember Me functionality
- Password validation
- Secure password hashing using ASP.NET Core Identity
- Protected dashboard and profile pages
- User-specific dashboard information
- PostgreSQL database
- Entity Framework Core migrations
- Responsive UI
- Docker support
- Render deployment

## Technologies Used

- ASP.NET Core 8
- C#
- ASP.NET Core Identity
- Entity Framework Core 8
- PostgreSQL
- Npgsql
- Bootstrap 5
- CoolAdmin
- Docker
- Render
- Git / GitHub

## Project Structure


NeoxisAuthApp/
│
├── Controllers/
├── Data/
├── Migrations/
├── Models/
├── ViewModels/
├── Views/
├── wwwroot/
├── Program.cs
├── appsettings.json
├── Dockerfile
└── NeoxisAuthApp.csproj

Authentication
Authentication is implemented using ASP.NET Core Identity.
Users can:
- Create an account using their name, email, and password
- Log in using their registered credentials
- Stay signed in using Remember Me
- Log out of their account
- Access protected pages only after authentication
Passwords are handled by ASP.NET Core Identity and are not stored as plain text.
Database
The application uses PostgreSQL with Entity Framework Core.
ASP.NET Core Identity manages the user-related tables, including:
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- AspNetUserLogins
- AspNetUserTokens
Database migrations are included in the project.
Configuration
The application uses a PostgreSQL connection string provided through environment variables in the production environment.
For local development, configure the database connection in your local application settings.
Example:
Host=localhost;
Port=5432;
Database=NeoxisAuth;
Username=postgres;
Password=your-password;

Production database credentials should not be committed to the repository.
Running Locally
Prerequisites
- .NET 8 SDK
- PostgreSQL
- Git
Clone the repository
git clone https://github.com/lisaajose/sign_up.git
cd sign_up

Restore dependencies
dotnet restore

Apply database migrations
dotnet ef database update

Run the application
dotnet run

The application will be available at the local URL shown by ASP.NET Core.
Docker
The project includes a Dockerfile for containerized deployment.
Build the Docker image:
docker build -t neoxis-auth-app .

Run the container:
docker run -p 8080:8080 neoxis-auth-app

Deployment
The application is deployed using Render.
The production database is hosted using Supabase PostgreSQL.
GitHub → Render → ASP.NET Core → Supabase PostgreSQL

The PostgreSQL connection is provided through Render environment variables.
UI
The application uses the CoolAdmin Bootstrap admin dashboard template by Colorlib.
The template is used for the login, registration, dashboard, profile, navigation, and other UI components.
CoolAdmin is licensed under the MIT License.
Security
- ASP.NET Core Identity authentication
- Password hashing
- Server-side validation
- Authentication cookies
- Authorization for protected pages
- Anti-forgery protection
- Production database credentials stored as environment variables
Testing
The following authentication flows can be tested:
1. Register a new account
2. Log in with valid credentials
3. Try logging in with invalid credentials
4. Access the dashboard after login
5. Log out
6. Try accessing the dashboard after logout
7. Register another account
8. Verify that each account displays its own information



Author
Lisa Jose
GitHub:
https://github.com/lisaajose
Repository:
https://github.com/lisaajose/sign_up
