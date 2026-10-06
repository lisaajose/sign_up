# Neoxis Authentication App

## Project Overview

A professional ASP.NET Core web application integrating the **CoolAdmin** Bootstrap template to provide a fully functional authentication system. The application focuses on clean architecture, standard security practices, and a polished user interface.

## Features

- **User registration**: Secure account creation capturing Full Name, Email, and Password.
- **User login**: Authentic Identity-based login system with "Remember Me" functionality.
- **Authentication**: Validates users against a persistent SQLite database.
- **Authorization**: Restricts access to the Dashboard and Profile pages. Unauthenticated users are redirected to the Login page.
- **Protected dashboard**: Displays dynamic information for the authenticated user, such as name, email, and account status.
- **User profile**: Dedicated page displaying account details.
- **Logout**: Secure session termination.
- **Database persistence**: Entity Framework Core with SQLite ensures user accounts survive application restarts.
- **Validation**: Server-side validation with standard ASP.NET Core Data Annotations.
- **Secure password handling**: Passwords are mathematically hashed by ASP.NET Core Identity. Never stored in plaintext.

## Technology Stack

- **C# / ASP.NET Core 8**: Provides a robust, high-performance backend framework.
- **ASP.NET Core Identity**: Manages authentication, hashing, cookies, and user state securely and out-of-the-box.
- **Entity Framework Core**: Simplifies database interactions using an Object-Relational Mapper (ORM).
- **SQLite**: A lightweight, serverless database ideal for a simple reliable development setup.
- **CoolAdmin Template**: A free Bootstrap 5 admin dashboard template by Colorlib. Used for the UI foundation to ensure a professional look without reinventing the wheel.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Any modern IDE or Editor (Visual Studio, Visual Studio Code, JetBrains Rider)
- Git (optional, for cloning)

## Setup

1. **Open the project directory**
   Open your terminal and navigate to the project directory:
   ```bash
   cd NeoxisAuthApp
   ```

2. **Restore dependencies**
   Restore the required NuGet packages:
   ```bash
   dotnet restore
   ```

3. **Apply EF Core migrations**
   Create and seed the SQLite database using Entity Framework Core tools. (Make sure you have `dotnet-ef` installed):
   ```bash
   dotnet ef database update
   ```
   *(This will create an `app.db` file in your project directory.)*

4. **Run the application**
   Start the web server:
   ```bash
   dotnet run
   ```
   The application will usually be available at `http://localhost:5000` or `https://localhost:5001`.

## Testing

Follow this flow to manually test the application's capabilities:

1. **Register a user**: Click "Create one" on the login page. Fill out the registration form with valid data.
2. **Verify persistence**: You should be redirected to the Dashboard automatically. The dashboard should display your Full Name and Email dynamically.
3. **Logout**: Open the account dropdown in the top right corner and click "Logout".
4. **Authorization Check**: Try accessing `http://localhost:5000/Dashboard` directly. You should be redirected back to the Login page.
5. **Login**: Use the credentials you just created to sign back in.
6. **View profile**: Navigate to the Profile page using the sidebar or account dropdown to view your user details.
7. **Database verification**: Stop the application (`Ctrl+C`), then start it again (`dotnet run`). Your user account will still be active and can be used to log in, proving persistence.

## License & Attribution

This project incorporates the **CoolAdmin** template by Colorlib.
Copyright © Colorlib. All rights reserved. Template by [Colorlib](https://colorlib.com).
